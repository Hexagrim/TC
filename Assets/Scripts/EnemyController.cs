using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    PlayerMovement player;

    [Header("settings")]
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    public float stopDistance = 0.5f;

    public Animator Anim;
    int currentPatrolIndex;
    bool isWaiting;


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
        Patrol();
        UpdateAnimations();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (hit.collider.GetComponent<IT_Door>())
            {
                hit.collider.GetComponent<IT_Door>().OpenEnemy(transform);
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
}
