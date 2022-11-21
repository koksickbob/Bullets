using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody charRigidbody;
    public CapsuleCollider capsuleCollider;
    [Header("�̵�")][Tooltip("�÷��̾� �̵� �ӵ�")]
    public float moveSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float applySpeed;
    public float jumpForce;
    //���� ����
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;
    //���� �� ���� ���� ����
    public float crouchPosY;
    public float originPosY;
    public float applyCrouchPosY;

    [Header("����")][Tooltip("�÷��̾� ���콺 ȸ�� �ӵ�")]
    public float turnSpeed = 4.0f;
    public Transform CameraTransform;
    public Camera Cam; // ����ī�޶�
    public float cameraRotationLimit;
    public float currentCameraRotationX;
    public float lookSensitivity;

    private float xRotate = 0.0f; // ����� X�� ȸ������ ���� ���� ( ī�޶��� �� �Ʒ� ���� )

    void Start()
    {
        //������Ʈ �Ҵ�
        charRigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        //�ʱ�ȭ
        applySpeed = moveSpeed;

        originPosY = Cam.transform.localPosition.y;
        applyCrouchPosY = originPosY;

        theGunControll = FineObjectOfType<GunControll>();

    }

    void Update()
    {
        MouseRotation(); // ���콺�� �����ӿ� ���� ī�޶� ȸ�� ��Ų��
        CharacterRotation();
        Move();
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();

        
        
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            jump();
        }
    }

    private void jump()
    {
        if (isCrouch)
            Crouch();

        charRigidbody.velocity = transform.up * jumpForce;

    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        if (isCrouch)
            Crouch();

        theGunControll.CancelFineSight();

        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = moveSpeed;
    }

    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    private void Crouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = moveSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {
        float _posY = Cam.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.2f);
            Cam.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        Cam.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    private void Move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");
        // �̵� ��ũ��Ʈ 2���� ����
        Vector3 _moveHorizontal = transform.right * hAxis;
        Vector3 _moveVertical = transform.forward * vAxis;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        charRigidbody.MovePosition(transform.position + _velocity * Time.deltaTime);
        //Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;

        //charRigidbody.velocity = inputDir * moveSpeed;

        //transform.LookAt(transform.position + inputDir);

    }

    void MouseRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        Cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        //float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed; // �¿�� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� �¿�� ȸ���� �� ���
        //float yRotate = CameraTransform.eulerAngles.y + yRotateSize; // ���� y�� ȸ�� ���� ���� ���ο� ȸ������ ���

        //float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed; // ���Ʒ��� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� ȸ���� �� ���
        //xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);
        //CameraTransform.eulerAngles = new Vector3(xRotate, yRotate, 0);

    }
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _charaterRotationY = new Vector3(0f, _yRotation, 0f) * turnSpeed;
        charRigidbody.MoveRotation(charRigidbody.rotation * Quaternion.Euler(_charaterRotationY));

    }

}
