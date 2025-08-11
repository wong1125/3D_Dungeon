using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameter")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayerMask;
    private Vector2 inputMovement;

    [Header("Camera Parameter")]
    [SerializeField] Transform mainCamera;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float mouseAltitudeLimit;
    private Vector2 inputDelta;
    private float currentCameraRotationX;
    private bool isFirstPerson;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();

        //y축 회전 막기
        var angularVelocity = rb.angularVelocity;
        angularVelocity.y = 0f;
        rb.angularVelocity = angularVelocity;
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    //인풋을 실제 움직임으로 변환
    void Move()
    {
        //구 전용 앞 방향 설정: 이게 없으면 공이 굴러가면서 Vector.Foward 기준도 같이 변한다.
        Vector3 foward = mainCamera.forward;
        Vector3 right = mainCamera.right;
        foward.y = 0f;
        right.y = 0f;
        foward.Normalize();
        right.Normalize();

        Vector3 actaulMovementVector = foward * inputMovement.y + right * inputMovement.x;
        actaulMovementVector *= moveSpeed;

        actaulMovementVector.y = rb.velocity.y;
        rb.velocity = actaulMovementVector;
    }

    void CameraLook()
    {
        currentCameraRotationX += inputDelta.y * mouseSensitivity;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -mouseAltitudeLimit, mouseAltitudeLimit);
        mainCamera.localEulerAngles = new Vector3(-currentCameraRotationX, 0, 0);
        transform.eulerAngles += new Vector3(0, inputDelta.x * mouseSensitivity, 0);
    }

    public void MoveInputRecieve(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            inputMovement = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            inputMovement = Vector2.zero;
        }
    }
    
    public void LookInputRecieve(InputAction.CallbackContext context)
    {
        inputDelta = context.ReadValue<Vector2>();
    }

    public void JumpInputRecieve(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGround())
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void PerspectiveInputRecieve(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())
        {
            ChangePerspective();
        }
    }

    bool IsGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, 0.6f, groundLayerMask))
            return true;
        return false;
    }

    public void ChangeSpeed(float speed)
    {
        moveSpeed += speed;
    }

    void ChangePerspective()
    {
        if (isFirstPerson)
        {
            mainCamera.position = new Vector3(0, -1.5f, -5);
        }
        else
        {
            mainCamera.position = Vector3.zero;
        }
        isFirstPerson = !isFirstPerson;
    }
}
