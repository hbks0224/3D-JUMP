using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
