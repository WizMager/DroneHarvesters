using System;
using Modules.Drone;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Views
{
    [RequireComponent(typeof(NavMeshAgent), typeof(LineRenderer))]
    public class DroneView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private ParticleSystem _unloadEffect;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _droneStatus;
        [SerializeField] private SpriteRenderer _droneStatusIcon;

        private Camera _camera;
        
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
            EFractionName fractionName,
            Camera camera
        )
        {
            DroneController = droneController;
            Fraction = fractionName;
            _camera = camera;
            
            SetFractionColor(fractionName);
        }

        private void SetFractionColor(EFractionName fractionName)
        {
            var materialPropertyBlock = new MaterialPropertyBlock();
            _meshRenderer.GetPropertyBlock(materialPropertyBlock);
            
            switch (fractionName)
            {
                case EFractionName.Red:
                    materialPropertyBlock.SetColor("_BaseColor", Color.red);
                    break;
                case EFractionName.Blue:
                    materialPropertyBlock.SetColor("_BaseColor", Color.blue);
                    break;
            }
            
            _meshRenderer.SetPropertyBlock(materialPropertyBlock);
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

        public void UnloadActivation(bool isEnable)
        {
            if (isEnable)
            {
                _unloadEffect.Play();
            }
            else
            {
                _unloadEffect.Stop();
            }
        }
        
        public void SetDroneStateIcon(Sprite icon)
        {
            _droneStatusIcon.sprite = icon;
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

        private void Update()
        {
            _droneStatus.LookAt(_droneStatus.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
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