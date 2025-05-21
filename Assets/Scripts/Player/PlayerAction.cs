using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    // �÷��̾��� �̵� �ӵ��� �����ϴ� ����
    public float speed = 5f;

    // ���� ����Ű �Է� ���� �����ϴ� ���� (2D ���� ����)
    private Vector2 curMovementInput;

    // Rigidbody ������Ʈ�� ������ ����
    private Rigidbody rb;

    public float jumpPower = 5f;
    
    public LayerMask groundLayerMask;
    void Start()
    {
        // ���� GameObject���� Rigidbody ������Ʈ�� �����ͼ� ����
        rb = GetComponent<Rigidbody>();
    }

    
    private void FixedUpdate()
    {
        // �÷��̾� �̵� ó��
        Move();
    }

    // �Է� �ý��ۿ��� �̵� �Է��� ���� �� ȣ��Ǵ� �Լ�
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        // Ű���峪 ���̽�ƽ�� ������ ��
        if (context.phase == InputActionPhase.Performed)
        {
            // �Էµ� 2D ���� ���� ���� (x: �¿�, y: �յ�)
            curMovementInput = context.ReadValue<Vector2>();
        }
        // Ű���峪 ���̽�ƽ �Է��� ������ ��
        else if (context.phase == InputActionPhase.Canceled)
        {
            // �Է��� 0���� �ʱ�ȭ�ؼ� ����
            curMovementInput = Vector2.zero;
        }
    }

    // ������ �̵��� ó���ϴ� �Լ�
    private void Move()
    {
        // �Էµ� ������ �������� �̵� ���� ���� ��� (���� + �¿�)
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        // �̵� �ӵ� ���ؼ� ���� �ӵ� ���ͷ� ��ȯ
        dir *= speed;

        // y�� ������ ���� �ӵ��� �״�� ���� (������ �߷� ����)
        dir.y = rb.velocity.y;

        // Rigidbody�� ���ο� �ӵ� ����
        rb.velocity = dir;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Started && IsGrounded())
        {

            Debug.Log("��������");
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);

        }

        else
        {

            Debug.Log("��������");

        }
    }

    bool IsGrounded()
    {

        // 4�������� ���̸� ���� �ٴ��� �ִ��� Ȯ��
        Ray[] rays = new Ray[4]
        {
            // ���ʿ��� �Ʒ���
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            // ���ʿ��� �Ʒ���
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            // �����ʿ��� �Ʒ���
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            // ���ʿ��� �Ʒ���
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)

        };

        for (int i = 0; i < rays.Length; i++)
        {
            Debug.DrawRay(rays[i].origin, rays[i].direction * 0.5f, Color.red);

            // Ray�� �Ʒ��� 0.1��ŭ ���� ������ �� ���̾�� ��Ҵ��� Ȯ��
            if (Physics.Raycast(rays[i], 0.5f, groundLayerMask))
            {
                return true; // �ϳ��� �ٴڿ� ������� true ����
            }


        }

        return false; // �� �� ������� �� ���� ����

    }




}
