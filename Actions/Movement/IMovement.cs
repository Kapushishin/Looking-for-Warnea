using UnityEngine;
using UnityEngine.AI;

public interface IMovement
{
    public void SetDestination(NavMeshAgent navMeshAgent, Vector3 position);
    public void StartAI(NavMeshAgent navMeshAgent, LayerMask enemyLayer, GameObject unit, Vector3 position);
    public bool Patroling();
}
