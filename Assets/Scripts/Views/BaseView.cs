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
            var materialPropertyBlock = new MaterialPropertyBlock();
            _meshRenderer.GetPropertyBlock(materialPropertyBlock);
            
            switch (_fraction)
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
    }
}