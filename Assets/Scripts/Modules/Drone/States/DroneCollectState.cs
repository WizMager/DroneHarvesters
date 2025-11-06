using System;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Modules.Drone.States
{
    public class DroneCollectState : IDroneState
    {
        public void EnterState(IDroneController droneController)
        {
            if (droneController.FreeResourcesList.Contains(droneController.TargetResource))
            {
                droneController.StartHarvestResource();
                
                ResourceHarvesting(droneController).Forget();
            }
            else
            {
                droneController.ChangeState(new DroneSearchState());
            }
        }

        private async UniTaskVoid ResourceHarvesting(IDroneController droneController)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            
            droneController.SetDestination(droneController.BasePosition);
        }

        public void UpdateState(IDroneController droneController)
        {
            var sqrDistanceToBase = (droneController.CurrentDronePosition - droneController.BasePosition).sqrMagnitude;
            if (sqrDistanceToBase < droneController.BaseDestinationDistance * droneController.BaseDestinationDistance)
            {
                droneController.ChangeState(new DroneReturnState());
            }
        }

        public void ExitState(IDroneController droneController)
        {
        }
    }
}