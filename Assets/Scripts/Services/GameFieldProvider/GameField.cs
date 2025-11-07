using UnityEngine;
using Views;

namespace Services.GameFieldProvider
{
    public class GameField : MonoBehaviour
    {
        [field: SerializeField] public ResourcesSpawnArea ResourcesSpawnArea { get; private set; }
        [field: SerializeField] public GameObject ResourcePrefab { get; private set; }
        [field: SerializeField] public GameObject DronePrefab { get; private set; }
        [field: SerializeField] public BaseView RedBase { get; private set; }
        [field: SerializeField] public BaseView BlueBase { get; private set; }
        [field: SerializeField] public Camera Camera { get; private set; }
    }
}

