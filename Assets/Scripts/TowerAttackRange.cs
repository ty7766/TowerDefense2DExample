using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{
    private void Awake()
    {
        OffAttackRange();
    }


    public void OnAttackRange(Vector3 position, float range)
    {
        gameObject.SetActive(true);
        //AttackRange�� 2�� ���Ͽ� ���� ���ϱ�
        float diameter = range * 2.0f;
        transform.localScale = Vector3.one * diameter;
        transform.position = position;
    }

    public void OffAttackRange()
    {
        gameObject.SetActive(false);
    }
}
