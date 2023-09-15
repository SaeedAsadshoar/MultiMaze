using UnityEngine;

namespace Domain.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Data/Game Config", order = 0)]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private float _energyFillRate;
        [SerializeField] private int _maxEnergy;
        [SerializeField] private float _energyDecAmount;

        public float EnergyFillRate => _energyFillRate;
        public int MaxEnergy => _maxEnergy;
        public float EnergyDecAmount => _energyDecAmount;
    }
}