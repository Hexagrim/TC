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
        
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!crouching && )
            crouching = !crouching;
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
