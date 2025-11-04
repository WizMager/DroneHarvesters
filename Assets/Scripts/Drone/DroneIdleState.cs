using Views;

namespace Drone
{
    public class DroneIdleState : IDroneState
    {
        public void EnterState(DroneView drone)
        {
            drone.ChangeState(new DroneSearchState());
        }

        public void UpdateState(DroneView drone)
        {
        }

        public void ExitState(DroneView drone)
        {
        }
    }
}