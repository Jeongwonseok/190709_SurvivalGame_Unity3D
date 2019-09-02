using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    // 활성화 여부
    public static bool isActivate = true;

    private void Start()
    {
        // 웨폰매니저에 컴포넌트 부여
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        // 웨폰매니저에 애니메이션 부여
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    // Update is called once per frame
    void Update()
    {
        // 만약 활성화 상태이면 공격 시도
        if (isActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject() && WeaponManager.currentWeapon.gameObject.transform.name == "Pickaxe")
            {
                // 만약 현재 태그가 Rock 이면 깎기
                if (hitInfo.transform.tag == "Rock")
                    hitInfo.transform.GetComponent<Rock>().Mining();
                // 만약 NPC(돼지) 이면 데미지 주기
                else if (hitInfo.transform.tag == "WeakAnimal")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                }

                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
