using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    public float amplitude = 0.015f;
    public float frequency = 10.0f;
        
    public Transform camera;
    public Transform cameraHolder;

    private Vector3 startPos;
    public CharacterController controller;

    void Start()
    {
        
    }


    void Update()
    {
        CheckMotion();
    }

    Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude / 2;

        return pos;
    }

    private void ResetPosition()
    {
        if (camera.localPosition == startPos) return;
        camera.localPosition = Vector3.Lerp(camera.localPosition, startPos, Time.deltaTime * 2);
    }

    private void CheckMotion()
    {
        float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
        ResetPosition();
        if (speed < 2) return;
        if (!controller.isGrounded) return;
        PlayMotion(FootStepMotion());
    }

    private void PlayMotion(Vector3 motion)
    {
        camera.localPosition += motion;
    }
}
