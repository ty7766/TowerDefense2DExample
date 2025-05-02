using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen;      //플레이어 데미지를 입을때 이펙트

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

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        //체력이 0이 되면 게임 오버
        if (currentHP <= 0 )
        { }
    }

    private IEnumerator HitAlphaAnimation()
    {
        //이미지의 투명도를 40%로 설정
        Color color = imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;

        //투명도가 0이 될때까지 감소
        while (color.a >= 0.0f)
        {
            color.a = color.a - Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
    }
}
