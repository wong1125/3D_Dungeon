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
    [SerializeField] private float firstCameraHeight;
    [SerializeField] private float thirdCameraDistance;
    private Vector2 inputDelta;
    private Vector3 foward;
    private float currentCameraRotationX;
    private bool isFirstPerson = true;

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
        foward = mainCamera.forward;
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
        transform.eulerAngles += isFirstPerson ? new Vector3(0, inputDelta.x * mouseSensitivity, 0) : new Vector3(0, inputDelta.x * mouseSensitivity, 0);
        if (!isFirstPerson)
        {
            PolarAngleCaulator(currentCameraRotationX);
        }
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
        if (!isFirstPerson)
        {
            Vector3 camEuler = mainCamera.rotation.eulerAngles;
            transform.eulerAngles = new Vector3(0f, camEuler.y, 0f);

            mainCamera.localPosition = new Vector3(0f, firstCameraHeight, 0f); // 로컬 0,0,0 대신 머리 위치로
            mainCamera.localEulerAngles = new Vector3(-currentCameraRotationX, 0, 0);
            Debug.Log(transform.position);
        }
        else
        {
            PolarAngleCaulator(currentCameraRotationX);
        }
        isFirstPerson = !isFirstPerson;
    }


    //3인칭 시점에서, 극좌표를 이용해 마우스 움직임에 따라 카메라 위치 변경
    //gpt가 만들었습니다.
    void PolarAngleCaulator(float currentCameraRotationX)
    {
        // 피벗(플레이어 머리 근처). 필요하면 상단을 SerializeField로 빼서 조절해도 됨.
        const float headHeight = 1.6f;
        Vector3 pivot = transform.position + Vector3.up * headHeight;

        // 거리(반지름)
        float r = thirdCameraDistance;

        // 방위각(수평 각도): 플레이어의 Y 오일러각을 부호 반대로 사용
        float azimuthDeg = -transform.eulerAngles.y - 90f;

        // 고도(수직 각도):  구면좌표에서도 동일하게 부호 반대.
        float elevationDeg = -currentCameraRotationX;

        float elev = elevationDeg * Mathf.Deg2Rad;
        float azi = azimuthDeg * Mathf.Deg2Rad;

        // 구면좌표 -> 월드 오프셋
        Vector3 offset;
        offset.x = r * Mathf.Cos(elev) * Mathf.Cos(azi);
        offset.y = r * Mathf.Sin(elev);
        offset.z = r * Mathf.Cos(elev) * Mathf.Sin(azi);

        Vector3 desiredPos = pivot + offset;

        // 위치 & 항상 타겟을 바라보게
        mainCamera.position = desiredPos;
        mainCamera.rotation = Quaternion.LookRotation(pivot - desiredPos, Vector3.up);
    }
}
