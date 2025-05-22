using System;
using UnityEngine;

// Player�� ���õ� ����� ��Ƶδ� ��.
// �̰��� ���� ��ɿ� ���� ����. (ex.CharacterManager.Instance.Player.controller)
public class Player : MonoBehaviour
{
    public PlayerAction controller;
    public PlayerCondition condition;
    public ItemData itemData;
    public Action addItem;
    public Transform dropPosition;

    private void Awake()
    {
        // �̱���Ŵ����� Player�� ������ �� �ְ� �����͸� �ѱ��.
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerAction>();
        condition = GetComponent<PlayerCondition>();
    }
}