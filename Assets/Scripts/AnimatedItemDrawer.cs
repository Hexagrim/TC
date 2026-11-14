
using System.Collections.Generic;
using UnityEngine;

public class AnimatedItemDrawer : MonoBehaviour
{
    HashSet<Rigidbody> items = new();

    Vector3 previousPosition;
    Quaternion previousRotation;

    private void Start()
    {
        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponentInParent<Item>();
        if (!item) return;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb) items.Add(rb);
    }

    private void OnTriggerExit(Collider other)
    {
        Item item = other.GetComponentInParent<Item>();
        if (!item) return;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb) items.Remove(rb);
    }

    private void FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        Vector3 movement = currentPosition - previousPosition;
        Quaternion rotation = currentRotation * Quaternion.Inverse(previousRotation);

        foreach (Rigidbody rb in items)
        {
            if (!rb) continue;

            Vector3 offset = rb.position - previousPosition;

            rb.MovePosition(currentPosition + rotation * offset);
            rb.MoveRotation(rotation * rb.rotation);
        }

        previousPosition = currentPosition;
        previousRotation = currentRotation;
    }
}
