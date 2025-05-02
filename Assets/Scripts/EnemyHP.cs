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
    private SpriteRenderer spriteRenderer;  //�� ������Ʈ ����

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //������ ������ �Ҵ�
    public void TakeDamage(float damage)
    {
        if (isDie == true) return;
        
        //ü�� ����
        currentHP = currentHP - damage;


        //����ȭ �ڷ�ƾ ����
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        //ü���� 0 ���ϸ� ������Ʈ ����
        if (currentHP <= 0)
        {
            isDie = true;
            enemy.OnDie();
        }
    }

    //���� ������� ������ 40% ����ȭ
    private IEnumerator HitAlphaAnimation()
    {
        //���� �� ����
        Color color = spriteRenderer.color;
        color.a = 0.4f;
        spriteRenderer.color = color;

        yield return new WaitForSeconds(0.05f);
        color.a = 1.0f;
        spriteRenderer.color = color;

    }
}
