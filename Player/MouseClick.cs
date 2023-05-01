using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour
{
    // ������� �� ����������� �������
    [SerializeField] private float canClickCd;
    private bool canClick = true;

    // ����� ���� �� �������� ����� ������������� (�������)
    [SerializeField] int _terrainLayer;
    // ����� ���� ������������� ������ � ������
    [SerializeField] int _friendlyLayer;
    // ����� ���� ��������� ������ � ������
    [SerializeField] int _enemyLayer;

    // ������� ����, ������� ������� ��� ����� � ����
    [SerializeField] Units _unit;
    Units _currentUnit;


    private void Update()
    {
        // ���� ����� ��� �� �������� ����� OnLeftClickAction
        if (Input.GetMouseButtonDown(0) && canClick)
        {
            canClick = false;
            OnLeftClickAction();
        }
        // ���� ����� ��� �� �������� ����� OnRightClickAction
        if (Input.GetMouseButtonDown(1) && canClick)
        {
            canClick = false;
            OnRightClickAction();
        }
    }

    // ��������� ���� �� ����������� ���� �� ������, ����� ���������� ����� � �������� ������������ ����.
    private void OnLeftClickAction()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        if (Physics.Raycast(castPoint, out RaycastHit hit, Mathf.Infinity) && !EventSystem.current.IsPointerOverGameObject())
        {
            // ���� ����������� ���� � �������� ���� Terrain, �� ���������� �������� ����� ����.
            if (hit.transform.gameObject.layer == _terrainLayer)
            {
                _unit.MoveUnit(new Vector3(hit.point.x, 0, hit.point.z));
            }
            // ���� ����������� ���� � �������� ���� Friendly, �� ����� ���������� ����� ��� ��������.
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
            // ���� ����������� ���� � �������� ���� Enemy, �� �������� ���������� ����� � ���������� ��� ���������� � UI
            else if (hit.transform.gameObject.layer == _enemyLayer)
            {
                _currentUnit = hit.transform.GetComponent<Units>();
                _currentUnit.SelectUnit(false);
            }
        }
        StartCoroutine(ClickTimeout());
    }

    // ��������� ���� �� ����������� ���� �� ������, ����� ���������� ����� � �������� ������������ ����.
    private void OnRightClickAction()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        if (Physics.Raycast(castPoint, out RaycastHit hit, Mathf.Infinity) && !EventSystem.current.IsPointerOverGameObject())
        {
            // ���� ����������� ���� � �������� ���� Terrain, �� ���������� �������� ����� ����.
            if (hit.transform.gameObject.layer == _terrainLayer)
            {
                _unit.StartAI(new Vector3(hit.point.x, 0, hit.point.z));
            }
        }
        StartCoroutine(ClickTimeout());
    }

    // ������� �� �������
    private IEnumerator ClickTimeout()
    {
        yield return new WaitForSeconds(canClickCd);
        canClick = true;
    }
}
