using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    
    [SerializeField] private Transform cameraTransform;

    [Header("Cam Movement")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float rotateSpeed = 50f;
    [SerializeField] private float zoom = 12f;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 60f;
    
    

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
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
    
    void HandleZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        scroll *= zoomSpeed;
        
        zoom -= scroll;
        
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cameraTransform.localPosition = new Vector3(0f, zoom, -zoom);
    }
}
