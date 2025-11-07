using System;
using Cysharp.Threading.Tasks;

namespace Modules.Drone.States
{
    public class DroneReturnState : IDroneState
    {
        public void EnterState(IDroneController droneController)
        {
            droneController.SetDroneState(EDroneState.Unload);
            
            UnloadResourceToBase(droneController).Forget();
        }

        private async UniTaskVoid UnloadResourceToBase(IDroneController droneController)
        {
            droneController.StartUnload();
            
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            
            if (!droneController.IsActive)
            {
                return;
            }
            
            droneController.ResourceUnload();
            droneController.ChangeState(new DroneIdleState());
        } 

        public void UpdateState(IDroneController droneController)
        {
        }

        public void ExitState(IDroneController droneController)
        {
        }
    }
}