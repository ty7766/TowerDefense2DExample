using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP = 20;
    private float currentHP;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        //���� End�� ������ �÷��̾� ������ ����
        currentHP -= damage;

        //ü���� 0�� �Ǹ� ���� ����
        if(currentHP <= 0 )
        { }
    }
}
