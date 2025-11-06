using UnityEngine;

using System.Collections.Generic;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private RectTransform _mapRect;
    [SerializeField] private Vector2 _worldOrigin = new Vector2(-125f, -150f);
    [SerializeField] private Vector2 _worldSize = new Vector2(250f, 300f);

    [Header("Prefabs")]
    [SerializeField] private Image _iconRedDronePrefab;
    [SerializeField] private Image _iconBlueDronePrefab;
    [SerializeField] private Image _iconBaseRedPrefab;
    [SerializeField] private Image _iconBaseBluePrefab;
    [SerializeField] private Image _iconResourcePrefab;

    [Header("Parents")]
    [SerializeField] private RectTransform _iconsContainer;

    private readonly List<Transform> _redUnits = new();
    private readonly List<Transform> _blueUnits = new();
    private readonly List<Image> _redIcons = new();
    private readonly List<Image> _blueIcons = new();
    private readonly List<Image> _resourceIcons = new();

    public void RegisterUnit(Transform unit, bool isRed)
    {
        if (isRed)
        {
            _redUnits.Add(unit);
            var icon = Instantiate(_iconRedDronePrefab, _iconsContainer);
            _redIcons.Add(icon);
        }
        else
        {
            _blueUnits.Add(unit);
            var icon = Instantiate(_iconBlueDronePrefab, _iconsContainer);
            _blueIcons.Add(icon);
        }
    }
    
    public void UnregisterUnit(Transform unit, bool isRed)
    {
        Destroy(unit.gameObject);
        
        if (isRed)
        {
            _redUnits.Remove(unit);
        }
        else
        {
            _blueUnits.Remove(unit);
        }
    }

    public void RegisterBase(Transform baseTransform, bool isRed)
    {
        var prefab = isRed ? _iconBaseRedPrefab : _iconBaseBluePrefab;
        var icon = Instantiate(prefab, _iconsContainer);
        icon.transform.localPosition = WorldToMap(baseTransform.position);
        icon.gameObject.SetActive(true);
    }

    public void RegisterResource(Transform resource)
    {
        var icon = Instantiate(_iconResourcePrefab, _iconsContainer);
        icon.transform.localPosition = WorldToMap(resource.position);
        _resourceIcons.Add(icon);
    }

    public void UnregisterResource(Transform resource)
    {
        for (var i = 0; i < _resourceIcons.Count; i++)
        {
            if (!_resourceIcons[i]) continue;
            
            if ((Vector2)_resourceIcons[i].transform.localPosition == WorldToMap(resource.position))
            {
                Destroy(_resourceIcons[i].gameObject);
                _resourceIcons.RemoveAt(i);
                break;
            }
        }
    }

    private void Update()
    {
        for (var i = 0; i < _redUnits.Count; i++)
        {
            if (_redUnits[i] == null) continue;
            _redIcons[i].transform.localPosition = WorldToMap(_redUnits[i].position);
        }

        for (var i = 0; i < _blueUnits.Count; i++)
        {
            if (_blueUnits[i] == null) continue;
            
            _blueIcons[i].transform.localPosition = WorldToMap(_blueUnits[i].position);
        }
    }

    private Vector2 WorldToMap(Vector3 worldPos)
    {
        var mapWidth = _mapRect.rect.width;
        var mapHeight = _mapRect.rect.height;
        
        var normalizedX = (worldPos.x - _worldOrigin.x) / _worldSize.x;
        var normalizedZ = (worldPos.z - _worldOrigin.y) / _worldSize.y;
        
        var mapX = (normalizedX - 0.5f) * mapWidth;
        var mapY = (normalizedZ - 0.5f) * mapHeight;

        return new Vector2(mapX, mapY);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(_worldOrigin.x + _worldSize.x / 2, 0, _worldOrigin.y + _worldSize.y / 2),
            new Vector3(_worldSize.x, 0, _worldSize.y));
    }
}
