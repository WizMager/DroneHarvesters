using System;
using System.Collections.Generic;
using Modules.Drone.States;
using UnityEngine;

namespace Db.Drone
{
    [CreateAssetMenu(fileName = "DroneData", menuName = "Data/DroneData")]
    public class DroneData : ScriptableObject
    {
        [Header("Drone Settings")]
        [SerializeField] private float _defaultDroneSpeed = 5f;
        [SerializeField] private bool _defaultIsDronePathEnabled = true;
        [SerializeField] private int _defaultDroneCount = 3;
        
        [Header("Icons")]
        [SerializeField] private List<DroneState> _droneIcons;
        
        public float DefaultDroneSpeed => _defaultDroneSpeed;
        public bool DefaultIsDronePathEnabled => _defaultIsDronePathEnabled;
        public int DefaultDroneCount => _defaultDroneCount;
        
        public Sprite GetStatusIcon(EDroneState droneState)
        {
            foreach (var drone in _droneIcons)
            {
                if (drone.State != droneState)
                    continue;

                return drone.Icon;
            }
            
            throw new Exception($"[{nameof(DroneData)}]: No icon found for state: {droneState}");
        }
    }
}