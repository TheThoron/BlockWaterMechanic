using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text cTextPlay;

    private const float fMinSpeadingSpeed = 2f;
    private const float fMaxSpeadingSpeed = 1 / 16f;
    private int nCount;

    public Slider cBlockBar;
    public Slider cSpeedBar;

    private World scWorld;

    public void Scene1()
    {
        nCount = 4;
        Water.SetSpreadingSpeed(fMaxSpeadingSpeed*4f);
        cSpeedBar.value = CalculatePercentSpeed();
        cTextPlay.text = System.Math.Round(1f / Water.GetSpreadingSpeed(), 2) + " step / sec";
    }

    public void Scene2()
    {
        nCount = 5;
        Water.SetSpreadingSpeed(fMaxSpeadingSpeed*2f);
        cSpeedBar.value = CalculatePercentSpeed();
        cTextPlay.text = System.Math.Round(1f / Water.GetSpreadingSpeed(), 2) + " step / sec";
    }

    public void Plus()
    {
        float fSpreadingSpeed = Water.GetSpreadingSpeed();
        if (fSpreadingSpeed > fMaxSpeadingSpeed)
        {
            nCount++;
            Water.SetSpreadingSpeed(fSpreadingSpeed / 2f);
            cTextPlay.text = System.Math.Round(1f / Water.GetSpreadingSpeed(), 2) + " step / sec";
        }
        else
        {
            nCount = 7;
            Water.SetSpreadingSpeed(0f);
            cTextPlay.text = "inf. step / sec";
        }
    }

    public void Minus()
    {
        float fSpreadingSpeed = Water.GetSpreadingSpeed();
        if (fSpreadingSpeed == 0f)
        {
            nCount--;
            Water.SetSpreadingSpeed(fMaxSpeadingSpeed);
        }
        else if (fSpreadingSpeed < fMinSpeadingSpeed)
        {
            nCount--;
            Water.SetSpreadingSpeed(fSpreadingSpeed * 2);
        }
        cTextPlay.text = System.Math.Round(1f / Water.GetSpreadingSpeed(), 2) + " step / sec";
    }


    void Start()
    {
        scWorld = GameObject.Find("World").GetComponent<World>();
        cBlockBar.value = CalculatePercentWater();
    }

    void Update()
    {
        cBlockBar.value = CalculatePercentWater();
        cSpeedBar.value = CalculatePercentSpeed();

        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Plus();
        }
        else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Minus();
        }
    }

    float CalculatePercentWater()
    {
        float fSumWater = scWorld.GetSumWaterInGame();
        float fBlocks = scWorld.GetSumWaterInGame() + scWorld.GetSolidBlocInGame();

        return (fSumWater / fBlocks);
    }

    float CalculatePercentSpeed()
    {
        float fSpreadingSpeed = Water.GetSpreadingSpeed();
        if (fSpreadingSpeed == 0f)
            return 1f;
        return (nCount/11f);
    }
}