using UnityEngine;

public class Movement2D : MonoBehaviour
{
    //SrializeField : private ������ Inspector���� ���̱� ����
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    private float baseMoveSpeed;        //SlowTower��

    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0,value);
        get => moveSpeed;
    }

    private void Awake()
    {
        baseMoveSpeed = MoveSpeed;
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

    public void ResetMoveSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }
}
