using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Condition : MonoBehaviour
{
    public float curValue;

    public float startValue;

    public float maxValue;
    public float passveValue;
    public Image uiBar;
    void Start()
    {
        curValue = startValue; 
    }

    // Update is called once per frame
    void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    float GetPercentage()
    {
        return curValue / maxValue;


    }

    public void Add(float value)
    {
        
        curValue = Mathf.Min(curValue+ value,maxValue);

    }

    public void Subtrack(float value)
    {

        curValue = Mathf.Max(curValue - value, 0); ;
    }
}
