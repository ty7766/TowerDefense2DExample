using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public enum WeaponState { SearchTarget = 0, AttackToTarget}

//타워의 행동
//1. 적 탐지
//2. 적 공격

public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;                            //발사체
    [SerializeField]
    private Transform spawnPoint;                                   //발사체 생성 위치
    [SerializeField]
    private float attackRate = 0.5f;                                //공격 속도
    [SerializeField]
    private float attackRange = 2.0f;                               //공격 범위
    private WeaponState weaponState = WeaponState.SearchTarget;     //타워 무기 상태 (1. 적 탐지)
    private Transform attackTarget = null;                          //공격 대상(2. 적 공격)
    private EnemySpawner enemySpawner;                              //게임에 존재하는 적 정보 획득용

    public void SetUp(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;
        ChangeState(WeaponState.SearchTarget);      //최초 상태를 1. 적 탐지로 설정
    }

    //적이 타워의 사정거리 안으로 들어오면 2. 적 공격 상태로 바꾸기
    public void ChangeState(WeaponState newState)
    { 
        //(적탐지->적공격) (적공격->적탐지)
        StopCoroutine(weaponState.ToString()); 
        weaponState = newState; //상태 변경
        StartCoroutine(weaponState.ToString());
    }

    private void Update()
    {
        //사정거리 내 적이 들어오면 타워의 시선을 적 방향으로 돌리기
        if (attackTarget != null)
            RotateToTarget();
    }

    private void RotateToTarget()
    {
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg; //도 단위의 각도 저장
        transform.rotation = Quaternion.Euler(0,0,degree);  //각도 만큼 Z축 회전
    }

    private IEnumerator SearchTarget()
    {
        while(true)
        {
            //제일 가까이 있는 적을 찾기 위해 최초 거리를 최대한 크게 설정
            float closestDistSqr = Mathf.Infinity;

            //맵에 존재하는 모든 적 검사
            for(int i = 0; i < enemySpawner.EnemyList.Count; ++i)
            {
                //적과 타워의 거리 계산
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                //사정 거리 내 적이 있고, 현재까지 검사한 적보다 거리가 가까우면
                if(distance <= attackRange && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform;
                }
            }

            if (attackTarget != null)
                ChangeState(WeaponState.AttackToTarget);

            yield return null;
        }
    }

    private IEnumerator AttackToTarget()
    {
        while(true)
        {
            //1. Target이 없으면 탐색으로 전환
            if(attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //2. Target이 타워 사정거리 내에 없으면 탐색으로 전환
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if(distance <= attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //3. 공격속도만큼 대기
            yield return new WaitForSeconds(attackRate);

            //4. 발사
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
    }
}
