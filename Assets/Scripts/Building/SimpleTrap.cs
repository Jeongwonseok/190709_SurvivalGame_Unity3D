﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{

    private Rigidbody[] rigid;
    [SerializeField] private GameObject go_Meat;

    [SerializeField] private int damage;

    private bool isActivated = false;

    private AudioSource theAudio;
    [SerializeField] private AudioClip sound_Activate;

    private StatusController theStatusController;

    // Start is called before the first frame update
    void Start()
    {
        theStatusController = FindObjectOfType<StatusController>();
        rigid = GetComponentsInChildren<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isActivated)
        {
            if(other.transform.tag != "Untagged")
            {
                isActivated = true;
                theAudio.clip = sound_Activate;
                theAudio.Play();

                Destroy(go_Meat); // 고기 제거

                for (int i = 0; i < rigid.Length; i++)
                {
                    rigid[i].useGravity = true;
                    rigid[i].isKinematic = false; // 고정한거 취소
                }

                if(other.transform.name == "Player")
                {
                    theStatusController.DecreaseHP(damage);
                }
            }
        }
    }

}
