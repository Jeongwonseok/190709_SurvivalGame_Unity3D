using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{

    [SerializeField]
    private float range; // 습득 가능한 최대 거리

    private bool pickupActivated = false; // 아이템 습득이 가능할 시 true
    
    private bool dissolveActivated = false; // 고기 해체 가능할 시 true
    private bool isDissolving = false; // 고기 해체 중에는 true

    private bool fireLookActivated = false; // 불을 근접해서 바라볼 시 true

    private RaycastHit hitInfo; // 충돌체 정보 저장

    [SerializeField]
    private LayerMask layerMask; // 아이템 레이어에만 반응하도록 레이어마스크 설정

    // 필요한 컴포넌트
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;
    [SerializeField]
    private WeaponManager theWeaponManager;
    [SerializeField]
    private QuickSlotController theQuickSlot;
    [SerializeField]
    private Transform tf_MeatDissolveTool; // 고기해체 툴

    [SerializeField]
    private string sound_Meat; // 소리 재생


    // Update is called once per frame
    void Update()
    {
        CheckAction();
        TryAction();
    }

    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckAction();
            CanPickUp();
            CanMeat();
            CanDropFire();
        }
    }

    private void CanPickUp()
    {
        if(pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다.");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    private void CanMeat()
    {
        if(dissolveActivated)
        {
            if ((hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal") && hitInfo.transform.GetComponent<Animal>().isDead && !isDissolving)
            {
                isDissolving = true;
                InfoDisappear();
                StartCoroutine(MeatCoroutine());
            }
        }
    }

    IEnumerator MeatCoroutine()
    {
        WeaponManager.isChangeWeapon = true;
        WeaponSway.isActivated = false;
        WeaponManager.currentWeaponAnim.SetTrigger("Weapon_Out");
        PlayerController.isActivated = false;
        yield return new WaitForSeconds(0.2f);

        WeaponManager.currentWeapon.gameObject.SetActive(false);
        tf_MeatDissolveTool.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        SoundManager.instance.PlaySE(sound_Meat);
        yield return new WaitForSeconds(1.8f);

        theInventory.AcquireItem(hitInfo.transform.GetComponent<Animal>().GetItem(), hitInfo.transform.GetComponent<Animal>().itemNumber);

        WeaponManager.currentWeapon.gameObject.SetActive(true);
        tf_MeatDissolveTool.gameObject.SetActive(false);

        PlayerController.isActivated = true;
        WeaponSway.isActivated = true;
        WeaponManager.isChangeWeapon = false;
        isDissolving = false;
    }

    private void CanDropFire()
    {
        if (fireLookActivated)
        {
            if (hitInfo.transform.tag == "Fire" && hitInfo.transform.GetComponent<Fire>().GetIsFire())
            {
                Slot _selectedSlot = theQuickSlot.GetSelectedSlot();
                if (_selectedSlot.item != null)
                    DropAnItem(_selectedSlot);
            }
        }
    }

    private void DropAnItem(Slot _selectedSlot)
    {
        switch (_selectedSlot.item.itemType)
        {
            case Item.ItemType.Used:
                if (_selectedSlot.item.itemName.Contains("고기"))
                {
                    Instantiate(_selectedSlot.item.itemPrefab, hitInfo.transform.position + Vector3.up, Quaternion.identity);
                    theQuickSlot.DecreaseSelectedItem();
                }
                break;
            case Item.ItemType.Ingredient:
                break;
        }
    }

    private void CheckAction()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
                ItemInfoAppear();

            else if (hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal")
                MeatInfoAppear();

            else if (hitInfo.transform.tag == "Fire")
                FireInfoAppear();

            else
                InfoDisappear();
        }
        else
            InfoDisappear();
    }

    private void Reset()
    {
        pickupActivated = false;
        dissolveActivated = false;
        fireLookActivated = false;
    }

    private void ItemInfoAppear()
    {
        Reset();
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득" + "<color=yellow>" + "(E)" + "</color>";

    }

    private void MeatInfoAppear()
    {
        if (hitInfo.transform.GetComponent<Animal>().isDead)
        {
            Reset();
            dissolveActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = hitInfo.transform.GetComponent<Animal>().animalName + " 해체하기" + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    private void FireInfoAppear()
    {
        Reset();
        fireLookActivated = true;

        if (hitInfo.transform.GetComponent<Fire>().GetIsFire())
        {
            actionText.gameObject.SetActive(true);
            actionText.text = "선택된 아이템 불에 넣기" + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        dissolveActivated = false;
        fireLookActivated = false;
        actionText.gameObject.SetActive(false);
    }


}
