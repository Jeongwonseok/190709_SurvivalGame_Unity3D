using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{


    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private Text txt_ItemName; // 아이템 이름
    [SerializeField]
    private Text txt_ItemDesc; // 아이템 설명
    [SerializeField]
    private Text txt_ItemHowtoUsed; // 아이템 사용 방법

    // 툴팁 보여주기
    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);
        // 위치 지정 (툴팁 띄우는 위치)
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base.GetComponent<RectTransform>().rect.height, 0f);
        go_Base.transform.position = _pos;

        txt_ItemName.text = _item.itemName;
        txt_ItemDesc.text = _item.itemDesc;

        if (_item.itemType == Item.ItemType.Equipment)
            txt_ItemHowtoUsed.text = "우클릭 - 장착";
        else if (_item.itemType == Item.ItemType.Used)
            txt_ItemHowtoUsed.text = "우클릭 - 먹기";
        else
            txt_ItemHowtoUsed.text = "";

    }

    // 툴팁 숨기기
    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
