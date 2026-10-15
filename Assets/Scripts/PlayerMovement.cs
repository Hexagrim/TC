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

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        
    }

    
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        MouseLook();
        MoveLogic();
    }

    void MoveLogic()
    {
        Vector3 moveDir = transform.forward * vertical + transform.right * horizontal;
        Vector3 move = moveDir.normalized * speed;
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

}
