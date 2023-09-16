using UnityEngine;

namespace Presentation.GamePlay.Balls.Interface
{
    public interface IBall
    {
        Transform RootTransform { get; }
        Rigidbody ObjectRigidbody { get; }
        void Kill();
    }
}