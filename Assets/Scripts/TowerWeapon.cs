using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public enum WeaponType { Cannon = 0, Laser, Slow, }
public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser,}

//타워의 행동
//1. 적 탐지
//2. 적 공격

public class TowerWeapon : MonoBehaviour
{
    [Header("Commons")]
    [SerializeField]
    private TowerTemplate towerTemplate;                            // 타워 정보
    [SerializeField]
    private Transform spawnPoint;                                   //발사체 생성 위치
    [SerializeField]
    private WeaponType weaponType;                                  //무기 속성 설정

    [Header("Cannon")]
    [SerializeField]
    private GameObject projectilePrefab;                            //발사체

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;                              //레이저 선
    [SerializeField]
    private Transform hitEffect;                                    //타격 효과
    [SerializeField]
    private LayerMask targetLayer;                                  //광선에 부딪히는 레이어 설정

    private int level = 0;                                          //타워 레벨
    private WeaponState weaponState = WeaponState.SearchTarget;     //타워 무기 상태 (1. 적 탐지)
    private Transform attackTarget = null;                          //공격 대상(2. 적 공격)
    private EnemySpawner enemySpawner;                              //게임에 존재하는 적 정보 획득용
    private SpriteRenderer spriteRenderer;                          //타워 오브젝트 이미지 변경용
    private PlayerGold PlayerGold;                                  //플레이어 골드 정보
    private Tile ownerTile;                                         //현재 타워가 배치되어있는 타일

    //Property
    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public float Slow => towerTemplate.weapon[level].slow;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;
    public WeaponType WeaponType => weaponType;


    //---------- 초기화 -------------
    public void SetUp(EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile)
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.PlayerGold = playerGold;
        this.enemySpawner = enemySpawner;
        this.ownerTile = ownerTile;

        //무기 속성이 공격하지 않는 타워면 [탐지 <-> 공격] 전환이 필요 없으므로
        if(weaponType == WeaponType.Cannon || weaponType == WeaponType.Laser)
        {
            ChangeState(WeaponState.SearchTarget);
        }
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
            //현재 타워에 가장 가까이 있는 적 탐색
            attackTarget = FindClosetAttackTarget();

            if (attackTarget != null)
            {
                if (weaponType == WeaponType.Cannon)
                    ChangeState(WeaponState.TryAttackCannon);
                else if (weaponType == WeaponType.Laser)
                    ChangeState(WeaponState.TryAttackLaser);
            }
            yield return null;
        }
    }

    //------------- 적 공격 시스템(캐논) --------------
    private IEnumerator TryAttackCannon()
    {
        while(true)
        {
            //1. target 가능 유효성 검사
            if (!IsPossibleToAttackTarget())
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //2. 공격속도만큼 대기
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            //3. 발사
            SpawnProjectile();
        }
    }

    //------------- 적 공격 시스템(레이저) --------------
    private IEnumerator TryAttackLaser()
    {
        //레이저 효과 활성화
        EnableLaser();

        while(true)
        {
            //target 공격이 불가능하면 비활성화하고 탐색 모드 진입
            if (IsPossibleToAttackTarget() == false)
            {
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //레이저 공격하기
            SpawnLaser();

            yield return null;
        }
    }

    //----------------- 타워에서 가장 가까운 적 탐색 --------------------
    private Transform FindClosetAttackTarget()
    {
        //제일 가까운 적 탐색 -> 최초 거리 최대한 크게
        float closestDistSqr = Mathf.Infinity;

        //EnemySpawner의 EnemyList에 있고 현재 맵에 있는 모든 적 검사
        for (int i = 0; i < enemySpawner.EnemyList.Count; ++i)
        {
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            //현재 검사중인 적과의 거리가 범위 내에 있고 현재까지 검사한 적보다 거리가 가까우면
            if (distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            {
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i].transform;
            }
        }

        return attackTarget;
    }

    //---------------- 적 공격이 가능한지 ---------------
    private bool IsPossibleToAttackTarget()
    {
        //적이 있는지 검사
        if (attackTarget == null)
        {
            return false;
        }

        //공격 범위를 벗어나면 새로운 적 탐색
        float distance = Vector3.Distance(attackTarget.position, transform.position);
        if (distance > towerTemplate.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }

        return true;
    }

    //------------- 탄환 생성 ---------------
    private void SpawnProjectile()
    {
        //생성된 발사체에게 attackTarget 정보 제공
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Projectile>().SetUp(attackTarget, towerTemplate.weapon[level].damage);
    }

    //------------- 레이저 활성화 -----------------
    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }

    //------------- 레이저 비활성화 -----------------
    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }

    //---------------- 레이저 공격 알고리즘 -------------------
    private void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction,
            towerTemplate.weapon[level].range, targetLayer);

        //같은 방향으로 여러개의 광선 발사(적 오브젝트가 겹쳐있는 경우 다른 오브젝트에 레이저가 막히는 현상 방지)
        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].transform == attackTarget)
            {
                //레이저 시작지점
                lineRenderer.SetPosition(0, spawnPoint.position);
                //레이저 목표지점
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                //타격 효과 위치 설정
                hitEffect.position = hit[i].point;
                //적 체력 감소 알고리즘(틱당 damage 적용)
                attackTarget.GetComponent<EnemyHP>().TakeDamage(towerTemplate.weapon[level].damage * Time.deltaTime);
            }
        }
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

        //무기 속성이 레이저이면
        if (weaponType == WeaponType.Laser)
        {
            //업그레이드마다 굵기 적용
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }
        return true;
    }

    //------------- 타워 판매 시스템 ---------------
    public void Sell()
    {
        //판매 금액만큼 골드를 추가하고 타일 초기화 및 타워 삭제
        PlayerGold.CurrentGold += towerTemplate.weapon[level].sell;
        ownerTile.IsBulidTower = false;
        Destroy(gameObject);
    }
}
