using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float rotateSpeed = 50f;

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 input = Vector3.zero;

        if (Keyboard.current.wKey.isPressed) input.z += 1;
        if (Keyboard.current.aKey.isPressed) input.x -= 1;
        if (Keyboard.current.sKey.isPressed) input.z -= 1;
        if (Keyboard.current.dKey.isPressed) input.x += 1;
        
        input = input.normalized;
        
        Vector3 direction = transform.forward * input.z + transform.right * input.x;
        
        transform.position += direction * (moveSpeed * Time.deltaTime);
    }

    void HandleRotation()
    {
        if (Keyboard.current.qKey.isPressed)
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
        }

        if (Keyboard.current.eKey.isPressed)
        {
            transform.Rotate(0f, -rotateSpeed * Time.deltaTime, 0f);
        }
    }
}
