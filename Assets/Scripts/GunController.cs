using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // 활성화 여부
    public static bool isActivate = false;

    // 현재 총 선언
    [SerializeField]
    private Gun currentGun;

    // 현재 연사 속도 변수 선언
    private float currentFireRate;

    // 재장전 상태변수 선언 >> false 일때만 발사
    private bool isReload = false;
    [HideInInspector]
    public bool isFineSightMode = false;


    // 정조준하고 원래로 돌아올때의 벡터값 변수
    [SerializeField]
    private Vector3 originPos;

    // 오디오 담을 소스 선언
    // 필요한 사운드 이름
    [SerializeField]
    private string shoot_Sound;

    // 레이저 충돌 정보 변수
    private RaycastHit hitInfo;
    [SerializeField]
    protected LayerMask layerMask; // 총쏠때 Player와 충돌하지않게 하기위해

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    // 피격 이펙트
    [SerializeField]
    private GameObject hit_effect_prefab;

    void Start()
    {
        // 초기화
        originPos = Vector3.zero;
        // 시작과 동시에 오디오 컴포넌트 선언
        //audioSource = GetComponent<AudioSource>();
        // originPos = transform.localPosition;
        theCrosshair = FindObjectOfType<Crosshair>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Inventory.inventoryActivated)
        {
            if (isActivate)
            {
                // 발사하기전 장전중에 발사 못하도록 해주는 메서드
                GunFireRateCalc();
                // 총쏘기 시도
                TryFire();
                // 수동 재장전 시도
                TryReload();
                // 정조준 시도
                TryFineSight();
            }
        }
    }

    // 연사 속도 계산 메서드
    // 즉, 점점 연사속도 줄여주는 메서드
    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;  // deltaTime 은 60분의1초 정도!!
        }
    }

    // 총쏘기 시도
    private void TryFire()
    {
        // 왼쪽 마우스 클릭하거나 현재 연사속도가 0이면 , 달리는 상태가 아니면
        if(Input.GetButton("Fire1") && currentFireRate<=0 && !isReload && !PlayerController.isRun)
        {
            // 총쏘기
            Fire();
        }
    }

    // 발사전 계산
    private void Fire()
    {
        if(!isReload)
        {
            if (currentGun.currentBulletCount > 0)
            {
                Shoot(); // 진짜 발사하기
            }
            else
            {
                CancleFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
        
    }

    // 발사후 계산
    private void Shoot()
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--;
        // 연사속도 재계산
        currentFireRate = currentGun.fireRate;

        // 발사와 동시에 sound 집어넣기
        //PlaySE(currentGun.fire_sound);
        SoundManager.instance.PlaySE(shoot_Sound);

        // 섬광 발동
        currentGun.muzzleFlash.Play();

        //
        Hit();

        // 반동 실시
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    // 피격 이펙트 메서드
    private void Hit()
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward +
            new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accracy , theCrosshair.GetAccuracy() + currentGun.accracy) ,
                        Random.Range(-theCrosshair.GetAccuracy() - currentGun.accracy, theCrosshair.GetAccuracy() + currentGun.accracy) ,
                        0)
                , out hitInfo, currentGun.range, layerMask))
        {
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f); // 메모리 관리를 위한 destroy 
        } 
    }

    // 수동 재장전 메서드
    private void TryReload()
    {
        if(Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancleFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;

        }
    }

    // 재장전 메서드
    IEnumerator ReloadCoroutine()
    {         
        // if) 소유하고 있는 총알의 개수가 0보다 크면
        if (currentGun.carryBulletCount > 0)
        {

            isReload = true;

            currentGun.anim.SetTrigger("Reload");// 재장전 트리거 발동

            currentGun.carryBulletCount += currentGun.currentBulletCount; // 현재 소유한 총알 버리지 않고 누적해서 재장전 하기
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime); // 재장전 시간동안 잠시 대기

            // if) 소유하고 있는 총알의 개수가 재장전 개수보다 크면 
            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount; // 현재 남아있는 총알 = 재장전 총알 개수
                currentGun.carryBulletCount -= currentGun.reloadBulletCount; // 소유하고 있는 총알 개수에 재장전한 총알 개수 빼주
            }
            // 소유하고 있는 총알이 얼마 없으면 >> 재장전에 필요한 개수보다 적으면
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount; // 전부다 재장전하고
                currentGun.carryBulletCount = 0; // 소유하고있는 총알개수 초기화
            }

            isReload = false;
        }
        else
        {
            // 총알 없을때 나는 소리 추가!!
            Debug.Log("소유한 총알이 없습니다.");
        }
    }

    // 정조준 메서드
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    // 정조준 취소 메서드 >> PlayerController 에서 사용할수 있도록 public으로 지정
    public void CancleFineSight()
    {
        if(isFineSightMode)
        {
            FineSight();
        }
    }

    // 정조준 실행
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        theCrosshair.FineSightAnimation(isFineSightMode);
        if(isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeActivateCoroutine());
        }
    }

    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator FineSightDeActivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    // 총기반동 메서드
    IEnumerator RetroActionCoroutine()
    {
        // 정조준 안했을때 최대 반동
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        // 정조준 했을때 최대 반동
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        // 정조준 상태가 아닐경우
        if(!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            // 반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce-0.02f)
            {
                // vector3 Lerp를 이용해 정조준 안했을때 최대 반동 vector 까지 0.4f만큼 준다.
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // 원위치
            while (currentGun.transform.localPosition != originPos)
            {
                // 현재 자기 위치부터 원래의 위치까지 반복시킨다. >> 느린 속도로...
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        // 정조준 상태일 경우 
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // 반동 시작 (약간의 여유 주기)
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                // vector3 Lerp를 이용해 정조준 안했을때 최대 반동 vector 까지 0.4f만큼 준다.
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // 원위치 (약간의 여유 주기)
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                // 현재 자기 위치부터 원래의 위치까지 반복시킨다. >> 느린 속도로...
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    // 오디오 집어넣어주는 메서드 >> 매게변수 : AudioClip
    //private void PlaySE(AudioClip _clip)
    //{
    //    audioSource.clip = _clip;
    //    audioSource.Play();
    //}

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }

    public void GunChange (Gun _gun)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentGun = _gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
}
