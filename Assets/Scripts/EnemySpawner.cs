using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;     //적
    [SerializeField]    
    private float spawnTime;            //생성 주기
    [SerializeField]
    private Transform[] wayPoints;      //적 오브젝트 이동 경로
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
            GameObject clone = Instantiate(enemyPrefab);    //적 오브젝트 생성
            Enemy enemy = clone.GetComponent<Enemy>();      //클론 가져오기
            enemy.SetUp(wayPoints);                         //적 이동 함수 루틴 실행
            enemyList.Add(enemy);                           //적이 생성되면 적 리스트에 추가
            yield return new WaitForSeconds(spawnTime);     //대기
        }
    }

    public void DestroyEnemy(Enemy enemy)
    {
        //리스트에 저장된 적 정보와 적 오브젝트 삭제
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}
