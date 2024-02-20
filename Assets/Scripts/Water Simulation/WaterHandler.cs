using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Water_Simulation
{
    public class WaterHandler : MonoBehaviour
    {
        [Serializable]
        public struct Wave
        {
            public float frequency;
            public Vector2 direction;
            public float speed;
            public float amplitude;
        };

        [SerializeField] private int _seed;
        [SerializeField] private int _wavesDetalization;
        [SerializeField] private float _wavesFrequencyMultiplier;
        [SerializeField] private float _wavesAmplitudeMultiplier;
        private List<Wave> _waves;
        [SerializeField] private float _waterAmplitude;
        [SerializeField] private float _waterFrequency;
        [FormerlySerializedAs("_speed")] [SerializeField] private float _waterSpeed;
        [SerializeField] private Material _waterMaterial;

        private ComputeBuffer _buffer;
        
        private void Start()
        {
            _waves = new List<Wave>();
            RecomputeWaves();
            
            ResetData(); 
        }

        private void ResetData()
        {
            _buffer?.Dispose();
            _buffer = new ComputeBuffer(_waves.ToArray().Length, sizeof(float) * 5, ComputeBufferType.Structured);
            _buffer.SetData(_waves.ToArray());
            
            _waterMaterial.SetBuffer("waves", _buffer);
            _waterMaterial.SetInt("wavesCount", _waves.ToArray().Length);
        }

        private void RecomputeWaves()
        {
            UnityEngine.Random.InitState(_seed);
            
            var amplitude = 1.0f;
            var frequency = 1.0f;
            var direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));

            for (int i = 0; i < _wavesDetalization; i++)
            {
                _waves.Add(new Wave()
                {
                    frequency = frequency * _waterFrequency,
                    direction = direction,
                    speed = _waterSpeed,
                    amplitude = amplitude * _waterAmplitude
                });

                frequency *= _wavesFrequencyMultiplier;
                amplitude *= _wavesAmplitudeMultiplier;
                direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            }
            
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        }

        public float GetWaveHeight(Vector3 position)
        {
            var displacement = 0.0f;

            if (_waves == null) return displacement; 
            
            for (int i = 0; i < _waves.Count; i++)
            {
                float frequency = _waves[i].frequency;
                Vector2 direction = _waves[i].direction;
                float speed = _waves[i].speed;
                float amplitude = _waves[i].amplitude;

                float xPhase = position.x * (-direction.x) + Time.time * speed;
                float zPhase = position.z * (-direction.y) + Time.time * speed;

                displacement += amplitude * Mathf.Exp(Mathf.Sin((xPhase + zPhase) * frequency));
            }

            return displacement;
        }

        private void OnDestroy()
        {
            _buffer?.Dispose();
        }
    }
}