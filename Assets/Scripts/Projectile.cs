using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;

    public void SetUp(Transform target)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target;
    }

    private void Update()
    {
        //target이 존재하면 탄환을 target의 위치로 이동
        if (target != null)
        {
            Vector3 direction = (target.position-transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        //target이 없어지면
        else
        {
            Destroy(gameObject);
        }
    }


    //탄환 <-> 적 충돌
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return; //적이 아닌 대상과 부딪힘
        if (collision.transform != target) return;  //현재 target인 적이 아님

        //탄환이 명중하면 적 삭제면
        collision.GetComponent<Enemy>().OnDie();
        Destroy(gameObject);
    }
}
