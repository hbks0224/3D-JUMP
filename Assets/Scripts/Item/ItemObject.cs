using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInteractable
{

    public string GetInteractPrompt();

    public void OnInteract();

}





public class ItemObject : MonoBehaviour , IInteractable
{
    public ItemData Data;

    public string GetInteractPrompt()
    {
        string str = $"{Data.displayName}\n{Data.description}";
        return str ;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = Data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
