using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public interface IMouse
{
    public List<Units> OnLeftClickAction(int _terrainLayer, int _friendlyLayer, int _enemyLayer, Units _unit, Units _currentUnit);

    public List<Units> OnRightClickAction(int _terrainLayer, int _friendlyLayer, int _enemyLayer, Units _unit, Units _currentUnit);
    public void OnMouseCameraAction(CinemachineVirtualCamera _virtualCamera, Vector2 value, GameObject _freeCameraObject);
    public List<Units> OnAreaSelectionAction(int _friendlyLayer);
}
