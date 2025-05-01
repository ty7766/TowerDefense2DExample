using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;     //��
    [SerializeField]    
    private float spawnTime;            //���� �ֱ�
    [SerializeField]
    private Transform[] wayPoints;      //�� ������Ʈ �̵� ���
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
            enemy.SetUp(wayPoints);                         //�� �̵� �Լ� ��ƾ ����
            enemyList.Add(enemy);                           //���� �����Ǹ� �� ����Ʈ�� �߰�
            yield return new WaitForSeconds(spawnTime);     //���
        }
    }

    public void DestroyEnemy(Enemy enemy)
    {
        //����Ʈ�� ����� �� ������ �� ������Ʈ ����
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}
