using UnityEngine;

namespace Domain.Data
{
    [CreateAssetMenu(fileName = "Level", menuName = "Data/LevelData")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private int _levelNo;
        [SerializeField] private int _ballCountInBase;
        [SerializeField] private int _ballNeededToFinish;
        [SerializeField] private Vector3 _ballSpawnPosition;
        [SerializeField] private GameObject _levelPrefab;

        public int LevelNo => _levelNo;
        public int BallCountInBase => _ballCountInBase;
        public int BallNeededToFinish => _ballNeededToFinish;
        public Vector3 BallSpawnPosition => _ballSpawnPosition;
        public GameObject LevelPrefab => _levelPrefab;
    }
}