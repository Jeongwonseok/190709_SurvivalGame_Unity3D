using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // 공유 자원 , 클래스 변수 = 정적 변수
    // 무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false;
    // 현재 무기 , 현재 무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // 현재 무기의 타입 
    [SerializeField]
    private string currentWeaponType;

    // 무기 교체 딜레이 
    [SerializeField]
    private float changeWeaponDelayTime;
    // 무기 교체가 완전히 끝난 시점
    [SerializeField]
    private float changeWeaponEndDelayTime;

    // 무기 종류들 전부 관리 
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private Hand[] hands;

    // 키값 줘서 관리하기 편하게 해줌!! >> 무기 접근 용이 
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

    // 필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;


    
    

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }

        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if(!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손")); // 무기교체실행 (맨손)
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1")); // 무기교체실행 (서브머신건)
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    // 무기 취소 함수
    private void CancelPreWeaponAction()
    {
        switch(currentWeaponType)
        {
            case "GUN" :
                theGunController.CancleFineSight();
                theGunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;

        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if(_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
        else if(_type == "HAND")
            theHandController.HandChange(handDictionary[_name]);
    }
}
