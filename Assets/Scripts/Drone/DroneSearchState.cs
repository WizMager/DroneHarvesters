using System.Linq;
using Modules;
using UnityEngine;
using Views;

namespace Drone
{
    public class DroneSearchState : IDroneState
    {
        public void EnterState(DroneView drone)
        {
            var resources = GameObject.FindGameObjectsWithTag("Resource");
            if (resources.Length == 0)
            {
                // Нет ресурсов — ждать
                drone.ChangeState(new DroneIdleState());
                return;
            }

            drone.targetResource = resources
                .OrderBy(r => Vector3.Distance(drone.transform.position, r.transform.position))
                .First();

            drone.agent.SetDestination(drone.targetResource.transform.position);
        }

        public void UpdateState(DroneView drone)
        {
            if (!drone.targetResource) 
            {
                drone.ChangeState(new DroneSearchState());
                return;
            }

            if (Vector3.Distance(drone.transform.position, drone.targetResource.transform.position) < drone.collectDistance)
            {
                drone.ChangeState(new DroneCollectState());
            }
        }

        public void ExitState(DroneView drone)
        {
        }
    }
}