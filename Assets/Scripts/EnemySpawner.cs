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
    [SerializeField]
    private PlayerGold playerGold;      //�÷��̾��� ���

    private List<Enemy> enemyList;
    public List<Enemy> EnemyList => enemyList;

    // -----------�ʱ�ȭ-----------
    void Awake()
    {
        enemyList = new List<Enemy>();
        StartCoroutine("SpawnEnemy");
    }

    // ---------- �� ������Ʈ & UI ���� ------------
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

    //--------- �� ������Ʈ ó��(End�� ������, ������� ��) ---------
    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        //���� End�� �������� ��
        if (type == EnemyDestroyType.Arrive)
        {
            playerHP.TakeDamage(1);
        }
        //���� źȯ�� �¾� ������� ��
        else if (type == EnemyDestroyType.Kill)
        {
            playerGold.CurrentGold += gold;
        }

        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    //---------�� UI�� �� ������Ʈ�� �ٰ� ����� ------------
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
