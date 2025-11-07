using UnityEngine;

namespace Services.GameFieldProvider.Impl
{
    public class GameFieldProvider : MonoBehaviour, IGameFieldProvider
    {
        [field:SerializeField] public GameField GameField { get; private set; }
    }
}