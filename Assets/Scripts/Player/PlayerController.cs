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

        //������ y�� ȸ�� ����
        var angularVelocity = rb.angularVelocity;
        angularVelocity.y = 0f;
        rb.angularVelocity = angularVelocity;
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    //��ǲ�� ���� ���������� ��ȯ
    void Move()
    {
        //�� ���� �� ���� ����: �̰� ������ ���� �������鼭 Vector.Foward ���ص� ���� ���Ѵ�.
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

        //��ٸ� ������
        //�ٵ� ��ٸ��� ���� forcemode�� ����ؼ� �����ؾ��ϴ� ������ �� �𸣰ڽ��ϴ�.
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

    //�� ����
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

    //��ٸ� ����
    bool IsLadder()
    {
        Ray ray = new Ray(transform.position, foward);
        if (Physics.Raycast(ray, 0.8f, ladderLayerMask))
            return true;
        return false;
    }

    //Q�� 1��Ī, 3��Ī ����
    void ChangePerspective()
    {
        if (!isFirstPerson)
        {
            //3��Ī -> 1��Ī �� ù ���� ����ȭ
            Vector3 camEuler = mainCamera.rotation.eulerAngles;
            transform.eulerAngles = new Vector3(0f, camEuler.y, 0f);

            //ī�޶� ��ġ ���󺹱�
            mainCamera.localPosition = new Vector3(0f, firstCameraHeight, 0f); 
            mainCamera.localEulerAngles = new Vector3(-currentCameraRotationX, 0, 0);

            CharacterManager.Instance.Player.investigation.ThirdPespectiveray(false);
        }
        else
        {
            PolarAngleCaulator(currentCameraRotationX);
            
            //��ȣ�ۿ� Ray ���� ����
            CharacterManager.Instance.Player.investigation.ThirdPespectiveray(true);
        }
        isFirstPerson = !isFirstPerson;
    }


    //3��Ī ��������, ����ǥ�� �̿��� ���콺 �����ӿ� ���� ī�޶� ��ġ ����
    //gpt�� ��������ϴ�.
    void PolarAngleCaulator(float currentCameraRotationX)
    {
        // �ǹ�(�÷��̾� �Ӹ� ��ó).
        const float headHeight = 1.6f;
        Vector3 pivot = transform.position + Vector3.up * headHeight;

        // �Ÿ�(������)
        float r = thirdCameraDistance;

        // ������(���� ����): �÷��̾��� Y ���Ϸ����� ��ȣ �ݴ�� ���
        float azimuthDeg = -transform.eulerAngles.y - 90f;

        // ��(���� ����):  ������ǥ������ �����ϰ� ��ȣ �ݴ�.
        float elevationDeg = -currentCameraRotationX;

        float elev = elevationDeg * Mathf.Deg2Rad;
        float azi = azimuthDeg * Mathf.Deg2Rad;

        // ������ǥ -> ���� ������
        Vector3 offset;
        offset.x = r * Mathf.Cos(elev) * Mathf.Cos(azi);
        offset.y = r * Mathf.Sin(elev);
        offset.z = r * Mathf.Cos(elev) * Mathf.Sin(azi);

        Vector3 desiredPos = pivot + offset;

        // ��ġ & �׻� Ÿ���� �ٶ󺸰�
        mainCamera.position = desiredPos;
        mainCamera.rotation = Quaternion.LookRotation(pivot - desiredPos, Vector3.up);
    }
    
    //���� �ܺο��� ���� �����ϴ� �Լ���
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
