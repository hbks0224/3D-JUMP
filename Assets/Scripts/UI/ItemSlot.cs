using TMPro; 
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;  

public class ItemSlot : MonoBehaviour
{
    // 이 슬롯이 현재 담고 있는 아이템의 데이터
    public ItemData item;


    public UIInventory inventory;

    
    public Image icon;
    // 아이템의 개수를 표시할 텍스트
    public TextMeshProUGUI quatityText;

    private Outline outline;

    // 인벤토리 슬롯의 인덱스
    public int index;

    // 슬롯에 담겨 있는 아이템의 개수
    public int quantity;


    private void Awake()
    {

        outline = GetComponent<Outline>();
    }

    // 슬롯에 아이템 정보를 설정하고 UI를 업데이트하는 메서드
    public void Set()
    {
        // 아이콘 Image 컴포넌트를 활성화하여 보이도록 합니다.
        icon.gameObject.SetActive(true);
        // 현재 슬롯의 아이템(item) 데이터에서 아이콘 스프라이트를 가져와 Image 컴포넌트에 설정
        icon.sprite = item.icon;
        // 아이템 개수(quantity)가 1보다 크면 개수를 텍스트로 표시하고, 그렇지 않으면 빈 문자열을 표시

        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty; 
    }

    // 슬롯을 비우고 UI를 초기 상태로 되돌리는 메서드
    public void Clear()
    {
        
        item = null;
        
        icon.gameObject.SetActive(false);

        quatityText.text = string.Empty; 
    }


}