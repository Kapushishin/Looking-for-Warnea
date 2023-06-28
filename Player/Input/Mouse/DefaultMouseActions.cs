using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultMouseActions : IMouse
{
    // проверяет было ли пересечение луча от камеры, через координаты мышки с объектом определённого слоя.
    public List<Units> OnLeftClickAction(int _terrainLayer, int _friendlyLayer, int _enemyLayer, Units _unit, Units _currentUnit)
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        if (Physics.Raycast(castPoint, out RaycastHit hit, Mathf.Infinity) && !EventSystem.current.IsPointerOverGameObject())
        {
            // если пересечение было с объектом слоя Terrain, то отправлять текущего юнита туда.
            if (hit.transform.gameObject.layer == _terrainLayer)
            {
                _unit.MoveUnit(new Vector3(hit.point.x, 0, hit.point.z));
            }
            // если пересечение было с объектом слоя Friendly, то брать дружеского юнита под контроль.
            else if (hit.transform.gameObject.layer == _friendlyLayer)
            {
                if (_currentUnit != _unit)
                {
                    _currentUnit = _unit;
                    _currentUnit.DeselectUnit();
                    _unit = hit.transform.GetComponent<Units>();
                    _unit.SelectUnit(true);
                }
                else
                {
                    _currentUnit = null;
                }
            }
            // если пересечение было с объектом слоя Enemy, то выделять вражеского юнита и показывать его информацию в UI
            else if (hit.transform.gameObject.layer == _enemyLayer)
            {
                _currentUnit = hit.transform.GetComponent<Units>();
                _currentUnit.SelectUnit(false);
            }
        }
        return new List<Units>() { _unit, _currentUnit };
    }


    // проверяет было ли пересечение луча от камеры, через координаты мышки с объектом определённого слоя.
    public List<Units> OnRightClickAction(int _terrainLayer, int _friendlyLayer, int _enemyLayer, Units _unit, Units _currentUnit)
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        if (Physics.Raycast(castPoint, out RaycastHit hit, Mathf.Infinity) && !EventSystem.current.IsPointerOverGameObject())
        {
            // если пересечение было с объектом слоя Terrain, то отправлять текущего юнита туда.
            if (hit.transform.gameObject.layer == _terrainLayer)
            {
                _unit.StartAI(new Vector3(hit.point.x, 0, hit.point.z));
            }
        }
        return new List<Units>() { _unit, _currentUnit };
    }

    public void OnMouseCameraAction(CinemachineVirtualCamera _virtualCamera, Vector2 value, GameObject _freeCameraObject)
    {
        _virtualCamera.Follow = _freeCameraObject.transform;
        _virtualCamera.LookAt = _freeCameraObject.transform;
        if (value.x >= Screen.width - 1)
        {
            _freeCameraObject.transform.position = new Vector3(_freeCameraObject.transform.position.x + 0.3f,
            _freeCameraObject.transform.position.y,
            _freeCameraObject.transform.position.z);
        }
        if (value.x <= Screen.width - (Screen.width - 1))
        {
            _freeCameraObject.transform.position = new Vector3(_freeCameraObject.transform.position.x - 0.3f,
            _freeCameraObject.transform.position.y,
            _freeCameraObject.transform.position.z);
        }
        if (value.y >= Screen.height - 1)
        {
            _freeCameraObject.transform.position = new Vector3(_freeCameraObject.transform.position.x,
            _freeCameraObject.transform.position.y,
            _freeCameraObject.transform.position.z + 0.3f);
        }
        if (value.y <= Screen.height - (Screen.height - 1))
        {
            _freeCameraObject.transform.position = new Vector3(_freeCameraObject.transform.position.x,
            _freeCameraObject.transform.position.y,
            _freeCameraObject.transform.position.z - 0.3f);
        }
    }

    public List<Units> OnAreaSelectionAction(int _friendlyLayer)
    {
        return new List<Units>();
    }
}
