using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInteractable
{

    public string GetInteractPrompt(); //UI�� ǥ���� ����

    public void OnInteract(); // ���ͷ��� ȣ��

}





public class ItemObject : MonoBehaviour , IInteractable //�������̽� ������ �����ؾ���
{
    public ItemData Data; //������ ������ ����ִ� ����

    public string GetInteractPrompt() //������ �̸��� ������ ��ȯ
    {
        string str = $"{Data.displayName}\n{Data.description}";
        return str ;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = Data;
        CharacterManager.Instance.Player.addItem?.Invoke();

        //�������� �ݴ´ٸ� ������Ʈ ����
        Destroy(gameObject);
    }
}
