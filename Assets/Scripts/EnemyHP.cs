using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    private float currentHP;
    private bool isDie = false;
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;  //적 오브젝트 색상

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //적에게 데미지 할당
    public void TakeDamage(float damage)
    {
        if (isDie == true) return;
        
        //체력 감소
        currentHP = currentHP - damage;


        //투명화 코루틴 실행
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        //체력이 0 이하면 오브젝트 삭제
        if (currentHP <= 0)
        {
            isDie = true;
            enemy.OnDie();
        }
    }

    //적이 대미지를 받으면 40% 투명화
    private IEnumerator HitAlphaAnimation()
    {
        //현재 적 색상
        Color color = spriteRenderer.color;
        color.a = 0.4f;
        spriteRenderer.color = color;

        yield return new WaitForSeconds(0.05f);
        color.a = 1.0f;
        spriteRenderer.color = color;

    }
}
