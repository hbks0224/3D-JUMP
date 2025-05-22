using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    usable


}
public enum UseableType
{
    Health,
    Speed


}
[Serializable]
public class ItemDataUsable 
{

    public UseableType type;
    public float value;

}



[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("info")]

    public string displayName;
    public string description;
    //사용 아이템만 구현 예정
    public Sprite icon;
    public ItemType type;
    public GameObject dropPrefab;

    [Header("Stacking")]

    public bool canStack;
    public int maxStackAmount;

    [Header("Usable")]

    public ItemDataUsable[] usables;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
