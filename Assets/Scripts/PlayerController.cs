using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 스피드 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;
    [SerializeField]
    private float swimSpeed;
    [SerializeField]
    private float swimFastSpeed;
    [SerializeField]
    private float upSwimSpeed;

    // 점프 변수
    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isWalk = false;
    public static bool isRun = false;   // 달리는 상황에서 총알 안나가게 하기위해 public static 으로 선언!!
    private bool isGround = true;
    private bool isCrouch = false;

    // 움직임 체크 변수
    private Vector3 lastPos;

    // 얼마나 앉을지 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // 땅 착지여부
    private CapsuleCollider capsuleCollider;

    // 민감도
    [SerializeField]
    private float lookSesitivity;

    // 카메라 한
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private GunController theGunController; // 달릴때 정조준모드 해제 시키기 위해 선언한 GunController 변수
    private Crosshair theCrosshair;
    private StatusController theStatusController;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theStatusController = FindObjectOfType<StatusController>();

        // 초기화
        applySpeed = walkSpeed; 
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    void FixedUpdate()
    {
        MoveCheck();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.canPlayerMove)
        {
            WaterCheck();
            IsGround();
            TryJump();

            if(!GameManager.isWater)
            {
                TryRun();
            }

            TryCrouch();
            Move();
            CameraRotation();
            CharacterRotation();
        }
    }

    // 물 체크
    private void WaterCheck()
    {
        if(GameManager.isWater)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
                applySpeed = swimFastSpeed;
            else
                applySpeed = swimSpeed;
        }
    }

    // 앉기 시도
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    // 앉기 동작
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);

        isWalk = false;
        theCrosshair.WalkingAnimation(isWalk);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }

    // 동작을 부드럽게 연결시용킬때용 사용!! 중요함
    // 위에 있는 동작들을 실행하는 도중에 코루틴을 만나면 같이 병렬 실행한다.
    // CPU 2개는 아니고 빠르게 왔다갔다 병렬처리!!
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            count++;
            // _posY 부터 applyCrouchposY 까지 0.3f씩 서서히 증가하도록 하는 기능
            _posY = Mathf.Lerp(_posY,applyCrouchPosY,0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 16)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }
    // 지면 체크
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y+0.1f);
        theCrosshair.JumpingAnimation(!isGround);
    }

    // 점프 시도
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0 && !GameManager.isWater)
        {
            Jump();
        }
        else if (Input.GetKey(KeyCode.Space) && GameManager.isWater)
        {
            UpSwim();
        }
    }

    // 물 위에 떠오르기
    private void UpSwim()
    {
        myRigid.velocity = transform.up * upSwimSpeed;
    }

    // 점프 동작
    private void Jump()
    {
        // 앉은 상태에서 점프 시 앉은 상태 해제
        if(isCrouch)
        {
            Crouch();
        }
        theStatusController.DecreaseStamina(100);
        myRigid.velocity = transform.up * jumpForce;
    }

    // 달리기 시도
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSP() > 0)
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0)
        {
            RunningCancel();
        }
    }

    // 달리기 동작
    private void Running()
    {
        // 앉은 상태에서 달리기 시 앉은 상태 해제
        if (isCrouch)
        {
            Crouch();
        }

        theGunController.CancleFineSight();  // 달리기 할때 정조준 모드 해제!!

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        theStatusController.DecreaseStamina(10);
        applySpeed = runSpeed;
    }

    // 달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    // 움직임(상,하,좌,우)
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        // x 좌 , 우
        // 오른쪽 1 왼쪽 -1 안누르면 0 리턴
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        // z 정면 , 뒤

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        // ex) 오른쪽 누르면 >>  (1,0,0) * 1
        Vector3 _moveVertical = transform.forward* _moveDirZ;
        // ex) 뒤 누르면 >>  (0,0,1) * -1

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + (_velocity * Time.deltaTime));
    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);  
            lastPos = transform.position;
        }
    }

    private void CharacterRotation() //좌우 캐릭터 회전
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSesitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    private void CameraRotation()  //상하 카메라 회전
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSesitivity;
        currentCameraRotationX = currentCameraRotationX - _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
