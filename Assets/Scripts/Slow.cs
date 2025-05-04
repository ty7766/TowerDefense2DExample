using UnityEngine;

public class Slow : MonoBehaviour
{
    private TowerWeapon towerWeapon;

    private void Awake()
    {
        towerWeapon = GetComponentInParent<TowerWeapon>();
    }

    // ---------------- 적이 들어오면 속도 감소 ----------------
    //감속 타워 범위에 맞게 서클 콜라이더 설정, 이를 기준으로 적용
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Enemy"))
        {
            return;
        }

        Movement2D movement2D = collision.GetComponent<Movement2D>();
        movement2D.MoveSpeed -= movement2D.MoveSpeed * towerWeapon.Slow;
    }

    //---------------- 적이 나가면 이동속도 복구 --------------
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }

        collision.GetComponent<Movement2D>().ResetMoveSpeed();
    }
}
