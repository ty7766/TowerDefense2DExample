using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject enemyPrefab;     //��
    //[SerializeField]    
    //private float spawnTime;            //���� �ֱ�

    [SerializeField]
    private GameObject enemyHPSliderPrefab;     //ü�¹� Slider UI ������
    [SerializeField]
    private Transform canvasTransform;          //Canvas
    [SerializeField]
    private Transform[] wayPoints;      //�� ������Ʈ �̵� ���
    [SerializeField]
    private PlayerHP playerHP;          //�÷��̾��� ü�� ������Ʈ
    [SerializeField]
    private PlayerGold playerGold;      //�÷��̾��� ���

    //���̺� ����
    private Wave currentWave;       //���� ���̺� ��
    private int currentEnemyCount;  //���� ���̺꿡�� �����ִ� �� ��

    //�� ���� ����
    private List<Enemy> enemyList;
    public List<Enemy> EnemyList => enemyList;
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    // -----------�ʱ�ȭ-----------
    void Awake()
    {
        enemyList = new List<Enemy>();

        /*enemyList = new List<Enemy>();
        StartCoroutine("SpawnEnemy");*/
    }

    //----------- ���̺� ���� -----------
    public void StartWave(Wave wave)
    {
        //�ʱ�ȭ
        currentWave = wave;
        currentEnemyCount = currentWave.maxEnemyCount;
        StartCoroutine("SpawnEnemy");
    }

    // ---------- ���̺� �� ������Ʈ & UI ���� ------------
    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;    //���� ���̺꿡 ������ �� ����
        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            //���̺꿡 �����ϴ� ���� �������� �� ������ ���� ����
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();   //������ �� ������Ʈ

            enemy.SetUp(this, wayPoints);   //�� ���� �ʱ�ȭ
            enemyList.Add(enemy);           //������ �� ����Ʈ�� �߰�
            SpawnEnemyHPSlider(clone);      //�� UI ����

            spawnEnemyCount++;

            yield return new WaitForSeconds(currentWave.spawnTime);
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

        currentEnemyCount--;
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
