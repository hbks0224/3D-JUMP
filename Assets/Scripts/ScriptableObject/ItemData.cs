using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템의 종류 열거형
public enum ItemType
{
    usable //소모품
          
}

// 소모품의 타입 열거형
public enum UseableType 
{
    Health, // 체력을 회복시키는 타입
    Speed   // 이동 속도를 변화시키는 타입
           
}


[Serializable]
public class ItemDataUsable
{
    public UseableType type; // 이 효과가 어떤 타입인가?
    public float value;      // 효과의 수치
}


[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] 

    public string displayName;  // 아이템의 표시 이름 
    public string description;  // 아이템에 대한 설명 
  
    public Sprite icon;         // 아이템의 UI 아이콘 이미지
    public ItemType type;       // 이 아이템의 타입
    public GameObject dropPrefab; // 아이템을 떨어뜨렸을 때 생성될 오브젝트 프리팹

    [Header("Stacking")] 

    public bool canStack;         // 아이템을 여러 개 가질 수 있는지 불값
    public int maxStackAmount;    // 가질 수 있는 최대 개수

    [Header("Usable")] 
    public ItemDataUsable[] usables;
}