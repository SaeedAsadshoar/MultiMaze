using UnityEngine;

namespace Presentation.GamePlay.Levels.Interface
{
    public interface ILevelGenerator
    {
        void GenerateLevel(Texture2D levelArt);
    }
}