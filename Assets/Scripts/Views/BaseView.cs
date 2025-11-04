using UnityEngine;
using Utils;

namespace Views
{
    public class BaseView : MonoBehaviour
    {
        [SerializeField] private EFractionName _fraction;
        [SerializeField] private MeshRenderer _meshRenderer;

        private void Start()
        {
            switch (_fraction)
            {
                case EFractionName.Red:
                    _meshRenderer.material.SetColor("_Color", Color.red);
                    break;
                case EFractionName.Blue:
                    _meshRenderer.material.SetColor("_Color", Color.blue);
                    break;
            }
        }
    }
}