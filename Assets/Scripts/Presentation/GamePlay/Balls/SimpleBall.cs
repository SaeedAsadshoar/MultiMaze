using System;
using Presentation.GamePlay.Balls.Abstract;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Presentation.GamePlay.Balls
{
    public class SimpleBall : Ball<SimpleBall>
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Color[] _colors;
        [SerializeField] private float _minScale;
        [SerializeField] private float _maxScale;

        private void OnEnable()
        {
            _meshRenderer.material.color = _colors[Random.Range(0, _colors.Length)];
            transform.localScale = Vector3.one * Random.Range(_minScale, _maxScale);
        }
    }
}