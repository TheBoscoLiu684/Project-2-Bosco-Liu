using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WrappedScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform playerTarget;
    public Transform[] patrolPoints;
    public float waitTime = 2f;
    public float stopDistance = 0.5f;

    public float detectRange = 10f;
    public float viewAngle = 90f;
    public float lostPlayer = 4f;
    public float lostPlayerTime;

    public NavMeshAgent agent;
    public Animator monAnim;
    public int patrolPointIndex;
    public MonsterState state = MonsterState.None;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monAnim = GetComponent<Animator>();
        if(patrolPoints.Length == 0)
        {
            Debug.Log("No points assigned");
            return;
        }
        NextPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector3.Distance(playerTarget.position, transform.position);
        bool canSeePlayer = playerDistance <= detectRange && CanSee();
        switch (state)
        {
            case MonsterState.Patrolling:
            {
                 Patrol();
                 if (playerDistance <= detectRange && CanSee())
                 {
                     state = MonsterState.Chasing;
                 }
                 break;
            }
                

            case MonsterState.Chasing:
            {
                 ChasingPlayer();
                 if (CanSee())
                 {
                     lostPlayerTime = 0f;
                 }
                 else
                 {
                     lostPlayerTime += Time.deltaTime;
                     if (lostPlayerTime >= lostPlayer)
                     {
                         state = MonsterState.Patrolling;
                     }
                 }
                 break;
            }
                

        }
        Patrol();
    }

    public void Patrol()
    {
        if (state == MonsterState.Waiting)
            return;
        
        if (agent.pathPending)
            return;

        if (!agent.hasPath && agent.remainingDistance > stopDistance)
        {
            Debug.LogWarning("Monster has no path to its destination!");
            return;
        }
        if (!agent.pathPending && agent.remainingDistance <= stopDistance)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    public void NextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;
        Transform target = patrolPoints[patrolPointIndex];
        agent.isStopped = false;
        bool accepted = agent.SetDestination(target.position);
        Debug.Log("Destination accepted: " + accepted);
        Debug.Log("Moving to point " + patrolPointIndex);

        patrolPointIndex = (patrolPointIndex + 1) % patrolPoints.Length;
        state = MonsterState.Patrolling;
    }

    public IEnumerator WaitAtPoint()
    {
        state = MonsterState.Waiting;
        agent.isStopped = true;
        Debug.Log("Waiting at Point");
        yield return new WaitForSeconds(waitTime);
        agent.isStopped = false;
        NextPatrolPoint();
    }

    public bool CanSee()
    {
        return FacingPlayer() && ClearPath();
    }

    public bool FacingPlayer()
    {
        Vector3 facingDir = (playerTarget.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, facingDir);
        Debug.Log("Angle to player: " + angle);
        return angle <= viewAngle / 2f;
    }
    public bool ClearPath()
    {
        Vector3 facingDir = playerTarget.position - transform.position;
        if(Physics.Raycast(transform.position, facingDir.normalized, out RaycastHit hit, facingDir.magnitude))
        {
            Debug.Log("hit: " + hit.transform.name);
            return hit.transform == playerTarget;
        }
        return true;
    }

    public void ChasingPlayer()
    {
        agent.SetDestination(playerTarget.position);
    }
    public enum MonsterState
    {
        None = 0,
        Patrolling = 1,
        Waiting = 2,
        Chasing = 3
    }
}
