namespace Drone
{
    public interface IDroneState
    {
        void EnterState(IDroneController droneController);
        void UpdateState(IDroneController droneController);
        void ExitState(IDroneController droneController);
    }
}