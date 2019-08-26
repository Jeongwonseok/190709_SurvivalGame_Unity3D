using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 활성화 여부 >> true 이면 인벤토리 활성화하고 카메라, 마우스 등등 다 꺼버리기위해
    public static bool inventoryActivated = false;

    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;
    [SerializeField]
    private GameObject go_ToolTip;
    [SerializeField]
    private GameObject go_QuickSlotParent;
    [SerializeField]
    private QuickSlotController theQuickSlot;

    private Slot[] slots; // 인벤토리 슬롯들
    private Slot[] quickslots; // 퀵슬롯들
    private bool isNotPut;
    private int slotNumber;

    // 인벤토리 슬롯들 반환
    public Slot[] GetSlots() { return slots; }

    [SerializeField] private Item[] items; // item 임시로 담아두는 배열

    // 아이템 인벤토리 로드 메서드
    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
            if (items[i].itemName == _itemName)
                slots[_arrayNum].AddItem(items[i], _itemNum);
    }

    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        quickslots = go_QuickSlotParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    private void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        go_InventoryBase.SetActive(false);
        go_ToolTip.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count=1)
    {
        PutSlot(quickslots, _item, _count);
        if (!isNotPut)
            theQuickSlot.IsActivatedQuickSlot(slotNumber);

        if (isNotPut)
            PutSlot(slots, _item, _count);

        if (isNotPut)
            Debug.Log("퀵슬롯과 인벤토리가 꽉 찼습니다.");
    }

    private void PutSlot(Slot[] _slots, Item _item, int _count)
    {
        if (Item.ItemType.Equipment != _item.itemType && Item.ItemType.Kit != _item.itemType)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item != null)
                {
                    if (_slots[i].item.itemName == _item.itemName)
                    {
                        slotNumber = i;
                        _slots[i].SetSlotCount(_count);
                        isNotPut = false;
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item == null)
            {
                _slots[i].AddItem(_item, _count);
                isNotPut = false;
                return;
            }
        }

        isNotPut = true;
    }

    public int GetItemCount(string _itemName)
    {
        int temp = SearchSlotItem(slots, _itemName);

        return temp != 0 ? temp : SearchSlotItem(quickslots, _itemName);
    }

    private int SearchSlotItem(Slot[] _slots, string _itemName)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if(_slots[i].item != null)
            {
                if (_itemName == _slots[i].item.itemName)
                    return _slots[i].itemCount;
            }
        }

        return 0;

    }

    public void SetItemCount(string _itemName, int _itemCount)
    {
        if (!ItemCountAdjust(slots, _itemName, _itemCount))
            ItemCountAdjust(quickslots, _itemName, _itemCount);
    }

    private bool ItemCountAdjust(Slot[] _slots, string _itemName, int _itemCount)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if(_slots[i].item != null)
            {
                if (_itemName == _slots[i].item.itemName)
                {
                    _slots[i].SetSlotCount(-_itemCount);
                    return true;
                }
            }
        }

        return false;
    }

}
