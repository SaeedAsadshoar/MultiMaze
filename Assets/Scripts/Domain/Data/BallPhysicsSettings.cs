using System;
using Domain.Enum;
using UnityEngine;

namespace Domain.Data
{
    [Serializable]
    public class BallPhysicsSettings
    {
        [SerializeField] private BallTypes _ballType;
        [SerializeField] private float _dynamicFriction;
        [SerializeField] private float _staticFriction;
        [SerializeField] private float _bounciness;

        public BallTypes BallType => _ballType;
        public float DynamicFriction => _dynamicFriction;
        public float StaticFriction => _staticFriction;
        public float Bounciness => _bounciness;
    }
}