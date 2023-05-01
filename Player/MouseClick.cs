using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour
{
    // кулдаун на возможность кликать
    [SerializeField] private float canClickCd;
    private bool canClick = true;

    // номер слоя по которому можно передвигаться (террейн)
    [SerializeField] int _terrainLayer;
    // номер слоя дружественных юнитов и зданий
    [SerializeField] int _friendlyLayer;
    // номер слоя вражеских юнитов и зданий
    [SerializeField] int _enemyLayer;

    // базовый юнит, который выделен при входе в игру
    [SerializeField] Units _unit;
    Units _currentUnit;


    private void Update()
    {
        // если нажат лкм то вызывать метод OnLeftClickAction
        if (Input.GetMouseButtonDown(0) && canClick)
        {
            canClick = false;
            OnLeftClickAction();
        }
        // если нажат пкм то вызывать метод OnRightClickAction
        if (Input.GetMouseButtonDown(1) && canClick)
        {
            canClick = false;
            OnRightClickAction();
        }
    }

    // проверяет было ли пересечение луча от камеры, через координаты мышки с объектом определённого слоя.
    private void OnLeftClickAction()
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
        StartCoroutine(ClickTimeout());
    }

    // проверяет было ли пересечение луча от камеры, через координаты мышки с объектом определённого слоя.
    private void OnRightClickAction()
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
        StartCoroutine(ClickTimeout());
    }

    // кулдаун на нажатия
    private IEnumerator ClickTimeout()
    {
        yield return new WaitForSeconds(canClickCd);
        canClick = true;
    }
}
