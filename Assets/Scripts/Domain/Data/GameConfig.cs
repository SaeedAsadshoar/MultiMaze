using UnityEngine;

namespace Domain.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Data/Game Config", order = 0)]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private bool _isLoadLevelByObjectPrefab;
        [SerializeField] private LevelSequence _levelSequence;

        [Header("Touch sensitivity"), SerializeField]
        private TouchSettings _touchSettings;

        [Header("Balls Physics"), SerializeField]
        private BallPhysicsSettings[] _ballPhysicsSettings;

        public bool IsLoadLevelByObjectPrefab => _isLoadLevelByObjectPrefab;
        public LevelSequence LevelSequence => _levelSequence;
        public TouchSettings TouchSettings => _touchSettings;
        public BallPhysicsSettings[] BallPhysicsSettings => _ballPhysicsSettings;
    }
}