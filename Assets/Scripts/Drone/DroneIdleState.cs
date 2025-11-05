using System;
using Cysharp.Threading.Tasks;

namespace Drone
{
    public class DroneIdleState : IDroneState
    {
        public void EnterState(IDroneController droneController)
        {
            WaitUntilHasFreeResources(droneController).Forget();
        }
        
        private async UniTaskVoid WaitUntilHasFreeResources(IDroneController droneController)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.3));
            
            await UniTask.WaitUntil(() => droneController.FreeResourcesList.Count > 0);
            
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