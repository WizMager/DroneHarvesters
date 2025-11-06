using UnityEngine;

namespace Services.Input
{
    public interface IInputService
    {
        Vector2 MoveDirection { get; }
        bool IsMoved { get; }

        void Initialize();
    }
}