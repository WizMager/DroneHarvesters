using Drone;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Views
{
    [RequireComponent(typeof(NavMeshAgent), typeof(LineRenderer))]
    public class DroneView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        
        [field: SerializeField] public float ResourceCollectDistance { get; private set; } = 2;
        [field: SerializeField] public float BaseDestinationDistance { get; private set; } = 10;
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        [field: SerializeField] public bool IsDrawPath { get; private set; } = true;
        [field: SerializeField] public float DroneSpeed { get; private set; } = 10f;
        
        public bool IsActive { get; private set; }
        public IDroneController DroneController { get; private set; }
        public EFractionName Fraction { get; private set; }

        public void Initialize(
            IDroneController droneController,
            EFractionName fractionName
        )
        {
            DroneController = droneController;
            Fraction = fractionName;
        }
        
        public void SetDroneSpeed(float speed)
        {
            DroneSpeed = speed;
            Agent.speed = DroneSpeed;
        }
        
        public void SetIsDrawPath(bool isDrawPath)
        {
            IsDrawPath = isDrawPath;
        }
        
        private void Start()
        {
            if (Agent == null)
            {
                Agent = GetComponent<NavMeshAgent>();
            }

            if (_lineRenderer == null)
            {
                _lineRenderer = GetComponent<LineRenderer>();
            }
        }

        private void OnEnable()
        {
            IsActive = true;
        }

        private void OnDisable()
        {
            IsActive = false;
        }

        private void FixedUpdate()
        {
            if (!IsDrawPath || !Agent || Agent.path == null)
            {
                _lineRenderer.positionCount = 0;
                
                return;
            }

            UpdatePathVisualization();
        }
        
        private void UpdatePathVisualization()
        {
            var corners = Agent.path.corners;
        
            if (corners.Length < 2)
            {
                _lineRenderer.positionCount = 0;
                return;
            }

            _lineRenderer.positionCount = corners.Length;
            _lineRenderer.SetPositions(corners);
        }
    }
}