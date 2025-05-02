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
        //적이 End에 들어오면 플레이어 데미지 감소
        currentHP -= damage;

        //체력이 0이 되면 게임 오버
        if(currentHP <= 0 )
        { }
    }
}
