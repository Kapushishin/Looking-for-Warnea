using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefaultMovement : IMovement
{
    // ��������� ����������� �����
    private float viewRadius = 20;
    private float attackRange = 2;
    private float viewAngle = 720;
    private float stayTime = 4;
    private LayerMask _enemyLayer;
    private LayerMask _obstacleMask;

    // ���������� ��� ���������� �� ���������� � �� ������
    private float _stayTime = 0;
    private bool _enemyInRange = false;
    private Vector3 _enemyPosition;
    protected int _currentWaypointIndex = 0;
    public bool _isAttacking = false;

    // navmeshagent ��� �������� ai
    private NavMeshAgent _navMeshAgent;
    private GameObject _unit;

    // waypoints �� ������� ����� ����
    private Vector3[] _waypoints;

    // ���������� ����� � ������ �����
    public void SetDestination(NavMeshAgent navMeshAgent, Vector3 position)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(position);
    }

    public void StartAI(NavMeshAgent navMeshAgent, LayerMask enemyLayer, GameObject unit, Vector3 position)
    {
        _navMeshAgent = navMeshAgent;
        _enemyLayer = enemyLayer;
        _unit = unit;
        _obstacleMask = LayerMask.NameToLayer("Default");
        _waypoints = new Vector3[5] { new Vector3(position.x + 10, position.y, position.z), position, 
            new Vector3(position.x - 10, position.y, position.z),
            new Vector3(position.x - 5, position.y, position.z + 5),
            new Vector3(position.x + 5, position.y, position.z +5),
             };
        Patroling();
    }

    // ���������� ����� �� ���������� � ����������
    public bool Patroling()
    {
        EnviromentView();
        _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex]);
        if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            Move();
        }
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        { 
            if (_stayTime <= 0)
            {
                NextPoint();
                Move();
                _stayTime = stayTime;
            }
            else
            {
                Stop();
                _stayTime -= Time.deltaTime * 10;
            }
        }
        if (_enemyInRange)
        {
            Chasing();
        }
        return _isAttacking;
    }

    // ������������� ���������� ��� �����������
    private void Chasing()
    {
        Move();
        _navMeshAgent.SetDestination(_enemyPosition);
        // ���� ��������� ������ ������� ���������, �� ���������� ������
        if (Vector3.Distance(_unit.transform.position, _enemyPosition) >= viewRadius)
        {
            _enemyInRange = false;
        }
        // ���� ��������� ����� ������� �����, �� ���������
        if (Vector3.Distance(_unit.transform.position, _enemyPosition) <= attackRange)
        {
            _isAttacking = true;
            _unit.transform.LookAt(_enemyPosition);
        }
        else _isAttacking = false;
    }

    // �������� ����� �� ���� ����������
    private void EnviromentView()
    {
        // ��������� ������ ����� ��� ����������� �������� �����������, _enemyInRange = true
        Collider[] playerDetect = Physics.OverlapSphere(_unit.transform.position, viewRadius, _enemyLayer.value);
        if (playerDetect.Length != 0)
        {
            Transform player = playerDetect[0].transform;
            Vector3 dirToPlayer = (player.position - _unit.transform.position).normalized;
            if (Vector3.Angle(_unit.transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(_unit.transform.position, player.position);
                //���� ��������� � ������� �����������, �� �� ���� ���� ����������� ���� obstacleMask, �� ��������� �� ���������, ����� ���������.
                if (!Physics.Raycast(_unit.transform.position, dirToPlayer, dstToPlayer, _obstacleMask))
                {
                    _enemyInRange = true;
                }
                else
                {
                    _enemyInRange = false;
                }
            }
            // ���� ��������� ���� �� ������ �����������, �� ��������� �� ���������
            if (Vector3.Distance(_unit.transform.position, player.position) > viewRadius)
            {
                _enemyInRange = false;
            }
            if (_enemyInRange)
            {
                _enemyPosition = player.transform.position;
            }
        }
    }

    // ����� ���������� ���������
    private void NextPoint()
    {
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex]);
    }

    // ����������� ��� ���������� ��������� ��� ������ ����������
    private void Stop()
    {
        _navMeshAgent.isStopped = true;
    }

    // ������������� ��������
    private void Move()
    {
        _navMeshAgent.isStopped = false;
    }
}
