using UnityEngine;

public class Movement2D : MonoBehaviour
{
    //SrializeField : private 변수도 Inspector에서 보이기 가능
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
        //일정 속도만큼 거리 이동(동기화)
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
