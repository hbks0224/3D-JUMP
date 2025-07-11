using System;
using UnityEngine;

// Player와 관련된 기능을 모아두는 곳.
// 이곳을 통해 기능에 각각 접근. (ex.CharacterManager.Instance.Player.controller)
public class Player : MonoBehaviour
{
    public PlayerAction controller;
    public PlayerCondition condition;
    public ItemData itemData;
    public Action addItem;
    public Transform dropPosition; //아이템 떨굴 위치

    private void Awake()
    {
        // 싱글톤매니저에 Player를 참조할 수 있게 데이터를 넘긴다.
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerAction>();
        condition = GetComponent<PlayerCondition>();
    }
}