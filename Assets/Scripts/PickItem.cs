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
    public Transform keyHand;
    public Transform plKeyHand;
    public Transform sdHand;
    public Transform wcHand;
    public Transform vwHand;
    public Transform bookHand;
    public Transform cbHand;
    //HANDS
    void Start()
    {
        
    }

    void Update()
    {
        RaycastHit hit;
        //the most beautiful calculation i have ever done that worked first try Camera.main.transform.position + Camera.main.transform.forward * 0.5f
        if (Physics.Raycast(Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Camera.main.transform.forward, out hit, range))
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

        lookingCrosshair.SetActive(lookItem != null && lookItem!= heldItem);
        //Debug.DrawRay(Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Camera.main.transform.forward * range, (lookItem == null)? Color.red : Color.green);


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
        heldItem.transform.position = Camera.main.transform.position + (Vector3.down * 0.5f);
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
        else if(heldItem.itemName == "key")
        {
            heldItem.transform.SetParent(keyHand);
        }
        else if(heldItem.itemName == "plkey")
        {
            heldItem.transform.SetParent(plKeyHand);
        }
        else if(heldItem.itemName == "screwdriver")
        {
            heldItem.transform.SetParent(sdHand);
        }
        else if (heldItem.itemName == "wirecutter")
        {
            heldItem.transform.SetParent(wcHand);
        }
        else if (heldItem.itemName == "valvewheel")
        {
            heldItem.transform.SetParent(vwHand);
        }
        else if (heldItem.itemName == "book")
        {
            heldItem.transform.SetParent(bookHand);
        }
        else if (heldItem.itemName == "crowbar")
        {
            heldItem.transform.SetParent(cbHand);
        }
    }
}
