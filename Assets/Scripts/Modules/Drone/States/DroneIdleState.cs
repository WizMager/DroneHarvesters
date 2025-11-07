using System;
using Cysharp.Threading.Tasks;

namespace Modules.Drone.States
{
    public class DroneIdleState : IDroneState
    {
        public void EnterState(IDroneController droneController)
        {
            WaitUntilHasFreeResources(droneController).Forget();
        }
        
        private async UniTaskVoid WaitUntilHasFreeResources(IDroneController droneController)
        {
            droneController.SetDroneState(EDroneState.Idle);
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.3));
            
            await UniTask.WaitUntil(() => droneController.FreeResourcesList.Count > 0);
            
            if (!droneController.IsActive)
            {
                return;
            }
            
            droneController.ChangeState(new DroneSearchState());
        }

        public void UpdateState(IDroneController droneController)
        {
        }

        public void ExitState(IDroneController droneController)
        {
        }
    }
}