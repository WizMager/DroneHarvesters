using Drone;
using UnityEngine;
using UnityEngine.AI;

namespace Views
{
    public class DroneView : MonoBehaviour
    {
        [field: SerializeField] public float ResourceCollectDistance { get; private set; } = 2;
        [field: SerializeField] public float BaseDestinationDistance { get; private set; } = 10;
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        
        public bool IsActive { get; private set; }
        public IDroneController DroneController { get; private set; }

        public void Initialize(IDroneController droneController)
        {
            DroneController = droneController;
        }
        
        private void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            IsActive = true;
        }

        private void OnDisable()
        {
            IsActive = false;
        }

        private void OnDrawGizmos()
        {
            if (!Agent || Agent.path == null) return;

            var corners = Agent.path.corners;
            if (corners.Length < 2) return;

            Gizmos.color = Color.cyan;

            for (var i = 0; i < corners.Length - 1; i++)
            {
                Gizmos.DrawLine(corners[i], corners[i + 1]);
                Gizmos.DrawSphere(corners[i], 0.1f);
            }
        }
    }
}