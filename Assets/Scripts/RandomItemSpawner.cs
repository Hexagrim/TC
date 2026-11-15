using System.Collections.Generic;
using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    public Transform[] possiblePoints;

    private static List<Transform> occupiedPoints = new List<Transform>();

    void Start()
    {
        occupiedPoints.RemoveAll(point => point == null);

        List<Transform> availablePoints = new List<Transform>();

        foreach (Transform point in possiblePoints)
        {
            if (point != null && !occupiedPoints.Contains(point))
                availablePoints.Add(point);
        }

        if (availablePoints.Count == 0) return;

        Transform selectedPoint = availablePoints[Random.Range(0, availablePoints.Count)];

        occupiedPoints.Add(selectedPoint);

        transform.SetPositionAndRotation(selectedPoint.position, selectedPoint.rotation);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetOccupiedPoints()
    {
        occupiedPoints.Clear();
    }
}
