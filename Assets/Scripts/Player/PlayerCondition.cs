using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    public PlayerAction playerAction;
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

    public void Heal(float amount)
    {

        health.Add(amount);

    }
    public void SpeedUp(float amount)
    {
        playerAction.speed += amount;
    }


}
