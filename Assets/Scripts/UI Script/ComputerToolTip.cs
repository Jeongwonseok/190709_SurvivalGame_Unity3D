using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerToolTip : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI;

    [SerializeField] private Text kitName;
    [SerializeField] private Text kitDesc;
    [SerializeField] private Text kitNeedItem;


    public void ShowToolTip(string _kitName, string _kitDesc, string[] _needItem, int[] _needItemNumber)
    {
        go_BaseUI.SetActive(true);

        kitName.text = _kitName;
        kitDesc.text = _kitDesc;
        for (int i = 0; i < _needItem.Length; i++)
        {
            kitNeedItem.text += _needItem[i];
            kitNeedItem.text += " x " + _needItemNumber[i].ToString() + "\n";
        }
    }

    public void HideToolTip()
    {
        go_BaseUI.SetActive(false);
        kitName.text = "";
        kitDesc.text = "";
        kitNeedItem.text = "";
    }
}
