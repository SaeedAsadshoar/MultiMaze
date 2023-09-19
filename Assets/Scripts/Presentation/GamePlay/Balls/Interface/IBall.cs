using Domain.Data;
using UnityEngine;

namespace Presentation.GamePlay.Balls.Interface
{
    public interface IBall
    {
        Transform RootTransform { get; }
        Rigidbody ObjectRigidbody { get; }
        PhysicMaterial PhysicMaterial { get; }
        bool IsInsideCup { get; }
        void Kill();
        void MoveInsideCup();
        void ExitPuzzle();
        void EnterPuzzle();
        void SetColorPalette(ColorPalette colorPalette);
    }
}