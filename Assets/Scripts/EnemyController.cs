using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrolling,
    Following
}

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    PlayerMovement player;
    public Animator Anim;

    [Header("settings")]

    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    public float stopDistance = 0.5f;
    public float detectionRange = 5f;
    public float viewAngle = 90f;
    public float losePlayerTime = 4f;
    public float chaseSpeed;
    public float patrolSpeed;
    
    int currentPatrolIndex;
    bool isWaiting;
    float timeSincePlayerLost;
    EnemyState currentState = EnemyState.Patrolling;

    Coroutine closeDoorAfterPassing;

    void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        GoToNextPatrolPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        switch (currentState)
        {
            case EnemyState.Patrolling:
                agent.speed = patrolSpeed;
                Patrol();
                if (distanceToPlayer <= detectionRange && CanSeePlayer())
                {
                    currentState = EnemyState.Following;
                }
                break;
            case EnemyState.Following:
                FollowPlayer();
                agent.speed = chaseSpeed;
                if(!CanSeePlayer())
                {
                    timeSincePlayerLost += Time.deltaTime;
                    if(timeSincePlayerLost >= losePlayerTime)
                    {
                        currentState = EnemyState.Patrolling;
                        StartCoroutine(WaitAtLastPlayerPoint());
                    }
                }
                else
                {
                    timeSincePlayerLost = 0f;
                }
                break;
        }

        UpdateAnimations();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (hit.collider.GetComponent<IT_Door>())
            {
                hit.collider.GetComponent<IT_Door>().OpenEnemy(transform);
                
                if (closeDoorAfterPassing == null)
                {
                    closeDoorAfterPassing = StartCoroutine(CloseDoor(hit.collider.GetComponent<IT_Door>()));
                }
            }
        }
    }
    
    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        int nextIndex;
        // first time using do/while. turns out unity doesnt have a fking exclude from random range function

        
        do nextIndex = Random.Range(0, patrolPoints.Length);
        while (nextIndex == currentPatrolIndex);
        currentPatrolIndex = nextIndex;
       
        //currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(patrolWaitTime);
        agent.isStopped = false;
        GoToNextPatrolPoint();
        isWaiting = false;
    }

    IEnumerator WaitAtLastPlayerPoint()
    {
        isWaiting = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(patrolWaitTime);
        agent.isStopped = false;
        GoToClosestPatrolPoint();
        isWaiting = false;
    }

    void Patrol()
    {
        if (isWaiting) return;
        if (!agent.pathPending && agent.remainingDistance <= stopDistance)
        {
            StartCoroutine(WaitAtPatrolPoint());
        }

    }
    void UpdateAnimations()
    {
        //empty for now since no model/animations
    }

    IEnumerator CloseDoor(IT_Door usedDoor)
    {
        yield return new WaitForSeconds(1.65f);
        usedDoor.CloseEnemy();
        closeDoorAfterPassing = null;

    }

    bool CanSeePlayer()
    {
        return IsFacingPlayer() && HasClearPathToPlayer();
    }

    bool IsFacingPlayer()
    {
        var dirToPlayer = (player.transform.position - transform.position).normalized;
        var angle = Vector3.Angle(transform.forward, dirToPlayer);
        return angle <= viewAngle / 2;
    }

    bool HasClearPathToPlayer()
    {
        var dirToPlayer = player.transform.position - transform.position;
        if(Physics.Raycast(transform.position,dirToPlayer.normalized,out RaycastHit hit, dirToPlayer.magnitude))
        {
            return hit.transform == player.transform;
        }
        return true;
    }

    void FollowPlayer()
    {
        agent.destination = player.transform.position;
    }

    void GoToClosestPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        var closestIndex = 0;
        var closestDistance = float.MaxValue;

        for(var i = 0;  i < patrolPoints.Length; i++)
        {
            //normal code: var distance = Vector3.Distance(transform.position, patrolPoints[i].position);
            //this is for prioritising the distance on x and z more than y, so it doesnt go to upper floor rooms as closest:
            
            var difference = transform.position - patrolPoints[i].position;
            var distance = new Vector2(difference.x, difference.z).magnitude + Mathf.Abs(difference.y) * 5f;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        currentPatrolIndex = closestIndex;
        agent.SetDestination(patrolPoints[closestIndex].position);
        Debug.Log(closestIndex);
        Debug.Log(currentPatrolIndex);

    }

}
