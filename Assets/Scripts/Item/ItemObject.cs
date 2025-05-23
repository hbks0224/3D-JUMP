using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInteractable
{

    public string GetInteractPrompt(); //UI에 표시할 정보

    public void OnInteract(); // 인터렉션 호출

}





public class ItemObject : MonoBehaviour , IInteractable //인터페이스 내용은 구현해야함
{
    public ItemData Data; //아이템 정보를 담고있는 변수

    public string GetInteractPrompt() //아이템 이름과 설명을 반환
    {
        string str = $"{Data.displayName}\n{Data.description}";
        return str ;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = Data;
        CharacterManager.Instance.Player.addItem?.Invoke();

        //아이템을 줍는다면 오브젝트 제거
        Destroy(gameObject);
    }
}
