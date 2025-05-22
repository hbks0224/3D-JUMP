using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    // 플레이어의 이동 속도를 조절하는 변수
    public float speed = 5f;

    // 현재 방향키 입력 값을 저장하는 변수 (2D 방향 벡터)
    private Vector2 curMovementInput;

    // Rigidbody 컴포넌트를 저장할 변수
    private Rigidbody rb;

    public float jumpPower = 200f;
    
    public LayerMask groundLayerMask;

    public Transform cameraContainer;
    public float minXLook;  // 최소 시야각
    public float maxXLook;  // 최대 시야각
    private float camCurXRot;
    public float lookSensitivity; // 카메라 민감도

    private Vector2 mouseDelta;  // 마우스 변화값

    [HideInInspector]
    public bool canLook = true;






    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //커서 숨기기
        // 현재 GameObject에서 Rigidbody 컴포넌트를 가져와서 저장
        rb = GetComponent<Rigidbody>();
    }

    
    private void FixedUpdate()
    {
        // 플레이어 이동 처리
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("JumpBoat"))
        {
            Debug.Log("점프대");
            Jumptest();

        }
    }
    void Jumptest()
    {
        

        rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);

    }




    void CameraLook()
    {
        // 마우스 움직임의 변화량(mouseDelta)중 y(위 아래)값에 민감도를 곱한다.
        // 카메라가 위 아래로 회전하려면 rotation의 x 값에 넣어준다. -> 실습으로 확인
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // 마우스 움직임의 변화량(mouseDelta)중 x(좌우)값에 민감도를 곱한다.
        // 카메라가 좌우로 회전하려면 rotation의 y 값에 넣어준다. -> 실습으로 확인
        // 좌우 회전은 플레이어(transform)를 회전시켜준다.
        // Why? 회전시킨 방향을 기준으로 앞뒤좌우 움직여야하니까.
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    // 입력 시스템에서 이동 입력을 받을 때 호출되는 함수
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        // 키보드나 조이스틱이 눌렸을 때
        if (context.phase == InputActionPhase.Performed)
        {
            // 입력된 2D 방향 값을 저장 (x: 좌우, y: 앞뒤)
            curMovementInput = context.ReadValue<Vector2>();
        }
        // 키보드나 조이스틱 입력이 멈췄을 때
        else if (context.phase == InputActionPhase.Canceled)
        {
            // 입력을 0으로 초기화해서 멈춤
            curMovementInput = Vector2.zero;
        }
    }

    // 실제로 이동을 처리하는 함수
    private void Move()
    {
        // 입력된 방향을 기준으로 이동 방향 벡터 계산 (전후 + 좌우)
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        // 이동 속도 곱해서 실제 속도 벡터로 변환
        dir *= speed;

        // y축 방향의 기존 속도는 그대로 유지 (점프나 중력 때문)
        dir.y = rb.velocity.y;

        // Rigidbody에 새로운 속도 적용
        rb.velocity = dir;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Started && IsGrounded())
        {

            Debug.Log("점프성공");
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);

        }

        else
        {

            Debug.Log("점프실패");

        }
    }

    bool IsGrounded()
    {

        // 4방향으로 레이를 쏴서 바닥이 있는지 확인
        Ray[] rays = new Ray[4]
        {
            // 앞쪽에서 아래로
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            // 뒤쪽에서 아래로
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            // 오른쪽에서 아래로
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            // 왼쪽에서 아래로
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)

        };

        for (int i = 0; i < rays.Length; i++)
        {
            Debug.DrawRay(rays[i].origin, rays[i].direction * 0.5f, Color.red);

            // Ray를 아래로 0.1만큼 쏴서 지정한 땅 레이어와 닿았는지 확인
            if (Physics.Raycast(rays[i], 0.5f, groundLayerMask))
            {
                return true; // 하나라도 바닥에 닿았으면 true 리턴
            }


        }

        return false; // 다 안 닿았으면 땅 위에 없음

    }






}
