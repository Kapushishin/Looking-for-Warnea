using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // номер слоя по которому можно передвигаться (террейн)
    [SerializeField] int _terrainLayer;
    // номер слоя дружественных юнитов и зданий
    [SerializeField] int _friendlyLayer;
    // номер слоя вражеских юнитов и зданий
    [SerializeField] int _enemyLayer;
    // основная камера
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    // основная камера
    [SerializeField] GameObject _freeCameraObject;

    // базовый юнит, который выделен при входе в игру
    [SerializeField] Units _unit;
    Units _heroUnit;
    Units _currentUnit;
    List<Units> _units;

    public IMouse _mouse;
    public IKeyboard _keyboard;


    private void Start()
    {
        _heroUnit = _unit;
        _mouse = new DefaultMouseActions();
        _keyboard = new DefaultKeyboardActions();
    }

    public void OnLeftMouse(InputValue value)
    {
        _units = _mouse.OnLeftClickAction(_terrainLayer, _friendlyLayer, _enemyLayer, _unit, _currentUnit);
        _unit = _units[0];
        _currentUnit = _units[1];
    }

    public void OnRightMouse(InputValue value)
    {
        _units = _mouse.OnRightClickAction(_terrainLayer, _friendlyLayer, _enemyLayer, _unit, _currentUnit);
        _unit = _units[0];
        _currentUnit = _units[1];
    }

    public void OnMouseCameraMove(InputValue value)
    {
        _mouse.OnMouseCameraAction(_virtualCamera, value.Get<Vector2>(), _freeCameraObject);
    }

    public void OnKeyboardCameraMove(InputValue value)
    {
        _keyboard.OnWASD(_virtualCamera, value.Get<Vector2>(), _freeCameraObject);
    }

    public void OnEscape(InputValue value)
    {
        _units = _keyboard.OnEscapePressed(_unit, _currentUnit, _heroUnit);
        _unit = _units[0];
        _currentUnit = _units[1];
    }

    public void OnSpace(InputValue value)
    {
        _keyboard.OnSpacePressed(_unit, _virtualCamera);
    }

    public void OnAreaSelection(InputValue value)
    {
        Debug.Log(value.Get<Vector2>());
    }
}
