using System;
using System.Globalization;
using Db;
using Db.Drone;
using Db.Resource;
using Services.StoreResource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ui
{
    public class UiController : MonoBehaviour
    {
        public Action<EFractionName, int> OnDroneCountChanged;
        public Action<float> OnDroneSpeedChanged;
        public Action<float> OnResourceSpawnTimerChanged;
        public Action<bool> OnDronePathToggleChanged;
        
        [SerializeField] private TMP_Text _redScore;
        [SerializeField] private TMP_Text _blueScore;
        [SerializeField] private TMP_Text _redDroneCountText;
        [SerializeField] private TMP_Text _blueDroneCountText;
        [SerializeField] private TMP_Text _droneSpeedText;
        [SerializeField] private TMP_Text _resourceSpawnTimerText;

        [SerializeField] private Slider _redDroneCount;
        [SerializeField] private Slider _blueDroneCount;
        [SerializeField] private Slider _droneSpeed;
        [SerializeField] private TMP_InputField _resourceSpawnTimer;
        [SerializeField] private Toggle _dronePathToggle;

        private IResourceStorageService _resourceStorageService;

        public void Initialize(IResourceStorageService resourceStorageService, DroneData droneData, ResourceData resourceData)
        {
            _resourceStorageService = resourceStorageService;
            _resourceStorageService.OnResourceChange += OnResourceChanged;
            
            SetDefaultValues(droneData, resourceData);
        }
        
        private void SetDefaultValues(DroneData droneData, ResourceData resourceData)
        {
            _redDroneCount.value = droneData.DefaultDroneCount;
            _blueDroneCount.value = droneData.DefaultDroneCount;
            _droneSpeed.value = droneData.DefaultDroneSpeed;
            _resourceSpawnTimer.text = resourceData.ResourceSpawnSpeed.ToString(CultureInfo.InvariantCulture);
            _dronePathToggle.isOn = droneData.DefaultIsDronePathEnabled;
            
            _redDroneCountText.text = droneData.DefaultDroneCount.ToString();
            _blueDroneCountText.text = droneData.DefaultDroneCount.ToString();
            _droneSpeedText.text = droneData.DefaultDroneSpeed.ToString(CultureInfo.InvariantCulture);
            _resourceSpawnTimerText.text = resourceData.ResourceSpawnSpeed.ToString(CultureInfo.InvariantCulture);
        }

        private void OnResourceChanged(EFractionName fractionName, int score)
        {
            switch (fractionName)
            {
                case EFractionName.Red:
                    _redScore.text = score.ToString();
                    break;
                case EFractionName.Blue:
                    _blueScore.text = score.ToString();
                    break;
            }
        }
        
        private void Start()
        {
            _redDroneCount.onValueChanged.AddListener(OnRedDroneCountChange);
            _blueDroneCount.onValueChanged.AddListener(OnBlueDroneCountChange);
            _droneSpeed.onValueChanged.AddListener(OnDroneSpeedChange);
            _resourceSpawnTimer.onValueChanged.AddListener(OnResourceSpawnTimerChange);
            _dronePathToggle.onValueChanged.AddListener(OnDronePathToggleChange);
        }

        private void OnRedDroneCountChange(float value)
        {
            var newDroneCount = (int)value;
            _redDroneCountText.text = newDroneCount.ToString();
            OnDroneCountChanged?.Invoke(EFractionName.Red, newDroneCount);
        }

        private void OnBlueDroneCountChange(float value)
        {
            var newDroneCount = (int)value;
            _blueDroneCountText.text = newDroneCount.ToString();
            OnDroneCountChanged?.Invoke(EFractionName.Blue, newDroneCount);
        }

        private void OnDroneSpeedChange(float value)
        {
            _droneSpeedText.text = value.ToString(CultureInfo.InvariantCulture);
            OnDroneSpeedChanged?.Invoke(value);
        }
        
        private void OnResourceSpawnTimerChange(string value)
        {
            if (!int.TryParse(value, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out var newTimer))
                return;

            _resourceSpawnTimerText.text = newTimer.ToString();
            OnResourceSpawnTimerChanged?.Invoke(newTimer);
        }
        
        private void OnDronePathToggleChange(bool value)
        {
            OnDronePathToggleChanged?.Invoke(value);
        }
    }
}