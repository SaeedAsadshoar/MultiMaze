using System;
using UnityEngine;

namespace Domain.Data
{
    [Serializable]
    public class TouchSettings
    {
        [SerializeField, Range(1.0f, 100.0f)] private float _stopRotateSpeed = 20;
        [SerializeField, Range(0.001f, 0.02f)] private float _deltaDistanceRatio = 0.005f;
        [SerializeField, Range(0.001f, 0.02f)] private float _centerDistanceRatio = 0.005f;

        public float StopRotateSpeed => _stopRotateSpeed;
        public float DeltaDistanceRatio => _deltaDistanceRatio;
        public float CenterDistanceRatio => _centerDistanceRatio;
    }
}