using Modules;
using Views;

namespace Drone
{
    public interface IDroneState
    {
        void EnterState(DroneView drone);
        void UpdateState(DroneView drone);
        void ExitState(DroneView drone);
    }
}