using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; // 총의 이름
    public float range; // 사정 거리
    public float accracy; // 정확도 
    public float fireRate; // 연사속도
    public float reloadTime; // 재장전 속도

    public int damage; // 총의 데미지

    public int reloadBulletCount; // 총알 재장전 개수
    public int currentBulletCount; // 현재 탄알집에 남아있는 총알의 개수
    public int maxBulletCount; // 최대 소유 가능 총알 개수
    public int carryBulletCount; // 현재 소유하고있는 총알 개수

    public float retroActionForce; // 반동 세기
    public float retroActionFineSightForce; // 정조준 시 반동 세기

    public Vector3 fineSightOriginPos; // 정조준할때 총의 위치
    public Animator anim; // 애니메이션 
    public ParticleSystem muzzleFlash; // 총구 섬광 변수

    //public AudioClip fire_sound; // 총 발사할때 사운드 변수



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
