using Domain.Data;
using Domain.Enum;
using Presentation.GamePlay.Balls.Abstract;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Presentation.GamePlay.Balls
{
    public class SimpleBall : Ball<SimpleBall>
    {
        [SerializeField] private BallTypes _ballType;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private float _minScale;
        [SerializeField] private float _maxScale;

        private BallPhysicsSettings _ballPhysicsSetting;

        private void OnEnable()
        {
            transform.localScale = Vector3.one * Random.Range(_minScale, _maxScale);
            _ballPhysicsSetting = GameConfigService.GetBallPhysicsSetting(_ballType);
            PhysicMaterial.bounciness = _ballPhysicsSetting.Bounciness;
            PhysicMaterial.dynamicFriction = _ballPhysicsSetting.DynamicFriction;
            PhysicMaterial.staticFriction = _ballPhysicsSetting.StaticFriction;
        }

#if UNITY_EDITOR
        private void Update()
        {
            PhysicMaterial.bounciness = _ballPhysicsSetting.Bounciness;
            PhysicMaterial.dynamicFriction = _ballPhysicsSetting.DynamicFriction;
            PhysicMaterial.staticFriction = _ballPhysicsSetting.StaticFriction;
        }
#endif

        public override void SetColorPalette(ColorPalette colorPalette)
        {
            _meshRenderer.material.mainTexture = colorPalette.ColorTexture[Random.Range(0, colorPalette.ColorTexture.Length)];
        }
    }
}