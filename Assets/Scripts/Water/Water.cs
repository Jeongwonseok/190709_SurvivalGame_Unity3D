using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{

    [SerializeField] private float waterDrag; // 물속 중력
    private float originDrag;

    [SerializeField] private Color waterColor; // 물속 색깔
    [SerializeField] private float waterFogDensity; // 물속 탁함 정도

    [SerializeField] private Color waterNightColor; // 밤 상태의 물속 색깔
    [SerializeField] private float waterNightFogDensity; // 밤 상태의 물속 탁함

    private Color originColor;
    private float originFogDensity;

    [SerializeField] private Color originNightColor;
    [SerializeField] private float originNightForDensity;

    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string sound_Breath;

    [SerializeField] private float breathTime;
    private float currentBreathTime;

    [SerializeField] private float totalOxygen;
    private float currentOxygen;

    private float temp;

    [SerializeField] private GameObject go_BaseUI;
    [SerializeField] private Text text_totalOxygen;
    [SerializeField] private Text text_currentOxygen;
    [SerializeField] private Image image_guage;

    private StatusController thePlayerStat;

    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;

        thePlayerStat = FindObjectOfType<StatusController>();
        currentOxygen = totalOxygen;
        text_totalOxygen.text = totalOxygen.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.isWater)
        {
            currentBreathTime += Time.deltaTime;
            if(currentBreathTime >= breathTime)
            {
                SoundManager.instance.PlaySE(sound_Breath);
                currentBreathTime = 0;
            }

        }

        DecreaseOxygen();
    }

    private void DecreaseOxygen()
    {
        if(GameManager.isWater)
        {
            currentOxygen -= Time.deltaTime;
            text_currentOxygen.text = Mathf.RoundToInt(currentOxygen).ToString();
            image_guage.fillAmount = currentOxygen / totalOxygen;

            if (currentOxygen <=0)
            {
                text_currentOxygen.text = "0";
                // text_currentOxygen.color = Color.red;
                temp += Time.deltaTime;
                if(temp >=1)
                {
                    thePlayerStat.DecreaseHP(1);
                    temp = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    private void GetWater(Collider _player)
    {
        SoundManager.instance.PlaySE(sound_WaterIn);

        go_BaseUI.SetActive(true);
        GameManager.isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;

        if(!GameManager.isNight)
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }

    }

    private void GetOutWater(Collider _player)
    {
        if (GameManager.isWater)
        {
            go_BaseUI.SetActive(false);
            currentOxygen = totalOxygen;
            // text_currentOxygen.color = new Color(24, 241, 38, 255);
            SoundManager.instance.PlaySE(sound_WaterOut);
            GameManager.isWater = false;
            _player.transform.GetComponent<Rigidbody>().drag = originDrag;

            if (!GameManager.isNight)
            {
                RenderSettings.fogColor = originColor;
                RenderSettings.fogDensity = originFogDensity;
            }
            else
            {
                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightForDensity;
            }
        }
    }
}
