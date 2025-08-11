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

        //y�� ȸ�� ����
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

            mainCamera.localPosition = new Vector3(0f, firstCameraHeight, 0f); // ���� 0,0,0 ��� �Ӹ� ��ġ��
            mainCamera.localEulerAngles = new Vector3(-currentCameraRotationX, 0, 0);
            Debug.Log(transform.position);
        }
        else
        {
            PolarAngleCaulator(currentCameraRotationX);
        }
        isFirstPerson = !isFirstPerson;
    }


    //3��Ī ��������, ����ǥ�� �̿��� ���콺 �����ӿ� ���� ī�޶� ��ġ ����
    //gpt�� ��������ϴ�.
    void PolarAngleCaulator(float currentCameraRotationX)
    {
        // �ǹ�(�÷��̾� �Ӹ� ��ó). �ʿ��ϸ� ����� SerializeField�� ���� �����ص� ��.
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
}
