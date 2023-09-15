using UnityEngine;

namespace Domain.Data
{
    [CreateAssetMenu(fileName = "LevelSequence", menuName = "Data/LevelsSequence", order = 1)]
    public class LevelSequence : ScriptableObject
    {
        [SerializeField] private LevelData[] _allLevelsData;

        public LevelData[] AllLevelsData => _allLevelsData;
    }
}