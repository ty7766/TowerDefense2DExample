using UnityEngine;

public class Slow : MonoBehaviour
{
    private TowerWeapon towerWeapon;

    private void Awake()
    {
        towerWeapon = GetComponentInParent<TowerWeapon>();
    }

    // ---------------- ���� ������ �ӵ� ���� ----------------
    //���� Ÿ�� ������ �°� ��Ŭ �ݶ��̴� ����, �̸� �������� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Enemy"))
        {
            return;
        }

        Movement2D movement2D = collision.GetComponent<Movement2D>();
        movement2D.MoveSpeed -= movement2D.MoveSpeed * towerWeapon.Slow;
    }

    //---------------- ���� ������ �̵��ӵ� ���� --------------
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }

        collision.GetComponent<Movement2D>().ResetMoveSpeed();
    }
}
