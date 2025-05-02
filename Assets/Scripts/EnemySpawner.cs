using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject enemyPrefab;     //적
    //[SerializeField]    
    //private float spawnTime;            //생성 주기

    [SerializeField]
    private GameObject enemyHPSliderPrefab;     //체력바 Slider UI 프리팹
    [SerializeField]
    private Transform canvasTransform;          //Canvas
    [SerializeField]
    private Transform[] wayPoints;      //적 오브젝트 이동 경로
    [SerializeField]
    private PlayerHP playerHP;          //플레이어의 체력 컴포넌트
    [SerializeField]
    private PlayerGold playerGold;      //플레이어의 골드

    //웨이브 정보
    private Wave currentWave;       //현재 웨이브 수
    private int currentEnemyCount;  //현재 웨이브에서 남아있는 적 수

    //적 생성 정보
    private List<Enemy> enemyList;
    public List<Enemy> EnemyList => enemyList;
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    // -----------초기화-----------
    void Awake()
    {
        enemyList = new List<Enemy>();

        /*enemyList = new List<Enemy>();
        StartCoroutine("SpawnEnemy");*/
    }

    //----------- 웨이브 시작 -----------
    public void StartWave(Wave wave)
    {
        //초기화
        currentWave = wave;
        currentEnemyCount = currentWave.maxEnemyCount;
        StartCoroutine("SpawnEnemy");
    }

    // ---------- 웨이브 적 오브젝트 & UI 생성 ------------
    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;    //현재 웨이브에 생성한 적 숫자
        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            //웨이브에 등장하는 적이 여러개일 때 임의의 적을 생성
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();   //생성한 적 오브젝트

            enemy.SetUp(this, wayPoints);   //적 정보 초기화
            enemyList.Add(enemy);           //생성된 적 리스트에 추가
            SpawnEnemyHPSlider(clone);      //적 UI 설정

            spawnEnemyCount++;

            yield return new WaitForSeconds(currentWave.spawnTime);
        }


    }

    //--------- 적 오브젝트 처리(End에 왔을때, 사망했을 때) ---------
    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        //적이 End에 도착했을 때
        if (type == EnemyDestroyType.Arrive)
        {
            playerHP.TakeDamage(1);
        }
        //적이 탄환에 맞아 사망했을 때
        else if (type == EnemyDestroyType.Kill)
        {
            playerGold.CurrentGold += gold;
        }

        currentEnemyCount--;
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    //---------적 UI가 적 오브젝트에 붙게 만들기 ------------
    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);  //SliderUI 생성
        sliderClone.transform.SetParent(canvasTransform);           //부모오브젝트(Canvas)크기를 1로 설정
        sliderClone.transform.localScale = Vector3.one;

        //slider가 쫓아다닐 대상을 본인으로 설정
        sliderClone.GetComponent<SliderPositionAutoSetter>().SetUp(enemy.transform);
        //slider UI에 자신의 체력 정보를 표시하도록 설정
        sliderClone.GetComponent<EnemyHPViewer>().SetUp(enemy.GetComponent<EnemyHP>());
    }
}
