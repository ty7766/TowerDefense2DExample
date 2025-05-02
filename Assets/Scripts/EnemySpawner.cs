using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;     //적
    [SerializeField]
    private GameObject enemyHPSliderPrefab;     //체력바 Slider UI 프리팹
    [SerializeField]
    private Transform canvasTransform;          //Canvas
    [SerializeField]    
    private float spawnTime;            //생성 주기
    [SerializeField]
    private Transform[] wayPoints;      //적 오브젝트 이동 경로
    [SerializeField]
    private PlayerHP playerHP;          //플레이어의 체력 컴포넌트
    [SerializeField]
    private PlayerGold playerGold;      //플레이어의 골드

    private List<Enemy> enemyList;
    public List<Enemy> EnemyList => enemyList;

    // -----------초기화-----------
    void Awake()
    {
        enemyList = new List<Enemy>();
        StartCoroutine("SpawnEnemy");
    }

    // ---------- 적 오브젝트 & UI 생성 ------------
    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate(enemyPrefab);    //적 오브젝트 생성
            Enemy enemy = clone.GetComponent<Enemy>();      //클론 가져오기
            enemy.SetUp(this, wayPoints);                         //적 이동 함수 루틴 실행
            enemyList.Add(enemy);                           //적이 생성되면 적 리스트에 추가

            SpawnEnemyHPSlider(clone);                      //적 체력을 나타내는 Slider UI 생성 및 호출
            yield return new WaitForSeconds(spawnTime);     //대기
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
