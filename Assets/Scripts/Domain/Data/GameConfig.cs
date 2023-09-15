using UnityEngine;

namespace Domain.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Data/Game Config", order = 0)]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private LevelSequence _levelSequence;
        public LevelSequence LevelSequence => _levelSequence;
    }
}