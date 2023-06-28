using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public interface IKeyboard
{
    public List<Units> OnEscapePressed(Units _unit, Units _currentUnit, Units _heroUnit);
    public void OnSpacePressed(Units _unit, CinemachineVirtualCamera _virtualCamera);
    public void OnWASD(CinemachineVirtualCamera _virtualCamera, Vector2 value, GameObject _freeCameraObject);
}
