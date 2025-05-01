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
        //target�� �����ϸ� źȯ�� target�� ��ġ�� �̵�
        if (target != null)
        {
            Vector3 direction = (target.position-transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        //target�� ��������
        else
        {
            Destroy(gameObject);
        }
    }


    //źȯ <-> �� �浹
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return; //���� �ƴ� ���� �ε���
        if (collision.transform != target) return;  //���� target�� ���� �ƴ�

        //źȯ�� �����ϸ� �� ������
        collision.GetComponent<Enemy>().OnDie();
        Destroy(gameObject);
    }
}
