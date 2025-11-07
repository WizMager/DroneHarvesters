using System;
using System.Collections.Generic;
using Db.Drone;
using Modules.Drone.States;
using UnityEngine;
using Utils;
using Views;

namespace Modules.Drone
{
    public class DroneController : IDroneController
    {
        private readonly DroneView _drone;
        private readonly DroneData _droneData;
        
        private Transform _baseTransform;
        private IDroneState _currentState;
        
        public ResourceView TargetResource { get; private set; }
        public Vector3 CurrentDronePosition => _drone.transform.position;
        public Vector3 BasePosition => _baseTransform.position;
        public float ResourceCollectDistance => _drone.ResourceCollectDistance;
        public float BaseDestinationDistance => _drone.BaseDestinationDistance;
        public bool IsActive => _drone != null && _drone.IsActive;
        public IReadOnlyList<ResourceView> FreeResourcesList { get; }
        public Action<ResourceView> OnHarvestResource { get; set; }
        public Action<EFractionName> OnResourceUnload { get; set; }

        public DroneController(
            DroneView drone, 
            IReadOnlyList<ResourceView> freeResourcesList,
            DroneData droneData
        )
        {
            _drone = drone;
            FreeResourcesList = freeResourcesList;
            _droneData = droneData;
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

        public void StartUnload()
        {
            _drone.UnloadActivation(true);
        }
        
        public void ResourceUnload()
        {
            _drone.UnloadActivation(false);
            OnResourceUnload?.Invoke(_drone.Fraction);
        }

        public void StartHarvestResource()
        {
            OnHarvestResource?.Invoke(TargetResource);
        }

        public void SetDroneState(EDroneState state)
        {
            if (_drone != null && _drone.IsActive)
            {
                _drone.SetDroneStateIcon(_droneData.GetStatusIcon(state));
            }
        }
    }
}