using UnityEngine;

public class Movement2D : MonoBehaviour
{
    //SrializeField : private 변수도 Inspector에서 보이기 가능
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    private float baseMoveSpeed;        //SlowTower용

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
        //일정 속도만큼 거리 이동(동기화)
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
