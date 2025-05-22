using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIInventory : MonoBehaviour
{

    public ItemSlot[] slots;
    public Transform dropPosition;      // item ���� �� �ʿ��� ��ġ
    public Transform BG;
    public Transform InfoBg;
    [Header("Selected Item")]           // ������ ������ ������ ���� ǥ�� ���� UI
    private ItemSlot selectedItem;
    private int selectedItemIndex = 0;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    private PlayerAction controller;
    private PlayerCondition condition;

    private int curwheelSlotIndex =01;
    void Start()
    {

        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;


        CharacterManager.Instance.Player.addItem += AddItem;  // ������ �Ĺ� ��


        slots = new ItemSlot[BG.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = BG.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        if(slots.Length > 0)
        {

            curwheelSlotIndex = 0;
            SelectItem(curwheelSlotIndex);
        }

        else
        {

            ClearSelectedItemWindow();
        }

            



    }

    private void Update()
    {
        MouseWheelSelect();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("������ ����");
            ThrowItem(selectedItem.item);
            
            RemoveSelctedItem();

        }
        if (selectedItem != null && Input.GetMouseButton(1))
        {
            Debug.Log("������ ���");
            ItemUse();
        }

    }

    void MouseWheelSelect()
    {
        if(slots.Length == 0)
        {
            return;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f){

            int prewheelSlotIndex = curwheelSlotIndex;

            if (scroll > 0f)
            {
                curwheelSlotIndex--;
                if (curwheelSlotIndex < 0)
                {

                    curwheelSlotIndex = slots.Length - 1;
                }

            }
            else if (scroll < 0f)
            {

                curwheelSlotIndex++;
                    if(curwheelSlotIndex >= slots.Length)
                {
                    curwheelSlotIndex = 0;
                }
            }

            if(prewheelSlotIndex != curwheelSlotIndex)
            {

                SelectItem(curwheelSlotIndex);
            }

        }

    }

    // ������ ������ ǥ���� ����â Clear �Լ�
    void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;

    }

    public void AddItem()
    {
        // 10�� ItemObject �������� Player�� �Ѱ��� ������ ������ ��
        ItemData data = CharacterManager.Instance.Player.itemData;

        // ������ ���� �� �ִ� �������̶��
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        // �� ���� ã��
        ItemSlot emptySlot = GetEmptySlot();

        // �� ������ �ִٸ�
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        // �� ���� ���� ���� ��
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // ���Կ� ������ ������ �ִٸ�
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    // ������ ���� �� �ִ� �������� ���� ã�Ƽ� return
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void ThrowItem(ItemData data)
    {
        GameObject droppedItem = Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
        droppedItem.layer = LayerMask.NameToLayer("Interactable");
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

    }

    void RemoveSelctedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0)
        {


            selectedItem.item = null;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }
    public void ItemUse()
    {
        if (selectedItem.item.type == ItemType.usable)
        {
            for (int i = 0; i < selectedItem.item.usables.Length; i++)
            {
                switch (selectedItem.item.usables[i].type)
                {
                    case UseableType.Health:
                        condition.Heal(selectedItem.item.usables[i].value);
                        break;

                    case UseableType.Speed:
                        condition.SpeedUp(selectedItem.item.usables[i].value , 10f);
                        break;


                }

            }
            RemoveSelctedItem(); 


        }

    }

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }

}


