using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
//not my code, just fixed an already online code
public class SmoothAgentMovement : MonoBehaviour
{
    [Header("Smoothing Settings")]
    [Tooltip("Higher values create wider, smoother arcs.")]
    [Range(0.01f, 0.5f)]
    public float smoothness = 0.15f;

    [Tooltip("Speed along the smooth path.")]
    public float movementSpeed = 3.5f;

    [Tooltip("How fast the agent will rotate to face the movement direction.")]
    public float rotationSpeed = 10f;

    private NavMeshAgent agent;
    private List<Vector3> smoothPathPoints = new List<Vector3>();
    private int currentPointIndex = 0;
    private bool isFollowingPath = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
    }
    public void SetDestination(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path))
        {
            if (path.corners.Length > 0)
            {
                smoothPathPoints = GenerateCatmullRomSpline(path.corners, smoothness);
                currentPointIndex = 0;
                isFollowingPath = true;

                agent.Warp(transform.position);
            }
        }
    }

    void Update()
    {
        if (!isFollowingPath || smoothPathPoints.Count == 0) return;

        Vector3 targetPoint = smoothPathPoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, movementSpeed * Time.deltaTime);
        agent.nextPosition = transform.position;

        Vector3 moveDirection = (targetPoint - transform.position);
        moveDirection.y = 0;
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            currentPointIndex++;
            if (currentPointIndex >= smoothPathPoints.Count)
            {
                isFollowingPath = false;
            }
        }
    }

 
    List<Vector3> GenerateCatmullRomSpline(Vector3[] corners, float granularity)
    {
        List<Vector3> results = new List<Vector3>();

        if (corners.Length < 2) return results;

        List<Vector3> extendedCorners = new List<Vector3>();
        extendedCorners.Add(corners[0] - (corners[1] - corners[0]));
        extendedCorners.AddRange(corners);
        extendedCorners.Add(corners[corners.Length - 1] + (corners[corners.Length - 1] - corners[corners.Length - 2])); // Fake next point

        for (int i = 1; i < extendedCorners.Count - 2; i++)
        {
            Vector3 p0 = extendedCorners[i - 1];
            Vector3 p1 = extendedCorners[i];
            Vector3 p2 = extendedCorners[i + 1];
            Vector3 p3 = extendedCorners[i + 2];

            for (float t = 0; t < 1; t += granularity)
            {
                Vector3 point = GetCatmullRomPosition(t, p0, p1, p2, p3);
                results.Add(point);
            }
        }

        results.Add(corners[corners.Length - 1]);
        return results;
    }

    Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // catmull-rom spline thingy that i found online, didnt fucking come up with this at 1 AM
        float t2 = t * t;
        float t3 = t2 * t;

        float x = 0.5f * ((2.0f * p1.x) + (-p0.x + p2.x) * t + (2.0f * p0.x - 5.0f * p1.x + 4.0f * p2.x - p3.x) * t2 + (-p0.x + 3.0f * p1.x - 3.0f * p2.x + p3.x) * t3);
        float y = 0.5f * ((2.0f * p1.y) + (-p0.y + p2.y) * t + (2.0f * p0.y - 5.0f * p1.y + 4.0f * p2.y - p3.y) * t2 + (-p0.y + 3.0f * p1.y - 3.0f * p2.y + p3.y) * t3);
        float z = 0.5f * ((2.0f * p1.z) + (-p0.z + p2.z) * t + (2.0f * p0.z - 5.0f * p1.y + 4.0f * p2.z - p3.z) * t2); 
        
        Vector3 a = 2f * p1;

        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        return 0.5f * (a + (b * t) + (c * t2) + (d * t3));

    }
}