using UnityEngine;

namespace Services.GameFieldProvider
{
    public class GameFieldProvider : MonoBehaviour
    {
        [field:SerializeField] public GameField GameField { get; private set; }
    }
}