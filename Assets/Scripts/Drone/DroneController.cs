using System;
using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Drone
{
    public class DroneController : IDroneController
    {
        private readonly DroneView _drone;
        
        private Transform _baseTransform;
        private IDroneState _currentState;
        
        public ResourceView TargetResource { get; private set; }
        public Vector3 CurrentDronePosition => _drone.transform.position;
        public Vector3 BasePosition => _baseTransform.position;
        public float ResourceCollectDistance => _drone.ResourceCollectDistance;
        public float BaseDestinationDistance => _drone.BaseDestinationDistance;
        public IReadOnlyList<ResourceView> FreeResourcesList { get; }
        public Action<ResourceView> OnHarvestResource { get; set; }

        public DroneController(DroneView drone, IReadOnlyList<ResourceView> freeResourcesList)
        {
            _drone = drone;
            FreeResourcesList = freeResourcesList;
            
            _drone.Initialize(this);
        }

        public void Initialize(BaseView baseView)
        {
            _baseTransform = baseView.transform;
        }

        public void Start()
        {
            ChangeState(new DroneIdleState());
        }

        public void Update()
        {
            if (!_drone.IsActive)
                return;
            
            _currentState?.UpdateState(this);
        }
        
        public void ChangeState(IDroneState newState)
        {
            _currentState?.ExitState(this);
            _currentState = newState;
            _currentState?.EnterState(this);
        }

        public void SetTarget(ResourceView target)
        {
            TargetResource = target;
        }

        public void SetDestination(Vector3 position)
        {
            _drone?.Agent?.SetDestination(position);
        }

        public void ResourceUnload()
        {
            //TODO: add resource to base storage
        }

        public void StartHarvestResource()
        {
            OnHarvestResource?.Invoke(TargetResource);
        }
    }
}