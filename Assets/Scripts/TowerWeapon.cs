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
    private TowerTemplate towerTemplate;                            // 타워 정보
    [SerializeField]
    private GameObject projectilePrefab;                            //발사체
    [SerializeField]
    private Transform spawnPoint;                                   //발사체 생성 위치

    private int level = 0;                                          //타워 레벨
    private WeaponState weaponState = WeaponState.SearchTarget;     //타워 무기 상태 (1. 적 탐지)
    private Transform attackTarget = null;                          //공격 대상(2. 적 공격)
    private EnemySpawner enemySpawner;                              //게임에 존재하는 적 정보 획득용
    private SpriteRenderer spriteRenderer;                          //타워 오브젝트 이미지 변경용
    private PlayerGold PlayerGold;                                  //플레이어 골드 정보

    //Property
    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;


    //---------- 초기화 -------------
    public void SetUp(EnemySpawner enemySpawner, PlayerGold playerGold)
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.PlayerGold = playerGold;
        this.enemySpawner = enemySpawner;

        ChangeState(WeaponState.SearchTarget);      //최초 상태를 [적 탐지] 상태로 설정
    }

    //-----------적이 타워의 사정거리 안으로 들어오면 [적 공격] 상태로 바꾸기 ----------------
    public void ChangeState(WeaponState newState)
    { 
        //(적탐지->적공격) (적공격->적탐지)
        StopCoroutine(weaponState.ToString()); 
        weaponState = newState; //상태 변경
        StartCoroutine(weaponState.ToString());
    }

    //사정거리 내 적이 들어오면 타워의 시선을 적 방향으로 돌리기
    private void Update()
    {
        if (attackTarget != null)
            RotateToTarget();
    }

    //----------- 타워 방향 회전 -------------
    private void RotateToTarget()
    {
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg; //도 단위의 각도 저장
        transform.rotation = Quaternion.Euler(0,0,degree);  //각도 만큼 Z축 회전
    }

    //------------- 적 감지 시스템 --------------
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
                if (distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
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

    //------------- 적 공격 시스템 --------------
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
            if(distance > towerTemplate.weapon[level].range)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //3. 공격속도만큼 대기
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            //4. 발사
            SpawnProjectile();
        }
    }

    //------------- 탄환 생성 ---------------
    private void SpawnProjectile()
    {
        //생성된 발사체에게 attackTarget 정보 제공
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Projectile>().SetUp(attackTarget, towerTemplate.weapon[level].damage);
    }

    //---------------- 타워 업그레이드 시스템 -----------------
    public bool Upgrade()
    {
        //타워 업그레이드 돈이 충분한지 검사
        if (PlayerGold.CurrentGold < towerTemplate.weapon[level + 1].cost)
            return false;

        level++;
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        PlayerGold.CurrentGold -= towerTemplate.weapon[level].cost;

        return true;
    }
}
