using UnityEngine;

namespace Views
{
    public class ResourceView : MonoBehaviour
    {
        public bool IsClaimed { get; set; }

        private void OnDisable()
        {
            IsClaimed = false;
        }
    }
}