using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    public Transform cameraHolder;

    public bool paused;
    public Slider sensSlider;

    AudioManager AM;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AM = FindAnyObjectByType<AudioManager>();
        realSpeed = speed;

        //for SFXX

        StartCoroutine(PlayerFootSteps());
    }
    
    void Update()
    {
        if(paused || FindAnyObjectByType<UI_Manager>().gameEnded) return;

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
            if (controller.height >= 1.5f)
            {
                controller.height = Mathf.Lerp(controller.height, 1.5f, Time.deltaTime * 20f);
            }
            else
            {
                controller.height = 1.5f;
            }
            realSpeed = 0.5f * speed;
        }
        else
        {
            if (controller.height <= 2.5f)
            {
                controller.height = Mathf.Lerp(controller.height, 2.5f, Time.deltaTime * 20f);
            }
            else
            {
                controller.height = 2.5f;
            }
            realSpeed = speed;
        }

        if (crouching)
        {
            cameraHolder.transform.localPosition = new Vector3(0, 0.75f, 0);
        }
        else
        {
            cameraHolder.transform.localPosition = new Vector3(0,1.5f,0);
        }
    }

    public void AdjustSens()
    {
        sens = sensSlider.value + 0.1f;
    }
    
    IEnumerator PlayerFootSteps()
    {
        while (true)
        {
            if (crouching)
            {
                if (controller.velocity.magnitude > 0.05f && controller.isGrounded)
                {
                    AM.playStep(transform, 0.1f);
                }
                yield return new WaitForSeconds(0.45f);
            }
            else
            {
                if (controller.velocity.magnitude > 0.05f && controller.isGrounded)
                {
                    AM.playStep(transform, 0.2f);
                }
                yield return new WaitForSeconds(0.35f);
            }
        }
       
    }


}
