using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{

    // 뛰기
    public void Run(Vector3 _targetPos)
    {
        destination = new Vector3(transform.position.x - _targetPos.x, 0f, transform.position.z - _targetPos.z).normalized;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        nav.speed = runSpeed;
        anim.SetBool("Running", isRunning);
    }

    // Animal 클래스의 Damage 상속받아 오버라이드
    public override void Damage(int _dmg, Vector3 _targetPos)
    {
        base.Damage(_dmg, _targetPos);
        if(!isDead)
        {
            Run(_targetPos);
        }
    }

}
