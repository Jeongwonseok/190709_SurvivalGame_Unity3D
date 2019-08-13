﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    protected StatusController thePlayerStatus;

    [SerializeField]
    protected string animalName; // 동물의 이름

    [SerializeField]
    protected int hp; // 동물의 체력

    [SerializeField]
    protected float walkSpeed; // 걷기 스피드
    [SerializeField]
    protected float runSpeed; // 뛰기 스피드

    protected Vector3 destination; // 목적지

    // 상태변수
    protected bool isAction; // 행동중인지 아닌지 판별
    protected bool isWalking; // 걷는지 안걷는지 판별
    protected bool isRunning; // 뛰는지 안뛰는지
    protected bool isChasing; // 추격중인지 판별
    protected bool isAttacking; // 공격중인지 판별
    protected bool isDead;    // 죽었는지 안죽었는지

    [SerializeField]
    protected float walkTime; // 걷기 시간
    [SerializeField]
    protected float waitTime; // 대기 시간
    [SerializeField]
    protected float runTime; // 뛰기 시간

    protected float currentTime;

    // 필요한 컴포넌트
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected Rigidbody rigid;
    [SerializeField]
    protected BoxCollider boxCol;
    protected AudioSource theAudio;
    protected NavMeshAgent nav; // 네비게이션 컴포넌트
    protected FieldOfViewAngle theViewAngle;


    [SerializeField]
    protected AudioClip[] sound_Normal;
    [SerializeField]
    protected AudioClip sound_Hurt;
    [SerializeField]
    protected AudioClip sound_Dead;



    // Start is called before the first frame update
    void Start()
    {
        thePlayerStatus = FindObjectOfType<StatusController>();
        theViewAngle = GetComponent<FieldOfViewAngle>();
        nav = GetComponent<NavMeshAgent>();
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // 자식객체에서 수정 가능하도록 >> virtual로 선언해야함!! >> pig 클래스에서 수정하도록 하기 위해!!
    // Update is called once per frame
    protected virtual void Update()
    {
        // 만약 돼지가 죽지 않았으면 실행
        if (!isDead)
        {
            Move();
            ElapseTime();
        }
    }

    // 돼지 움직이기
    protected void Move()
    {
        if (isWalking || isRunning)
            // rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
            // SetDestination 이용해서 목적지 설정
            nav.SetDestination(transform.position + destination * 5f);
    }


    // 경과 시간 (걷기, 뛰기, 먹기 등등..)
    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 && !isChasing && !isAttacking)
            {
                ReSet();
            }
        }
    }

    // 리셋 >> virtual 이용하여 다른 클래스에서 기능을 완성할수 있도록!!
    protected virtual void ReSet()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        nav.speed = walkSpeed;
        nav.ResetPath();
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        destination.Set(Random.Range(-0.2f, -0.2f), 0f, Random.Range(0.5f, 1f));
    }

    // 걷기 시도
    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        nav.speed = walkSpeed;
        Debug.Log("걷기");
    }



    // 데미지
    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }

            PlaySE(sound_Hurt);
            anim.SetTrigger("Hurt");
        }
    }

    // 죽기
    protected void Dead()
    {
        PlaySE(sound_Dead);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
    }

    // 랜덤 사운드
    protected void RandomSound()
    {
        int _random = Random.Range(0, 3); // 일상 사운드 3개
        PlaySE(sound_Normal[_random]);
    }

    // 사운드 실행
    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

}
