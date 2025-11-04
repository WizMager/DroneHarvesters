using Views;

namespace Drone
{
    public class DroneReturnState : IDroneState
    {
        public void EnterState(DroneView drone)
        {
            drone.homeBase.AddResource(1);
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