using NUnit.Framework.Internal.Execution;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public float range = 4f;
    Interactable lookObject;

    public GameObject lookingCrosshair;

    void Start()
    {
        
    }

    void Update()
    {
        //same logic as the item thingy:

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            if (hit.collider.GetComponent<Interactable>())
            {
                lookObject = hit.collider.GetComponent<Interactable>();
            }
            else
            {
                lookObject = null;
            }

        }
        else
        {
            lookObject = null;
        }

        lookingCrosshair.SetActive(lookObject != null);

        if (Input.GetKeyDown(KeyCode.F) && lookObject!=null)
        {

            lookObject.Use();
            // if i ever wanna make a hold to use thingy:
            //lookObject.StartUsing();
        }
        //
        //hold to use logic too:
        //if(Input.GetKeyUp(Keycode.F)){
        //lookObject.StopUsing();
        //}
        //

    }

}
