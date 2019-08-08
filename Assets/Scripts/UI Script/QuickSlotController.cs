using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{

    [SerializeField] private Slot[] quickSlots;  // 퀵슬롯들
    [SerializeField] private Transform tf_parent;  // 퀵슬롯의 부모 객체

    private int selectedSlot; // 선택된 퀵슬롯(0~7)

    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_SelectedImage; // 선택된 퀵슬롯의 이미지
    [SerializeField]
    private WeaponManager theWeaponManager;


    // Start is called before the first frame update
    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();
        selectedSlot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TryInputNumber();
    }

    private void TryInputNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeSlot(0);
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            ChangeSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            ChangeSlot(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            ChangeSlot(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            ChangeSlot(6);
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            ChangeSlot(7);
    }

    public void IsActivatedQuickSlot(int _num)
    {
        if(selectedSlot == _num)
        {
            Excute();
            return;
        }
        if(DragSlot.instance != null)
        {
            if(DragSlot.instance.dragSlot != null)
            {
                if (DragSlot.instance.dragSlot.GetQuickSlotNumber() == selectedSlot)
                {
                    Excute();
                    return;
                }
            }
        }
    }

    private void ChangeSlot(int _num)
    {
        SelectedSlot(_num);

        Excute();
    }

    private void SelectedSlot(int _num)
    {
        // 선택된 슬롯
        selectedSlot = _num;

        // 선택된 슬롯으로 이미지 이동
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;
    }

    private void Excute()
    {
        if(quickSlots[selectedSlot].item != null)
        {
            if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Equipment)
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));
            else if(quickSlots[selectedSlot].item.itemType == Item.ItemType.Used)
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "맨손"));
            else
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "맨손"));
        }
        else
        {
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "맨손"));
        }
    }
}
