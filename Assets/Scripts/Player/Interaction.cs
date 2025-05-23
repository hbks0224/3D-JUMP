using System.Collections; 
using System.Collections.Generic;
using TMPro; 
using UnityEngine; 
using UnityEngine.InputSystem; 

public class Interaction : MonoBehaviour 
{
    
    public float checkRate = 0.05f; // ��ȣ�ۿ� ������ ��ü�� �󸶳� ���� Ȯ������ �ð� ����
    private float lastCheckTIme; // ���������� Ȯ���� �ð��� ����
    public float maxCheckDistance; // ��ȣ�ۿ� ������ ��ü�� Ž���� �ִ� �Ÿ�
    public LayerMask layerMask; //  �浹�� ������ Ư�� ���̾���� ����

    private GameObject curInteractGameObject; // ���� �÷��̾ �ٶ󺸰� �ִ� ��ȣ�ۿ� ������ ���� ������Ʈ�� ����
    private IInteractable curInteractable; // ���� ��ȣ�ۿ� ������ ��ü�� IInteractable �������̽� ������ ����


    public TextMeshProUGUI promptText; 
    private Camera _camera; 

    void Start()
    {

        _camera = Camera.main; 

    }

    void Update()
    {
        
        
        if (Time.time - lastCheckTIme > checkRate)
        {
            lastCheckTIme = Time.time; // ������ Ȯ�� �ð��� ���� �ð����� ������Ʈ

            // ī�޶� ȭ�� �߾ӿ��� �������� ���̸� ����
            // Screen.width / 2, Screen.height / 2�� ȭ���� ���߾� 
            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit; // ����ĳ��Ʈ�� �浹 ������ ������ ����

            // ray: ��� ����
            // out hit: �浹 ������ ���⿡ �����
            // maxCheckDistance: �ִ� Ž�� �Ÿ�
            // layerMask: ������ ���̾ �ִ� ��ü�ϰ� �浹�� Ȯ��
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // ����ĳ��Ʈ�� ���� �����Ǿ��ٸ�
                // ������ ������ ��ȣ�ۿ� ��ü�� ���� ������ ��ü�� �ٸ� ��쿡�� ������ ó��
                // (���� ��ü�� ��� �ٶ󺸰� ���� �� ���ʿ��� ������Ʈ�� ����)
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject; // ���� ��ȣ�ۿ� ������ ���� ������Ʈ�� ������Ʈ
                    curInteractable = hit.collider.GetComponent<IInteractable>(); // ������ ������Ʈ���� IInteractable �������̽��� ã�� �Ҵ�
                    SetPromptText(); 
                }
            }
            else // ����ĳ��Ʈ�� �ƹ��͵� �������� �ʾҴٸ�
            {
                curInteractGameObject = null; // ���� ��ȣ�ۿ� ������ ���� ������Ʈ�� null�� ����
                curInteractable = null; // ���� ��ȣ�ۿ� ������ �������̽��� null�� ����
                promptText.gameObject.SetActive(false); 
            }
        }
    }

    
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true); 
        // ���� ��ȣ�ۿ� ������ ��ü(curInteractable)�� null�� �ƴϰ�, IInteractable �������̽��� ������ �ִٸ�
        // GetInteractPrompt() �޼��带 ȣ���Ͽ� ǥ���� �ؽ�Ʈ�� ������ UI�� ����.

        if (curInteractable != null) 
        {
            promptText.text = curInteractable.GetInteractPrompt();
        }
        else 
        {
            promptText.gameObject.SetActive(false); 
        }
    }

    // Unity Input System�� ���� ȣ��Ǵ� ��ȣ�ۿ� �Է� ó�� �޼���

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        //  Ű�� ���� ���� AND ���� ��ȣ�ۿ� ������ ��ü�� ������

        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract(); // ���� ��ȣ�ۿ� ������ ��ü�� OnInteract() ����
            // ��ȣ�ۿ� �Ŀ��� �Ϲ������� �ش� ��ü���� ��ȣ�ۿ� ���¸� �ʱ�ȭ
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false); 
        }
    }
}