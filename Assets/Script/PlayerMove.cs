using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody charRigidbody;
    public CapsuleCollider capsuleCollider;
    [Header("이동")][Tooltip("플레이어 이동 속도")]
    public float moveSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float applySpeed;
    public float jumpForce;
    //상태 변수
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;
    //앉을 시 앉을 정도 변수
    public float crouchPosY;
    public float originPosY;
    public float applyCrouchPosY;

    [Header("시점")][Tooltip("플레이어 마우스 회전 속도")]
    public float turnSpeed = 4.0f;
    public Transform CameraTransform;
    public Camera Cam; // 메인카메라
    public float cameraRotationLimit;
    public float currentCameraRotationX;
    public float lookSensitivity;

    private float xRotate = 0.0f; // 사용할 X축 회전량을 별도 정의 ( 카메라의 위 아래 방향 )

    void Start()
    {
        //컴포넌트 할당
        charRigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        //초기화
        applySpeed = moveSpeed;

        originPosY = Cam.transform.localPosition.y;
        applyCrouchPosY = originPosY;

        theGunControll = FineObjectOfType<GunControll>();

    }

    void Update()
    {
        MouseRotation(); // 마우스의 움직임에 따라 카메라를 회전 시킨다
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
        // 이동 스크립트 2가지 종류
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
        //float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed; // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
        //float yRotate = CameraTransform.eulerAngles.y + yRotateSize; // 현재 y축 회전 값에 더한 새로운 회전각도 계산

        //float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed; // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산
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
