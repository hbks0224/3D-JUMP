using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIInventory : MonoBehaviour
{
    // �κ��丮�� �����ϴ� ��� ItemSlot���� �迭
    public ItemSlot[] slots;
    // item ���� �� �ʿ��� ��ġ
    public Transform dropPosition;
    
    public Transform BG; 

    public Transform InfoBg;

    [Header("Selected Item")]
    private ItemSlot selectedItem; // ���� ���õ� ������ 
    private int selectedItemIndex = 0; // ���� ���õ� ������ ������ �ε���
    public TextMeshProUGUI selectedItemName; 
    public TextMeshProUGUI selectedItemDescription; 

    
    private PlayerAction controller;
    private PlayerCondition condition;

    // ���콺 �ٷ� ���� ���õ� ������ �ε����� ����
    private int curwheelSlotIndex = 0;

    void Start()
    {
        
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // �÷��̾��� addItem �̺�Ʈ�� �� UIInventory�� AddItem �޼��带 ����
   
        CharacterManager.Instance.Player.addItem += AddItem;

        
        slots = new ItemSlot[BG.childCount];

        // BG�� �� �ڽ� ���� ������Ʈ���� ItemSlot ������Ʈ�� ã�� �迭�� �Ҵ��ϰ� �ʱ�ȭ
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = BG.GetChild(i).GetComponent<ItemSlot>(); // �ڽ����κ��� ItemSlot ������Ʈ ��������
            slots[i].index = i; // ������ �ε��� ����
            slots[i].inventory = this; // ������ ���� UIInventory�� �����ϵ��� ����
            slots[i].Clear(); // ���� �ʱ�ȭ 
        }

        // �κ��丮 ������ �ϳ� �̻� �����ϸ�
        if (slots.Length > 0)
        {
            curwheelSlotIndex = 0; // ù ��° ������ �⺻ �������� ����
            SelectItem(curwheelSlotIndex); // ù ��° ���� ���� ó��
        }
        else // ������ ������
        {
            ClearSelectedItemWindow(); // ���õ� ������ ����â�� ���
        }
    }

    private void Update()
    {
        // ���콺 �� �Է��� �޾� ������ ������ ����
        MouseWheelSelect();


        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("������ ����");
           
            if (selectedItem != null && selectedItem.item != null)
            {
                ThrowItem(selectedItem.item); // ���õ� �������� ���
                RemoveSelctedItem(); // �κ��丮���� �ش� ������(1��) ���� 
            }
        }

        
        if (selectedItem != null && selectedItem.item != null && Input.GetMouseButton(1)) 
        {
            Debug.Log("������ ���");
            ItemUse(); // ������ ��� ����
        }
    }

    // ���콺 �� ��ũ�ѷ� �κ��丮 ������ �����ϴ� �޼���
    void MouseWheelSelect()
    {
        
        if (slots.Length == 0)
        {
            return;
        }

        // ���콺 �� ��ũ�� ���� ������ (���� ��ũ�� �� ���, �Ʒ��� ��ũ�� �� ����)
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        
        if (scroll != 0f)
        {
            int prewheelSlotIndex = curwheelSlotIndex; // ���� ���� �ε��� ����

            if (scroll > 0f) // ���� ��ũ�� 
            {
                curwheelSlotIndex--;
                if (curwheelSlotIndex < 0) // �ε����� 0���� �۾����� ������ �������� ��ȯ
                {
                    curwheelSlotIndex = slots.Length - 1;
                }
            }
            else if (scroll < 0f) // �Ʒ��� ��ũ�� 
            {
                curwheelSlotIndex++;
                if (curwheelSlotIndex >= slots.Length) // �ε����� ���� ������ �Ѿ�� ù ��° �������� ��ȯ
                {
                    curwheelSlotIndex = 0;
                }
            }

            // ���õ� ���� �ε����� ����Ǿ��ٸ�
            if (prewheelSlotIndex != curwheelSlotIndex)
            {
                SelectItem(curwheelSlotIndex); // �� �ε����� ������ ���� ó��
            }
        }
    }

    // ������ �������� ������ ǥ���ϴ� UI â�� �ʱ�ȭ�ϴ� �Լ��Դϴ�.
    void ClearSelectedItemWindow()
    {
        selectedItem = null; // ���õ� ������ ���� ���� ����
        selectedItemName.text = string.Empty; // ������ �̸� �ؽ�Ʈ ����
        selectedItemDescription.text = string.Empty; // ������ ���� �ؽ�Ʈ ����
    }

    // �κ��丮�� �������� �߰��ϴ� �޼���
    public void AddItem()
    {
        // ����� �ּ�: 10�� ItemObject �������� Player�� �Ѱ��� ������ ������ ��
        // CharacterManager�� ���� ���� �÷��̾ ȹ���Ϸ��� �ϴ� ������ �����͸� ������
        ItemData data = CharacterManager.Instance.Player.itemData;
        if (data == null) return; // �߰��� ������ �����Ͱ� ������ �ߴ�

        // �������� ��ø�� �� �ִ� ���
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

            ItemSlot emptySlot = GetEmptySlot();

            if (emptySlot != null) 
            {
                emptySlot.item = data; 
                emptySlot.quantity = 1; 
                UpdateUI(); 
                CharacterManager.Instance.Player.itemData = null; 
                return; 
            }

           
            Debug.Log("�κ��丮�� ���� �� �������� �����ϴ�: " + data.displayName);
            ThrowItem(data); 
            CharacterManager.Instance.Player.itemData = null; 
        }
    }

    // �κ��丮�� ��� ���� UI�� ���� ���¿� �°� ����
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
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
        if (data == null || data.dropPrefab == null) return; // �����ͳ� ��� �������� ������ ���� ����

        GameObject droppedItem = Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
        // ������ ������ ������Ʈ�� ���̾ "Interactable"�� �����Ͽ�, �÷��̾ �ٽ� �ֿ� �� �ְ� ����
        droppedItem.layer = LayerMask.NameToLayer("Interactable");
    }

   
    public void SelectItem(int index)
    {

        if (index < 0 || index >= slots.Length || slots[index].item == null)
        {

            return;
        }

        selectedItem = slots[index]; // ���� ���õ� �������� ����
        selectedItemIndex = index; // ���� ���õ� �ε����� ����

        // ���õ� ������ ����â�� ������ �̸��� ������ ǥ��
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;
    }


    void RemoveSelctedItem()
    {
        if (selectedItem == null || selectedItem.item == null) return; 

        selectedItem.quantity--;

        if (selectedItem.quantity <= 0) // ������ ������ 0 ���ϰ� �Ǹ�
        {
            selectedItem.item = null; // ���Կ��� ������ �����͸� ���� (������ ���)
            ClearSelectedItemWindow(); // ���õ� ������ ����â�� ���ϴ�.
        }
        UpdateUI(); 
    }


    public void ItemUse()
    {
        if (selectedItem == null || selectedItem.item == null) return; // ���õ� �������� ������ �ߴ�


        if (selectedItem.item.type == ItemType.usable)
        {

            for (int i = 0; i < selectedItem.item.usables.Length; i++)
            {
                ItemDataUsable usableEffect = selectedItem.item.usables[i];
                switch (usableEffect.type) // �� ȿ���� Ÿ�Կ� ���� �ٸ� ó��
                {
                    case UseableType.Health: // ü�� ȸ��/���� ȿ���� ���
                        condition.Heal(usableEffect.value); 
                        break;
                    case UseableType.Speed: // �ӵ� ���� ȿ���� ���
                                            
                        condition.SpeedUp(usableEffect.value, 5f);
                        break;
                  
                }
            }
            RemoveSelctedItem(); 
        }
    }


    public bool HasItem(ItemData item, int quantity)
    {
        // TODO: ���� �κ��丮�� ��ȸ�ϸ� ������ ���� ���ο� ������ Ȯ���ϴ� ���� ���� �ʿ�
        int count = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                count += slots[i].quantity;
            }
        }
        return count >= quantity;
        // return false; // ����� �׻� false ��ȯ
    }
}