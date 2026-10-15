using UnityEngine;

public class PickItem : MonoBehaviour
{
    public float range;

    public Transform itemHand;

    public Item lookItem;

    public GameObject lookingCrosshair;
    void Start()
    {
        
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            if (hit.collider.GetComponent<Item>())
            {
                lookItem = hit.collider.GetComponent<Item>();
            }
            else
            {
                lookItem = null;
            }

        }
        else
        {
            lookItem = null;
        }

        //handle the crosshair change:

        lookingCrosshair.SetActive(lookItem != null);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10f, Color.red);
    }
}
