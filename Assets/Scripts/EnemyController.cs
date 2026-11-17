using JetBrains.Annotations;
using System;
using System.Collections;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
public enum EnemyState
{
    Patrolling,
    Following,
    Attacking
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
    public Transform enemyHead;
    public float attackRange = 1.2f;


    int currentPatrolIndex;
    bool isWaiting;
    float timeSincePlayerLost;
    public bool attacking;

    EnemyState currentState = EnemyState.Patrolling;

    Coroutine closeDoorAfterPassing;

    AudioManager AM;
    void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
        AM = FindAnyObjectByType<AudioManager>();
    }

    void Start()
    {
        GoToNextPatrolPoint();
    }

    void Update()
    {

        if (FindAnyObjectByType<UI_Manager>().gameEnded)
        {
            agent.enabled = false;
            return;
        }
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        switch (currentState)
        {
            case EnemyState.Patrolling:
                AM.stopChase();
                agent.speed = patrolSpeed;
                Patrol();
                if (distanceToPlayer <= detectionRange && CanSeePlayer())
                {
                    currentState = EnemyState.Following;
                }
                break;

            case EnemyState.Following:

                FollowPlayer();
                AM.playChase();
                agent.speed = chaseSpeed;

                if(distanceToPlayer <= attackRange)
                {
                    currentState = EnemyState.Attacking;
                }

                if (!CanSeePlayer())
                {
                    timeSincePlayerLost += Time.deltaTime;
                    if (timeSincePlayerLost >= losePlayerTime)
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
            case EnemyState.Attacking:
                
                if (!attacking) StartAttack();

                //Invoke(EndDay());
                break;

        }

        UpdateAnimations();
        LookAtPlayerWhenChasing();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
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
    
    void StartAttack()
    {
        agent.isStopped = true;
        attacking = true;
        //probably not the way to go, but it works for now:
        PickItem pi = player.GetComponent<PickItem>();
        if (pi.heldItem != null)
        {
            pi.heldItem.Drop();
            pi.heldItem.transform.parent = null;
            pi.heldItem = null;
        }
        pi.enabled = false;
        player.GetComponent<Interact>().enabled = false;
        player.enabled = false;
        Anim.SetBool("IsAttacking", true);
        AM.PlayJumpscare();
        StartCoroutine(SendPlayerToMenu());
        
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        int nextIndex;
        // first time using do/while. turns out unity doesnt have a fking exclude from random range function
        //linq for the clutchh, takes closest n number of points
        Transform[] closestPoints = patrolPoints.OrderBy(i => Vector3.Distance(patrolPoints[currentPatrolIndex].position, i.position)).Take(6).ToArray();
        do nextIndex = UnityEngine.Random.Range(0, closestPoints.Length);
        while (nextIndex == currentPatrolIndex);
        currentPatrolIndex = Array.IndexOf(patrolPoints, closestPoints[nextIndex]);

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
    IEnumerator SendPlayerToMenu()
    {

        yield return new WaitForSeconds(2f);
        FindAnyObjectByType<UI_Manager>().t_anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1f);
        //send player to menu
        SceneManager.LoadSceneAsync("MainMenu");

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
        //NOW IMA ANIMATEEEEEE

        bool isMoving = agent.velocity.sqrMagnitude > 0.1f;
        Anim.SetBool("IsWalking", isMoving);

        Anim.SetBool("IsChasing", isMoving && currentState == EnemyState.Following);

    }

    IEnumerator CloseDoor(IT_Door usedDoor)
    {
        yield return new WaitForSeconds(2f);
        usedDoor.CloseEnemy();
        closeDoorAfterPassing = null;

    }

    bool CanSeePlayer()
    {
        //Debug.Log(HasClearPathToPlayer().ToString() + IsFacingPlayer().ToString());

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
        if (Physics.Raycast(transform.position, dirToPlayer.normalized, out RaycastHit hit, dirToPlayer.magnitude))
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

        for (var i = 0; i < patrolPoints.Length; i++)
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

    void LookAtPlayerWhenChasing()
    {
        if (enemyHead == null) return;
        if (currentState != EnemyState.Following) return;
        Vector3 direction = Camera.main.transform.position - enemyHead.position;
        Quaternion targetWorldRotation = Quaternion.LookRotation(direction);
        Vector3 targetLocalRotation = (Quaternion.Inverse(transform.rotation) * targetWorldRotation).eulerAngles;
        float yRotation = Mathf.Clamp(Mathf.DeltaAngle(0f, targetLocalRotation.y), -80f, 80f);

        Quaternion targetRotation = Quaternion.Euler(0f, yRotation, 0f);

        enemyHead.localRotation = Quaternion.Lerp(
            enemyHead.localRotation,
            targetRotation,
            Time.deltaTime * 5f
        );
    }

}

