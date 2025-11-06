using System.Linq;

namespace Modules.Drone.States
{
    public class DroneSearchState : IDroneState
    {
        public void EnterState(IDroneController droneController)
        {
            if (droneController.FreeResourcesList.Count == 0)
            {
                droneController.ChangeState(new DroneIdleState());
                return;
            }

            droneController.SetDroneState(EDroneState.Run);
            
            var closestResource = droneController.FreeResourcesList[0];
            var closesDistance = (droneController.CurrentDronePosition - droneController.FreeResourcesList[0].transform.position).sqrMagnitude;
            
            foreach (var resourceView in droneController.FreeResourcesList)
            {
                var checkDist = (droneController.CurrentDronePosition - resourceView.transform.position).sqrMagnitude;
                
                if (checkDist > closesDistance)
                    continue;

                closesDistance = checkDist;
                closestResource = resourceView;
            }
            
            droneController.SetTarget(closestResource);
            droneController.SetDestination(closestResource.transform.position);
        }

        public void UpdateState(IDroneController droneController)
        {
            if (!droneController.FreeResourcesList.Contains(droneController.TargetResource)) 
            {
                droneController.ChangeState(new DroneIdleState());
                return;
            }
            
            var sqrDistance = (droneController.CurrentDronePosition - droneController.TargetResource.transform.position).sqrMagnitude;
            
            if (sqrDistance < droneController.ResourceCollectDistance * droneController.ResourceCollectDistance)
            {
                droneController.ChangeState(new DroneCollectState());
            }
        }

        public void ExitState(IDroneController droneController)
        {
        }
    }
}