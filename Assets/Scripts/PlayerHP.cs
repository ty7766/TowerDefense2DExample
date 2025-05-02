using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen;      //�÷��̾� �������� ������ ����Ʈ

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

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        //ü���� 0�� �Ǹ� ���� ����
        if (currentHP <= 0 )
        { }
    }

    private IEnumerator HitAlphaAnimation()
    {
        //�̹����� ������ 40%�� ����
        Color color = imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;

        //������ 0�� �ɶ����� ����
        while (color.a >= 0.0f)
        {
            color.a = color.a - Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
    }
}
