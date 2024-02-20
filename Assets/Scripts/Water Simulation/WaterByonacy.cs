using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Water_Simulation
{
    public class WaterBuoyancy : MonoBehaviour
    {
        [SerializeField] private WaterHandler _handler;
        [SerializeField] private bool _prepare;

        [SerializeField] private float _power;
        [SerializeField] private float _maxDistantPower;

        [SerializeField] private float _drag;
        [SerializeField] private float _angularDrag;

        [SerializeField] private Transform[] _markers;

        [SerializeField] private Rigidbody _rb;

        private IEnumerator Start()
        {
            yield return null;
            
            if (_prepare)
            {
                var waveHeight = _handler.GetWaveHeight(transform.position);
                var position = transform.position;
                position.y = waveHeight;

                transform.position = position;
            }
        }

        private void FixedUpdate()
        {
            var force = (-Physics.gravity) / _markers.Length;

            foreach (var marker in _markers)
            {
                var targetHeight = _handler.GetWaveHeight(marker.position);
                var difference = marker.position.y - targetHeight;
                if (difference <= 0)
                {
                    _rb.AddForceAtPosition(force + Vector3.up * Mathf.Clamp(-difference, 0, _maxDistantPower) * _power, marker.position);

                    _rb.drag = _drag;
                    _rb.angularDrag = _angularDrag;
                }
                else
                {
                    _rb.drag = 0.0f;
                    _rb.angularDrag = _angularDrag;
                }
            }
        }
    }
}