using UnityEngine;

public class PickItem : MonoBehaviour
{
    public float range;

    public Transform itemHand;

    public Item lookItem;
    public GameObject lookingCrosshair;

    public Item heldItem;


    //HANDS
    public Transform torchHand;
    //HANDS
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

        //picking it up

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(heldItem!=null)
            {
                Drop();
            }

            else if (lookItem != null && heldItem == null)
            {
                PickUp(lookItem);
            }
        }
        
        //use item
        if (Input.GetKeyDown(KeyCode.Mouse0) && heldItem != null)
        {
            heldItem.Use();
        }
    }

    void PickUp(Item i)
    {
        heldItem = i;
        heldItem.Pick();
        SetParent();
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;

    }

    void Drop()
    {
        heldItem.Drop();
        heldItem.transform.parent = null;
        heldItem.transform.position = transform.position;
        heldItem.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 200);

        heldItem = null;
    }

    void SetParent()
    {
        if (heldItem.itemName == "item")
        {
            heldItem.transform.SetParent(itemHand);
        }
        else if(heldItem.itemName == "torch")
        {
            heldItem.transform.SetParent(torchHand);
        }
    }
}
