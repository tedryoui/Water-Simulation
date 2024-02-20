using System;
using UnityEngine;

namespace Water_Simulation
{
    public class ClampRotation : MonoBehaviour
    {
        [SerializeField] private Vector2 _zRotationRange;
        [SerializeField] private Vector2 _xRotationRange;

        private void Update()
        {
            var zRot = Mathf.Clamp(transform.localRotation.eulerAngles.z, _zRotationRange.x, _zRotationRange.y);
            var xRot = Mathf.Clamp(transform.localRotation.eulerAngles.x, _xRotationRange.x, _xRotationRange.y);
            
            transform.localRotation = Quaternion.Euler(xRot, 0.0f, zRot);
        }
    }
}