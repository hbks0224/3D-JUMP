using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIInventory : MonoBehaviour
{
    // 인벤토리를 구성하는 모든 ItemSlot들의 배열
    public ItemSlot[] slots;
    // item 버릴 때 필요한 위치
    public Transform dropPosition;
    
    public Transform BG; 

    public Transform InfoBg;

    [Header("Selected Item")]
    private ItemSlot selectedItem; // 현재 선택된 아이템 
    private int selectedItemIndex = 0; // 현재 선택된 아이템 슬롯의 인덱스
    public TextMeshProUGUI selectedItemName; 
    public TextMeshProUGUI selectedItemDescription; 

    
    private PlayerAction controller;
    private PlayerCondition condition;

    // 마우스 휠로 현재 선택된 슬롯의 인덱스를 저장
    private int curwheelSlotIndex = 0;

    void Start()
    {
        
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // 플레이어의 addItem 이벤트에 이 UIInventory의 AddItem 메서드를 구독
   
        CharacterManager.Instance.Player.addItem += AddItem;

        
        slots = new ItemSlot[BG.childCount];

        // BG의 각 자식 게임 오브젝트에서 ItemSlot 컴포넌트를 찾아 배열에 할당하고 초기화
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = BG.GetChild(i).GetComponent<ItemSlot>(); // 자식으로부터 ItemSlot 컴포넌트 가져오기
            slots[i].index = i; // 슬롯의 인덱스 설정
            slots[i].inventory = this; // 슬롯이 현재 UIInventory를 참조하도록 설정
            slots[i].Clear(); // 슬롯 초기화 
        }

        // 인벤토리 슬롯이 하나 이상 존재하면
        if (slots.Length > 0)
        {
            curwheelSlotIndex = 0; // 첫 번째 슬롯을 기본 선택으로 설정
            SelectItem(curwheelSlotIndex); // 첫 번째 슬롯 선택 처리
        }
        else // 슬롯이 없으면
        {
            ClearSelectedItemWindow(); // 선택된 아이템 정보창을 비움
        }
    }

    private void Update()
    {
        // 마우스 휠 입력을 받아 아이템 슬롯을 선택
        MouseWheelSelect();


        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("아이템 버림");
           
            if (selectedItem != null && selectedItem.item != null)
            {
                ThrowItem(selectedItem.item); // 선택된 아이템을 드랍
                RemoveSelctedItem(); // 인벤토리에서 해당 아이템(1개) 제거 
            }
        }

        
        if (selectedItem != null && selectedItem.item != null && Input.GetMouseButton(1)) 
        {
            Debug.Log("아이템 사용");
            ItemUse(); // 아이템 사용 로직
        }
    }

    // 마우스 휠 스크롤로 인벤토리 슬롯을 선택하는 메서드
    void MouseWheelSelect()
    {
        
        if (slots.Length == 0)
        {
            return;
        }

        // 마우스 휠 스크롤 값을 가져옴 (위로 스크롤 시 양수, 아래로 스크롤 시 음수)
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        
        if (scroll != 0f)
        {
            int prewheelSlotIndex = curwheelSlotIndex; // 이전 선택 인덱스 저장

            if (scroll > 0f) // 위로 스크롤 
            {
                curwheelSlotIndex--;
                if (curwheelSlotIndex < 0) // 인덱스가 0보다 작아지면 마지막 슬롯으로 순환
                {
                    curwheelSlotIndex = slots.Length - 1;
                }
            }
            else if (scroll < 0f) // 아래로 스크롤 
            {
                curwheelSlotIndex++;
                if (curwheelSlotIndex >= slots.Length) // 인덱스가 슬롯 개수를 넘어가면 첫 번째 슬롯으로 순환
                {
                    curwheelSlotIndex = 0;
                }
            }

            // 선택된 슬롯 인덱스가 변경되었다면
            if (prewheelSlotIndex != curwheelSlotIndex)
            {
                SelectItem(curwheelSlotIndex); // 새 인덱스로 아이템 선택 처리
            }
        }
    }

    // 선택한 아이템의 정보를 표시하는 UI 창을 초기화하는 함수입니다.
    void ClearSelectedItemWindow()
    {
        selectedItem = null; // 선택된 아이템 슬롯 참조 해제
        selectedItemName.text = string.Empty; // 아이템 이름 텍스트 비우기
        selectedItemDescription.text = string.Empty; // 아이템 설명 텍스트 비우기
    }

    // 인벤토리에 아이템을 추가하는 메서드
    public void AddItem()
    {
        // 사용자 주석: 10강 ItemObject 로직에서 Player에 넘겨준 정보를 가지고 옴
        // CharacterManager를 통해 현재 플레이어가 획득하려고 하는 아이템 데이터를 가져옴
        ItemData data = CharacterManager.Instance.Player.itemData;
        if (data == null) return; // 추가할 아이템 데이터가 없으면 중단

        // 아이템이 중첩될 수 있는 경우
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

           
            Debug.Log("인벤토리가 가득 차 아이템을 버립니다: " + data.displayName);
            ThrowItem(data); 
            CharacterManager.Instance.Player.itemData = null; 
        }
    }

    // 인벤토리의 모든 슬롯 UI를 현재 상태에 맞게 갱신
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
        if (data == null || data.dropPrefab == null) return; // 데이터나 드랍 프리팹이 없으면 실행 안함

        GameObject droppedItem = Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
        // 생성된 아이템 오브젝트의 레이어를 "Interactable"로 설정하여, 플레이어가 다시 주울 수 있게 설정
        droppedItem.layer = LayerMask.NameToLayer("Interactable");
    }

   
    public void SelectItem(int index)
    {

        if (index < 0 || index >= slots.Length || slots[index].item == null)
        {

            return;
        }

        selectedItem = slots[index]; // 현재 선택된 슬롯으로 설정
        selectedItemIndex = index; // 현재 선택된 인덱스로 설정

        // 선택된 아이템 정보창에 아이템 이름과 설명을 표시
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;
    }


    void RemoveSelctedItem()
    {
        if (selectedItem == null || selectedItem.item == null) return; 

        selectedItem.quantity--;

        if (selectedItem.quantity <= 0) // 아이템 개수가 0 이하가 되면
        {
            selectedItem.item = null; // 슬롯에서 아이템 데이터를 제거 (슬롯을 비움)
            ClearSelectedItemWindow(); // 선택된 아이템 정보창을 비웁니다.
        }
        UpdateUI(); 
    }


    public void ItemUse()
    {
        if (selectedItem == null || selectedItem.item == null) return; // 선택된 아이템이 없으면 중단


        if (selectedItem.item.type == ItemType.usable)
        {

            for (int i = 0; i < selectedItem.item.usables.Length; i++)
            {
                ItemDataUsable usableEffect = selectedItem.item.usables[i];
                switch (usableEffect.type) // 각 효과의 타입에 따라 다른 처리
                {
                    case UseableType.Health: // 체력 회복/변경 효과일 경우
                        condition.Heal(usableEffect.value); 
                        break;
                    case UseableType.Speed: // 속도 변경 효과일 경우
                                            
                        condition.SpeedUp(usableEffect.value, 5f);
                        break;
                  
                }
            }
            RemoveSelctedItem(); 
        }
    }


    public bool HasItem(ItemData item, int quantity)
    {
        // TODO: 실제 인벤토리를 순회하며 아이템 존재 여부와 개수를 확인하는 로직 구현 필요
        int count = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                count += slots[i].quantity;
            }
        }
        return count >= quantity;
        // return false; // 현재는 항상 false 반환
    }
}