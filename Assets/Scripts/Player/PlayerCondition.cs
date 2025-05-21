using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    Condition health { get { return uiCondition.health; } }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        health.Subtrack(health.passveValue * Time.deltaTime);

        if (health.curValue == 0)
        {
            Debug.Log("»ç¸Á");

        }

    }


}
