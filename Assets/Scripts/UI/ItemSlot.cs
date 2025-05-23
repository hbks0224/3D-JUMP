using TMPro; 
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;  

public class ItemSlot : MonoBehaviour
{
    // �� ������ ���� ��� �ִ� �������� ������
    public ItemData item;


    public UIInventory inventory;

    
    public Image icon;
    // �������� ������ ǥ���� �ؽ�Ʈ
    public TextMeshProUGUI quatityText;

    private Outline outline;

    // �κ��丮 ������ �ε���
    public int index;

    // ���Կ� ��� �ִ� �������� ����
    public int quantity;


    private void Awake()
    {

        outline = GetComponent<Outline>();
    }

    // ���Կ� ������ ������ �����ϰ� UI�� ������Ʈ�ϴ� �޼���
    public void Set()
    {
        // ������ Image ������Ʈ�� Ȱ��ȭ�Ͽ� ���̵��� �մϴ�.
        icon.gameObject.SetActive(true);
        // ���� ������ ������(item) �����Ϳ��� ������ ��������Ʈ�� ������ Image ������Ʈ�� ����
        icon.sprite = item.icon;
        // ������ ����(quantity)�� 1���� ũ�� ������ �ؽ�Ʈ�� ǥ���ϰ�, �׷��� ������ �� ���ڿ��� ǥ��

        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty; 
    }

    // ������ ���� UI�� �ʱ� ���·� �ǵ����� �޼���
    public void Clear()
    {
        
        item = null;
        
        icon.gameObject.SetActive(false);

        quatityText.text = string.Empty; 
    }


}