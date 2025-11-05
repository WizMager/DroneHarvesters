using System;
using Cysharp.Threading.Tasks;

namespace Drone
{
    public class DroneReturnState : IDroneState
    {
        public void EnterState(IDroneController droneController)
        {
            UnloadResourceToBase(droneController).Forget();
        }

        private async UniTaskVoid UnloadResourceToBase(IDroneController droneController)
        {
            //TODO: add visual effect of resource unload
            
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            
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