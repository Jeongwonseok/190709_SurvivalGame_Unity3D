using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{

    [SerializeField]
    protected string animalName; // 동물의 이름

    [SerializeField]
    protected int hp; // 동물의 체력

    [SerializeField]
    protected float walkSpeed; // 걷기 스피드
    [SerializeField]
    protected float runSpeed; // 뛰기 스피드
    [SerializeField]
    protected float turningSpeed; // 회전 스피드

    protected float applySpeed;


    protected Vector3 direction; // 방향

    // 상태변수
    protected bool isAction; // 행동중인지 아닌지 판별
    protected bool isWalking; // 걷는지 안걷는지 판별
    protected bool isRunning; // 뛰는지 안뛰는지
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

    [SerializeField]
    protected AudioClip[] sound_Normal;
    [SerializeField]
    protected AudioClip sound_Hurt;
    [SerializeField]
    protected AudioClip sound_Dead;



    // Start is called before the first frame update
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        // 만약 돼지가 죽지 않았으면 실행
        if (!isDead)
        {
            Move();
            Rotation();
            ElapseTime();
        }
    }

    // 돼지 움직이기
    protected void Move()
    {
        if (isWalking || isRunning)
            rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
    }

    // 돼지 회전
    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    // 경과 시간 (걷기, 뛰기, 먹기 등등..)
    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
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
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
    }

    // 걷기 시도
    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
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
