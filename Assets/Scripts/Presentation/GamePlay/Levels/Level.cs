using Presentation.GamePlay.Levels.Interface;
using UnityEngine;

namespace Presentation.GamePlay.Levels
{
    public class Level : MonoBehaviour, ILevel
    {
        [SerializeField] private Rigidbody _rigidbody;
        public Rigidbody LevelRigidbody => _rigidbody;
    }
}