using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float sens;
    private CharacterController controller;
    float horizontal;
    float vertical;

    public float gravity;
    float verticalVelocity;

    float verticalRotation;
    public GameObject camera;
    public bool crouching;

    float realSpeed;
    public Transform upRayOrigin;

    bool canStand;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        realSpeed = speed;
    }
    
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        MouseLook();
        MoveLogic();
        CrouchLogic();
    }

    void MoveLogic()
    {
        Vector3 moveDir = transform.forward * vertical + transform.right * horizontal;
        Vector3 move = moveDir.normalized * realSpeed;
        move.y = verticalForce();
        controller.Move(move * Time.deltaTime);
    }

    float verticalForce()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        return verticalVelocity;
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * sens;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 80);

        transform.Rotate(mouseX * sens * Vector3.up);
        camera.transform.localRotation = Quaternion.Euler(verticalRotation, camera.transform.localRotation.y, camera.transform.localRotation.z);

    }

    void CrouchLogic()
    {

        Vector3 point1 = upRayOrigin.position;
        Vector3 point2 = upRayOrigin.position + Vector3.up;
        float radius = 0.1f;

        Collider[] hits = Physics.OverlapCapsule(point1, point2, radius);

        canStand = true;

        foreach (Collider col in hits)
        {
            if (!col.isTrigger && col.transform.root != transform.root)
            {
                canStand = false;
                break;
            }
        }

        Debug.DrawLine(point1, point2, canStand ? Color.green : Color.red);
       //gotta love the ternary operators :>

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canStand || !crouching)
            {
                crouching = !crouching;
            }
        }

        if (crouching)
        {
            if (controller.height >= 1f)
            {
                controller.height = Mathf.Lerp(controller.height, 1f, Time.deltaTime * 20f);
            }
            else
            {
                controller.height = 1f;
            }
            realSpeed = 0.5f * speed;
        }
        else
        {
            if (controller.height <= 2f)
            {
                controller.height = Mathf.Lerp(controller.height, 2f, Time.deltaTime * 20f);
            }
            else
            {
                controller.height = 2f;
            }
            realSpeed = speed;
        }
    }
}
