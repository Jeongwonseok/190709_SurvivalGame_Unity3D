 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float finishTime;
    
    // true 일때만 데미지 받도록!!
    private bool isHurt = false;
    private bool isActivate = false;

    public IEnumerator ActivatedTrapCoroutine()
    {
        isActivate = true;

        yield return new WaitForSeconds(finishTime);
        isActivate = false;
        isHurt = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isActivate)
        {
            if(!isHurt)
            {
                isHurt = true;

                if(other.transform.name == "Player")
                {
                    other.transform.GetComponent<StatusController>().DecreaseHP(damage);
                }
            }
        }
    }
}
