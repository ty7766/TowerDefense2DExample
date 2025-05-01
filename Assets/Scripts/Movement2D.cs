using UnityEngine;

public class Movement2D : MonoBehaviour
{
    //SrializeField : private ������ Inspector���� ���̱� ����
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    public float MoveSpeed => moveSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���� �ӵ���ŭ �Ÿ� �̵�(����ȭ)
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
