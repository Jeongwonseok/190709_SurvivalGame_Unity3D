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
	private CloseWeapon[] hands;
	[SerializeField]
	private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickaxes;

    // 키값 줘서 관리하기 편하게 해줌!! >> 무기 접근 용이 
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
	private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    // 필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickaxeController thePickaxeController;





    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }

        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
		for (int i = 0; i < axes.Length; i++)
		{
			axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
		}
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
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
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe")); // 무기교체실행 (도끼)
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe")); // 무기교체실행 (곡괭이)
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
            case "AXE":
                AxeController.isActivate = false;
                break;
            case "PICKAXE":
                PickaxeController.isActivate = false;
                break;

        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if(_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
		else if (_type == "HAND")
			theHandController.CloseWeaponChange(handDictionary[_name]);
        else if (_type == "AXE")
            theAxeController.CloseWeaponChange(axeDictionary[_name]);
        else if (_type == "PICKAXE")
            thePickaxeController.CloseWeaponChange(pickaxeDictionary[_name]);
    }

    public IEnumerator WeaponInCoroutine()
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        currentWeapon.gameObject.SetActive(false);
    }

    public void WeaponOut()
    {
        isChangeWeapon = false;

        currentWeapon.gameObject.SetActive(true);
    }
}
