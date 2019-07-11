using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // 바위의 체력
    [SerializeField]
    private int hp;

    // 파편 제거시간
    [SerializeField]
    private float destroyTime;

    // 구체 콜라이더 
    [SerializeField]
    private SphereCollider col;

    // 필요한 게임 오브젝트
    // 일반 바위
    [SerializeField]
    private GameObject go_rock;
    // 깨진 바위
    [SerializeField]
    private GameObject go_debris;
    // 채굴 이펙트
    [SerializeField]
    private GameObject go_effect_prefabs;

    // 필요한 사운드 이름
    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;

    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);

        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);

        hp--;
        if(hp<=0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);
        col.enabled = false;
        Destroy(go_rock); // 바로 삭제

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime); // 일정시간 간격 주고 삭제
    }
}
