using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefaultMovement : IMovement
{
    // параметры обнаружени€ врага
    private float viewRadius = 20;
    private float attackRange = 2;
    private float viewAngle = 720;
    private float stayTime = 4;
    private float speedRun = 4;
    private float speedWalk = 3.5f;
    private LayerMask _enemyLayer;
    private LayerMask _obstacleMask;

    // переменные дл€ следовани€ по вейпоинтам и за врагом
    private float _stayTime = 0;
    private bool _enemyInRange = false;
    private Vector3 _enemyPosition;
    private GameObject _player;
    protected int _currentWaypointIndex = 0;
    public bool _isAttacking = false;

    // navmeshagent дл€ контрол€ ai
    private NavMeshAgent _navMeshAgent;
    private GameObject _unit;

    // waypoints по которым ходит юнит
    private Vector3[] _waypoints;

    // отправл€ет юнита в данную точку
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
        _waypoints = new Vector3[5] { position, 
            new Vector3(position.x + 5, position.y, position.z +5),
            new Vector3(position.x + 5, position.y, position.z + 10),
            new Vector3(position.x + 10, position.y, position.z +10),
            new Vector3(position.x + 10, position.y, position.z +5) };
        Patroling();
    }

    // следование моба по вейпонтам с остановкой
    public bool Patroling()
    {
        EnviromentView();
        _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex]);
        if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            Move(speedWalk);
        }
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        { 
            if (_stayTime <= 0)
            {
                NextPoint();
                Move(speedWalk);
                _stayTime = stayTime;
            }
            else
            {
                Stop();
                _stayTime -= Time.deltaTime;
            }
        }
        if (_enemyInRange)
        {
            Chasing();
        }
        return _isAttacking;
    }

    // преследование игрока при обнаружении
    private void Chasing()
    {
        Move(speedRun);
        _navMeshAgent.SetDestination(_enemyPosition);
        // ≈сли игрок дальше радиуса видимости, то прекращать погоню
        if (Vector3.Distance(_unit.transform.position, _enemyPosition) >= viewRadius)
        {
            _enemyInRange = false;
        }
        // ≈сли игрок ближе радиуса атаки, то атаковать
        if (Vector3.Distance(_unit.transform.position, _enemyPosition) <= attackRange)
        {
            _isAttacking = true;
            _unit.transform.LookAt(_enemyPosition);
        }
        else _isAttacking = false;
    }

    // проверка видит ли моб игрока перед собой
    private void EnviromentView()
    {
        // коллайдер вокруг врага при пересечении которого игроком, playerInRange = true
        Collider[] playerDetect = Physics.OverlapSphere(_unit.transform.position, viewRadius, _enemyLayer.value);
        if (playerDetect.Length != 0)
        {
            Transform player = playerDetect[0].transform;
            Vector3 dirToPlayer = (player.position - _unit.transform.position).normalized;
            if (Vector3.Angle(_unit.transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(_unit.transform.position, player.position);
                //если игрок в радиусе обнаружени€, но на пути есть преп€тствие сло€ obstacleMask, то игрок не обнаружен, иначе обнаружен.
                if (!Physics.Raycast(_unit.transform.position, dirToPlayer, dstToPlayer, _obstacleMask))
                {
                    _enemyInRange = true;
                }
                else
                {
                    _enemyInRange = false;
                }
            }
            // если игрок ушЄл за радиус обнаружени€, то игрок не обнаружен
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

    // выбор следующего вейпоинта
    private void NextPoint()
    {
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex]);
    }

    // остановитс€ при достижении вейпоинта или потере игрока
    private void Stop()
    {
        _navMeshAgent.isStopped = true;
    }

    // возобновление движени€
    private void Move(float speed)
    {
        _navMeshAgent.isStopped = false;
    }
}
