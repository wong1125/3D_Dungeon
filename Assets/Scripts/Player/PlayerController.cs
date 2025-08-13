using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool controllerOn = true;
    
    [Header("Movement Parameter")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask ladderLayerMask;
    private Vector2 inputMovement;
    private bool isMoving = false;
    public bool IsMoving { get { return isMoving; } }
    private int runSwitch = 0;
    public int RunSwitch { get { return runSwitch; } }
    private int jumpSwitchDefault = 1;
    private int jumpSwitch = 1;

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
    private PlayerCondition playerCondition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerCondition = CharacterManager.Instance.Player.condition;
        Cursor.lockState = CursorLockMode.Locked;
        rb.MovePosition(Vector3.zero);
    }


    private void FixedUpdate()
    {
        if (controllerOn)
            Move();

        //임의의 y축 회전 막기
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
        actaulMovementVector *= moveSpeed + runSwitch * runSpeed;

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
            isMoving = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            inputMovement = Vector2.zero;
            isMoving = false;
        }
    }

    public void LookInputRecieve(InputAction.CallbackContext context)
    {
        inputDelta = context.ReadValue<Vector2>();
    }

    public void JumpInputRecieve(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && (jumpSwitch > 0 || IsGround()))
        {
            jumpSwitch--;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }

        //사다리 오르기
        //근데 사다리를 굳이 forcemode를 사용해서 구현해야하는 이유는 잘 모르겠습니다.
        else if (context.phase == InputActionPhase.Performed && IsLadder())
        {
            rb.AddForce(Vector2.up * jumpPower * 0.5f, ForceMode.VelocityChange);
            if (rb.velocity.magnitude > 3.0f)
                rb.velocity = rb.velocity.normalized * 3.0f;
        }
    }

    public void PerspectiveInputRecieve(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            ChangePerspective();
        }
    }

    public void RunInputRecieve(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && IsGround() && playerCondition.CurrentStamina > 0)
        {
            runSwitch = 1;
        }
        else
        {
            runSwitch = 0;
        }
    }

    //땅 판정
    bool IsGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, 0.6f, groundLayerMask))
        {
            jumpSwitch = jumpSwitchDefault;
            return true;
        }
        return false;
    }

    //사다리 판정
    bool IsLadder()
    {
        Ray ray = new Ray(transform.position, foward);
        if (Physics.Raycast(ray, 0.8f, ladderLayerMask))
            return true;
        return false;
    }

    //Q로 1인칭, 3인칭 변경
    void ChangePerspective()
    {
        if (!isFirstPerson)
        {
            //3인칭 -> 1인칭 때 첫 시점 동기화
            Vector3 camEuler = mainCamera.rotation.eulerAngles;
            transform.eulerAngles = new Vector3(0f, camEuler.y, 0f);

            //카메라 위치 원상복구
            mainCamera.localPosition = new Vector3(0f, firstCameraHeight, 0f); 
            mainCamera.localEulerAngles = new Vector3(-currentCameraRotationX, 0, 0);

            CharacterManager.Instance.Player.investigation.ThirdPespectiveray(false);
        }
        else
        {
            PolarAngleCaulator(currentCameraRotationX);
            
            //상호작용 Ray 길이 변경
            CharacterManager.Instance.Player.investigation.ThirdPespectiveray(true);
        }
        isFirstPerson = !isFirstPerson;
    }


    //3인칭 시점에서, 극좌표를 이용해 마우스 움직임에 따라 카메라 위치 변경
    //gpt가 만들었습니다.
    void PolarAngleCaulator(float currentCameraRotationX)
    {
        // 피벗(플레이어 머리 근처).
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
    
    //이하 외부에서 변수 변경하는 함수들
    public void ChangeSpeed(float speed)
    {
        moveSpeed += speed;
    }

    public void ChangeJumpPower(float jump)
    {
        jumpPower += jump;
    }

    public void BanRunning()
    {
        runSwitch = 0;
    }

    public void ControllerSwitch()
    {
        controllerOn = !controllerOn;
    }

    public void MultipleJump(int jump)
    {
        jumpSwitchDefault = jump;
    }
}
