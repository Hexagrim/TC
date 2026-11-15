using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RandomItemSpawner : MonoBehaviour
{
    public Transform[] possiblePoints;
    public float minDistance = 2f;

    private static List<Transform> occupiedPoints = new List<Transform>();

    void Start()
    {
        List<Transform> availablePoints = new List<Transform>();

        foreach (Transform point in possiblePoints)
        {
            bool valid = true;

            foreach (Transform occupied in occupiedPoints)
            {
                if (Vector3.Distance(point.position, occupied.position) < minDistance)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
                availablePoints.Add(point);
        }

        if (availablePoints.Count == 0) return;

        Transform selectedPoint = availablePoints[UnityEngine.Random.Range(0, availablePoints.Count)];

        occupiedPoints.Add(selectedPoint);
        transform.position = selectedPoint.position;
    }
}
