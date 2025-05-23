using System.Collections; 
using System.Collections.Generic;
using TMPro; 
using UnityEngine; 
using UnityEngine.InputSystem; 

public class Interaction : MonoBehaviour 
{
    
    public float checkRate = 0.05f; // 상호작용 가능한 객체를 얼마나 자주 확인할지 시간 간격
    private float lastCheckTIme; // 마지막으로 확인한 시간을 저장
    public float maxCheckDistance; // 상호작용 가능한 객체를 탐지할 최대 거리
    public LayerMask layerMask; //  충돌을 감지할 특정 레이어들을 지정

    private GameObject curInteractGameObject; // 현재 플레이어가 바라보고 있는 상호작용 가능한 게임 오브젝트를 저장
    private IInteractable curInteractable; // 현재 상호작용 가능한 객체의 IInteractable 인터페이스 참조를 저장


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
            lastCheckTIme = Time.time; // 마지막 확인 시간을 현재 시간으로 업데이트

            // 카메라 화면 중앙에서 앞쪽으로 레이를 생성
            // Screen.width / 2, Screen.height / 2는 화면의 정중앙 
            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit; // 레이캐스트의 충돌 정보를 저장할 변수

            // ray: 쏘는 광선
            // out hit: 충돌 정보가 여기에 저장됨
            // maxCheckDistance: 최대 탐지 거리
            // layerMask: 지정된 레이어에 있는 객체하고만 충돌을 확인
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // 레이캐스트에 무언가 감지되었다면
                // 이전에 감지된 상호작용 객체와 현재 감지된 객체가 다른 경우에만 로직을 처리
                // (같은 객체를 계속 바라보고 있을 때 불필요한 업데이트를 방지)
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject; // 현재 상호작용 가능한 게임 오브젝트를 업데이트
                    curInteractable = hit.collider.GetComponent<IInteractable>(); // 감지된 오브젝트에서 IInteractable 인터페이스를 찾아 할당
                    SetPromptText(); 
                }
            }
            else // 레이캐스트에 아무것도 감지되지 않았다면
            {
                curInteractGameObject = null; // 현재 상호작용 가능한 게임 오브젝트를 null로 설정
                curInteractable = null; // 현재 상호작용 가능한 인터페이스를 null로 설정
                promptText.gameObject.SetActive(false); 
            }
        }
    }

    
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true); 
        // 현재 상호작용 가능한 객체(curInteractable)가 null이 아니고, IInteractable 인터페이스를 가지고 있다면
        // GetInteractPrompt() 메서드를 호출하여 표시할 텍스트를 가져와 UI에 설정.

        if (curInteractable != null) 
        {
            promptText.text = curInteractable.GetInteractPrompt();
        }
        else 
        {
            promptText.gameObject.SetActive(false); 
        }
    }

    // Unity Input System에 의해 호출되는 상호작용 입력 처리 메서드

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        //  키를 누른 순간 AND 현재 상호작용 가능한 객체가 있을때

        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract(); // 현재 상호작용 가능한 객체의 OnInteract() 실행
            // 상호작용 후에는 일반적으로 해당 객체와의 상호작용 상태를 초기화
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false); 
        }
    }
}