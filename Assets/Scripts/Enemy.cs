using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Mech : Enemy Obj가 WayPoint를 확인하면 다음 순서의 WayPoint로 이동
    private int wayPointCount;      //이동 경로 개수
    private Transform[] wayPoints;  //이동 경로 정보
    private int currentIndex = 0;
    private Movement2D movement2D;
    private EnemySpawner enemySpawner;      //적 삭제를 본인이 하지 않고 EnemySpawner에 전달

    [SerializeField]
    private float enemyRotation;

    public void SetUp(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        //적 이동 경로 WayPoint 정보 설정
        //Enemy는 올바른 순서의 WayPoint를 하나씩 따라가면서 Goal지점에 도착
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        //적의 위치를 첫 번째 WayPoint 위치로 설정(Start에서 리스폰)
        transform.position = wayPoints[currentIndex].position;

        //적 이동/목표지점 설정 반복함수
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        //스폰 지점 바로 다음 WayPoints 로 다음 방향 이동 설정
        NextMoveTo();

        while(true)
        {
            transform.Rotate(Vector3.forward * enemyRotation);

            //Enemy가 목표 위치에 다다르기 직전에 다음 이동방향 설정
            //Enemy의 맵 이탈 방지
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed)
                NextMoveTo();

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        //아직 이동할 WayPoint가 있으면 다음 WayPoint 이동
        //아니면 오브젝트 삭제
        if(currentIndex < wayPointCount - 1)
        {
            //적의 위치를 정확하게 목표 위치로 설정
            transform.position = wayPoints[currentIndex].position;
            //다음 WayPoints로 이동 방향 설정
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
            OnDie();
    }

    public void OnDie()
    {
        //적이 삭제될 때 리스트에서도 제거 해야되기 때문에 여기서 제거하지 않고 EnemySpawner로 넘기기
        enemySpawner.DestroyEnemy(this);
    }
}
