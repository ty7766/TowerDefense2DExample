using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;     //��
    [SerializeField]
    private GameObject enemyHPSliderPrefab;     //ü�¹� Slider UI ������
    [SerializeField]
    private Transform canvasTransform;          //Canvas
    [SerializeField]    
    private float spawnTime;            //���� �ֱ�
    [SerializeField]
    private Transform[] wayPoints;      //�� ������Ʈ �̵� ���
    [SerializeField]
    private PlayerHP playerHP;          //�÷��̾��� ü�� ������Ʈ

    private List<Enemy> enemyList;
    public List<Enemy> EnemyList => enemyList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        enemyList = new List<Enemy>();
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate(enemyPrefab);    //�� ������Ʈ ����
            Enemy enemy = clone.GetComponent<Enemy>();      //Ŭ�� ��������
            enemy.SetUp(this, wayPoints);                         //�� �̵� �Լ� ��ƾ ����
            enemyList.Add(enemy);                           //���� �����Ǹ� �� ����Ʈ�� �߰�

            SpawnEnemyHPSlider(clone);                      //�� ü���� ��Ÿ���� Slider UI ���� �� ȣ��
            yield return new WaitForSeconds(spawnTime);     //���
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy)
    {
        //���� End�� �����ϸ� �÷��̾� ü�� -1
        if (type == EnemyDestroyType.Arrive)
        {
            playerHP.TakeDamage(1);
        }

        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);  //SliderUI ����
        sliderClone.transform.SetParent(canvasTransform);           //�θ������Ʈ(Canvas)ũ�⸦ 1�� ����
        sliderClone.transform.localScale = Vector3.one;

        //slider�� �Ѿƴٴ� ����� �������� ����
        sliderClone.GetComponent<SliderPositionAutoSetter>().SetUp(enemy.transform);
        //slider UI�� �ڽ��� ü�� ������ ǥ���ϵ��� ����
        sliderClone.GetComponent<EnemyHPViewer>().SetUp(enemy.GetComponent<EnemyHP>());
    }
}
