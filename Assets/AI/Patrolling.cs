using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Patrolling : MonoBehaviour
{
    [SerializeField] float speed;

    NavMeshAgent agent;
    public void OnStart(NavMeshAgent agent, Transform target)
    {
        this.agent = agent;
        agent.speed = speed;
        agent.SetDestination(target.position);
        agent.updatePosition = true;
        agent.updateRotation = true;

        Debug.Log("StartCalled");
    }

    public bool IsReached()
    {
        if (agent.remainingDistance < 0.5F)
            return true;
        return false;
    }

    public int UpdatePatrolPointIndex(int patrolPointCount, int currentIndex)
    {
        currentIndex++;
        if(currentIndex >= patrolPointCount)
            currentIndex = 0;

        return currentIndex;
    }
}
