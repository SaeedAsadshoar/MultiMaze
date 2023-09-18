using UnityEngine;

namespace Domain.Data
{
    [CreateAssetMenu(fileName = "Level", menuName = "Data/LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private int _levelNo;
        [SerializeField] private int _ballCountInBase;
        [SerializeField] private int[] _ballNeededToFinish;

        [Header("Level Objects")] [Space(20)] [Header("With GameObject")] [SerializeField]
        private GameObject _levelPrefab;

        [SerializeField] private GameObject _cup;

        [Header("With Level Texture")] [SerializeField]
        private Texture2D _pipeTexture;

        [SerializeField] private GameObject _basePrefab;

        public int LevelNo => _levelNo;
        public int BallCountInBase => _ballCountInBase;
        public int[] BallNeededToFinish => _ballNeededToFinish;
        public GameObject LevelPrefab => _levelPrefab;
        public GameObject Cup => _cup;
        public Texture2D PipeTexture => _pipeTexture;
        public GameObject BasePrefab => _basePrefab;
    }
}