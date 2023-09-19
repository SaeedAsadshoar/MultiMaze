using System;
using UnityEngine;

namespace Domain.Data
{
    [Serializable]
    public class ColorPalette
    {
        [SerializeField] private Texture2D[] _colorTexture;

        public Texture2D[] ColorTexture => _colorTexture;
    }
}