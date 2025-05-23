using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    public PlayerAction playerAction;
    Coroutine coroutine;
    float originSpeed; //�ӵ� ����� ����
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
            

        }

    }

    public void Heal(float amount)
    {

        health.Add(amount);

    }
    public void SpeedUp(float amount, float  duration)
    {
        if (coroutine != null)
        {

            StopCoroutine(coroutine);
            

        }

        coroutine = StartCoroutine(SpeedUpCoroutine(amount, duration));
    }

    IEnumerator SpeedUpCoroutine(float amout, float duration)
    {

        originSpeed = playerAction.speed;
        playerAction.speed += amout;
        Debug.Log("�ӵ�����");
        float curTime = duration;

        while (curTime > 0)
        {

            curTime -=Time.deltaTime;
            yield return null;

        }


        Debug.Log("�ӵ�����");

        playerAction.speed = originSpeed;
        coroutine = null;



    }


}
