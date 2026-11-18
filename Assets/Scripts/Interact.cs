
using UnityEngine;

public class Interact : MonoBehaviour
{
    public float range = 4f;
    Interactable lookObject;

    public GameObject lookingCrosshair;
    public LayerMask ignoreLayer;

    public bool paused;
    void Start()
    {
        
    }

    void Update()
    {
        //same logic as the item thingy:

        if (paused || FindAnyObjectByType<UI_Manager>().gameEnded) return;
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range , ~ignoreLayer ))
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

        lookingCrosshair.SetActive(lookObject != null && lookObject.canUse);

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
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic) return;

        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        rb.AddForce(pushDirection * 0.5f, ForceMode.Impulse);
    }

}
