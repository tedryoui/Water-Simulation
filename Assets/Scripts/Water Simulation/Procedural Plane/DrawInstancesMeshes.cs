using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Water_Simulation.Procedural_Plane
{
    public class DrawInstancesMeshes : MonoBehaviour
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;

        [SerializeField] private ShadowCastingMode _castShadow;
        [SerializeField] private bool _receiveShadow;
        
        [Header("Information")] 
        [SerializeField] private int gridSize;
        [SerializeField] private float meshSize;

        Matrix4x4[] _matrices;

        private void Start()
        {
            PrepareMatrices();
        }

        private void PrepareMatrices()
        {
            var pivotDelta = Vector3.right * meshSize / 2.0f + Vector3.forward * meshSize / 2.0f;
            var centerPivotDelta =
                Vector3.left * (meshSize * gridSize) / 2.0f + Vector3.back * (meshSize * gridSize) / 2.0f;
            
            List<Matrix4x4> matrices = new List<Matrix4x4>();
            for (int x = 0; x < gridSize; x++)
                for (int z = 0; z < gridSize; z++)
                    matrices.Add(Matrix4x4.TRS(
                        (Vector3.zero + pivotDelta + centerPivotDelta) + Vector3.right * x * meshSize + Vector3.forward * z * meshSize,
                        Quaternion.identity, Vector3.one
                        ));

            _matrices = matrices.ToArray();
        }

        private void Update()
        {
            Graphics.DrawMeshInstanced(_mesh, 0, _material, _matrices, _matrices.Length,
                new MaterialPropertyBlock(), _castShadow, _receiveShadow);
        }
    }
}