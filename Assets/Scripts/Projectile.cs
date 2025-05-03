using UnityEngine;

//탄환 클래스
public class Projectile : MonoBehaviour
{
    private Movement2D  movement2D;
    private Transform   target;
    private float       damage;

    //------------ 초기화 ---------------
    public void SetUp(Transform target, float damage)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target;
        this.damage = damage;
    }

    //--------------- 적 오브젝트에 탄환 이동 -----------------
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

        //탄환이 명중하면 [대미지]만큼 적 체력 감소
        collision.GetComponent<EnemyHP>().TakeDamage(damage);
        //탄환 제거
        Destroy(gameObject);
    }
}
