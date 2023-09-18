using UnityEngine;

namespace Domain.Data
{
    [CreateAssetMenu(fileName = "Level", menuName = "Data/LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private int _levelNo;
        [SerializeField] private int _ballCountInBase;
        [SerializeField] private int[] _ballNeededToFinish;

        [SerializeField] private GameObject _levelPrefab;
        [SerializeField] private GameObject _cup;

        public int LevelNo => _levelNo;
        public int BallCountInBase => _ballCountInBase;
        public int[] BallNeededToFinish => _ballNeededToFinish;
        public GameObject LevelPrefab => _levelPrefab;
        public GameObject Cup => _cup;
    }
}