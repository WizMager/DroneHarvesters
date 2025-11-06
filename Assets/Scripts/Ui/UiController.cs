using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _redScore;
        [SerializeField] private TMP_Text _blueScore;

        [SerializeField] private Slider _droneCount;
        [SerializeField] private Slider _droneSpeed;
        [SerializeField] private TMP_InputField _resourceSpawnTimer;
        [SerializeField] private Toggle _dronePathToggle;
    }
}