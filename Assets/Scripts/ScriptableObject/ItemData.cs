using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� ���� ������
public enum ItemType
{
    usable //�Ҹ�ǰ
          
}

// �Ҹ�ǰ�� Ÿ�� ������
public enum UseableType 
{
    Health, // ü���� ȸ����Ű�� Ÿ��
    Speed   // �̵� �ӵ��� ��ȭ��Ű�� Ÿ��
           
}


[Serializable]
public class ItemDataUsable
{
    public UseableType type; // �� ȿ���� � Ÿ���ΰ�?
    public float value;      // ȿ���� ��ġ
}


[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] 

    public string displayName;  // �������� ǥ�� �̸� 
    public string description;  // �����ۿ� ���� ���� 
  
    public Sprite icon;         // �������� UI ������ �̹���
    public ItemType type;       // �� �������� Ÿ��
    public GameObject dropPrefab; // �������� ����߷��� �� ������ ������Ʈ ������

    [Header("Stacking")] 

    public bool canStack;         // �������� ���� �� ���� �� �ִ��� �Ұ�
    public int maxStackAmount;    // ���� �� �ִ� �ִ� ����

    [Header("Usable")] 
    public ItemDataUsable[] usables;
}