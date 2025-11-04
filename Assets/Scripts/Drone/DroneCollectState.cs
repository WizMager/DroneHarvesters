using Modules;
using UnityEngine;
using Views;

namespace Drone
{
    public class DroneCollectState : IDroneState
    {
        public void EnterState(DroneView drone)
        {
            if (drone.targetResource)
            {
                Object.Destroy(drone.targetResource);
                drone.targetResource = null;
            }

            drone.agent.SetDestination(drone.homeBase.transform.position);
        }

        public void UpdateState(DroneView drone)
        {
            if (Vector3.Distance(drone.transform.position, drone.homeBase.transform.position) < drone.collectDistance * 5)
            {
                drone.ChangeState(new DroneReturnState());
            }
        }

        public void ExitState(DroneView drone)
        {
        }
    }
}