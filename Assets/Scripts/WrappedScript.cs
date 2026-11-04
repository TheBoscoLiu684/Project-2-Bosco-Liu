using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class WrappedScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform playerTarget;
    public Transform[] patrolPoints;
    public float waitTime = 4f;
    public float stopDistance = 0.5f;

    public float detectRange = 20f;
    public float viewAngle = 160f;
    public float lostPlayer = 4f;
    public float lostPlayerTime;
    public Vector3 lastSeenPos;
    public float investigateTime = 4f;

    public NavMeshAgent agent;
    public Animator monAnim;
    public int patrolPointIndex;
    public MonsterState state = MonsterState.None;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monAnim = GetComponentInChildren<Animator>();
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
                 monAnim.SetBool("isWalk", agent.velocity.magnitude > 0.1f && state != MonsterState.Chasing);
                 if (playerDistance <= detectRange && CanSee())
                 {
                     state = MonsterState.Chasing;
                 }
                 break;
            }

            case MonsterState.Waiting:
            {
                    monAnim.SetBool("isWait", state == MonsterState.Waiting);
                    break;
            }
            case MonsterState.Chasing:
            {
                 ChasingPlayer();
                 monAnim.SetBool("isChase", state == MonsterState.Chasing);
                 if (CanSee())
                 {
                     lostPlayerTime = 0f;
                     lastSeenPos = playerTarget.position;
                     //Debug.Log("Last seen position: " + lastSeenPos);
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
        agent.speed = 2;
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
        //Debug.Log("Moving to point " + patrolPointIndex);

        patrolPointIndex = (patrolPointIndex + 1) % patrolPoints.Length;
        state = MonsterState.Patrolling;
    }

    public void ReturnToPoint()
    {
        agent.isStopped = false;
        Transform target = patrolPoints[(patrolPointIndex - 1 + patrolPoints.Length) % patrolPoints.Length];
        agent.SetDestination(target.position);
        state = MonsterState.Patrolling;
    }

    public IEnumerator WaitAtPoint()
    {
        state = MonsterState.Waiting;
        agent.isStopped = true;
        //Debug.Log("Waiting at Point");
        yield return new WaitForSeconds(waitTime);
        agent.isStopped = false;
        NextPatrolPoint();
    }

    public IEnumerator Investigate()
    {
        agent.isStopped = false;
        agent.SetDestination(lastSeenPos); //walk towards the position it saw player last
        state = MonsterState.Investigating;
        while (agent.pathPending || agent.remainingDistance > stopDistance) //keeps running until the monster gets to the position then runs the rest of the code
        {
            yield return null;
        }

        agent.isStopped = true;
        Debug.Log("Now Investigating");
        yield return new WaitForSeconds(investigateTime);

        agent.isStopped = false;
        ReturnToPoint();
    }

    public bool CanSee()
    {
        if (playerTarget.GetComponent<PlayerScript>().isHidden)
            return false;
        return FacingPlayer() && ClearPath();
    }

    public bool FacingPlayer()
    {
        Vector3 facingDir = (playerTarget.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, facingDir);
        //Debug.Log("Angle to player: " + angle);
        return angle <= viewAngle / 2f;
    }
    public bool ClearPath()
    {
        Vector3 facingDir = playerTarget.position - transform.position;
        if(Physics.Raycast(transform.position, facingDir.normalized, out RaycastHit hit, facingDir.magnitude))
        {
            //Debug.Log("hit: " + hit.transform.name);
            return hit.transform == playerTarget;
        }
        return true;
    }

    public void ChasingPlayer()
    {
        if (playerTarget.GetComponent<PlayerScript>().isHidden)
        {
            lostPlayerTime = 0f;
            Debug.Log("Stopping Chase");
            Debug.Log("Investigating");
            StartCoroutine(Investigate());
            return;
        }
        agent.isStopped = false;   
        agent.SetDestination(playerTarget.position);
        agent.speed = 6f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
        if (collision.gameObject.CompareTag("Player") && !player.isHidden && player != null)
        {
            SceneManager.LoadScene("Retry");
        }
    }
    public enum MonsterState
    {
        None = 0,
        Patrolling = 1,
        Waiting = 2,
        Chasing = 3,
        Investigating = 4

    }
}
