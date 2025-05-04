using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public enum WeaponType { Cannon = 0, Laser, Slow, }
public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser,}

//Ÿ���� �ൿ
//1. �� Ž��
//2. �� ����

public class TowerWeapon : MonoBehaviour
{
    [Header("Commons")]
    [SerializeField]
    private TowerTemplate towerTemplate;                            // Ÿ�� ����
    [SerializeField]
    private Transform spawnPoint;                                   //�߻�ü ���� ��ġ
    [SerializeField]
    private WeaponType weaponType;                                  //���� �Ӽ� ����

    [Header("Cannon")]
    [SerializeField]
    private GameObject projectilePrefab;                            //�߻�ü

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;                              //������ ��
    [SerializeField]
    private Transform hitEffect;                                    //Ÿ�� ȿ��
    [SerializeField]
    private LayerMask targetLayer;                                  //������ �ε����� ���̾� ����

    private int level = 0;                                          //Ÿ�� ����
    private WeaponState weaponState = WeaponState.SearchTarget;     //Ÿ�� ���� ���� (1. �� Ž��)
    private Transform attackTarget = null;                          //���� ���(2. �� ����)
    private EnemySpawner enemySpawner;                              //���ӿ� �����ϴ� �� ���� ȹ���
    private SpriteRenderer spriteRenderer;                          //Ÿ�� ������Ʈ �̹��� �����
    private PlayerGold PlayerGold;                                  //�÷��̾� ��� ����
    private Tile ownerTile;                                         //���� Ÿ���� ��ġ�Ǿ��ִ� Ÿ��

    //Property
    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public float Slow => towerTemplate.weapon[level].slow;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;
    public WeaponType WeaponType => weaponType;


    //---------- �ʱ�ȭ -------------
    public void SetUp(EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile)
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.PlayerGold = playerGold;
        this.enemySpawner = enemySpawner;
        this.ownerTile = ownerTile;

        //���� �Ӽ��� �������� �ʴ� Ÿ���� [Ž�� <-> ����] ��ȯ�� �ʿ� �����Ƿ�
        if(weaponType == WeaponType.Cannon || weaponType == WeaponType.Laser)
        {
            ChangeState(WeaponState.SearchTarget);
        }
    }

    //-----------���� Ÿ���� �����Ÿ� ������ ������ [�� ����] ���·� �ٲٱ� ----------------
    public void ChangeState(WeaponState newState)
    { 
        //(��Ž��->������) (������->��Ž��)
        StopCoroutine(weaponState.ToString()); 
        weaponState = newState; //���� ����
        StartCoroutine(weaponState.ToString());
    }

    //�����Ÿ� �� ���� ������ Ÿ���� �ü��� �� �������� ������
    private void Update()
    {
        if (attackTarget != null)
            RotateToTarget();
    }

    //----------- Ÿ�� ���� ȸ�� -------------
    private void RotateToTarget()
    {
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg; //�� ������ ���� ����
        transform.rotation = Quaternion.Euler(0,0,degree);  //���� ��ŭ Z�� ȸ��
    }

    //------------- �� ���� �ý��� --------------
    private IEnumerator SearchTarget()
    {
        while(true)
        {
            //���� Ÿ���� ���� ������ �ִ� �� Ž��
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

    //------------- �� ���� �ý���(ĳ��) --------------
    private IEnumerator TryAttackCannon()
    {
        while(true)
        {
            //1. target ���� ��ȿ�� �˻�
            if (!IsPossibleToAttackTarget())
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //2. ���ݼӵ���ŭ ���
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            //3. �߻�
            SpawnProjectile();
        }
    }

    //------------- �� ���� �ý���(������) --------------
    private IEnumerator TryAttackLaser()
    {
        //������ ȿ�� Ȱ��ȭ
        EnableLaser();

        while(true)
        {
            //target ������ �Ұ����ϸ� ��Ȱ��ȭ�ϰ� Ž�� ��� ����
            if (IsPossibleToAttackTarget() == false)
            {
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //������ �����ϱ�
            SpawnLaser();

            yield return null;
        }
    }

    //----------------- Ÿ������ ���� ����� �� Ž�� --------------------
    private Transform FindClosetAttackTarget()
    {
        //���� ����� �� Ž�� -> ���� �Ÿ� �ִ��� ũ��
        float closestDistSqr = Mathf.Infinity;

        //EnemySpawner�� EnemyList�� �ְ� ���� �ʿ� �ִ� ��� �� �˻�
        for (int i = 0; i < enemySpawner.EnemyList.Count; ++i)
        {
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            //���� �˻����� ������ �Ÿ��� ���� ���� �ְ� ������� �˻��� ������ �Ÿ��� ������
            if (distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            {
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i].transform;
            }
        }

        return attackTarget;
    }

    //---------------- �� ������ �������� ---------------
    private bool IsPossibleToAttackTarget()
    {
        //���� �ִ��� �˻�
        if (attackTarget == null)
        {
            return false;
        }

        //���� ������ ����� ���ο� �� Ž��
        float distance = Vector3.Distance(attackTarget.position, transform.position);
        if (distance > towerTemplate.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }

        return true;
    }

    //------------- źȯ ���� ---------------
    private void SpawnProjectile()
    {
        //������ �߻�ü���� attackTarget ���� ����
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Projectile>().SetUp(attackTarget, towerTemplate.weapon[level].damage);
    }

    //------------- ������ Ȱ��ȭ -----------------
    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }

    //------------- ������ ��Ȱ��ȭ -----------------
    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }

    //---------------- ������ ���� �˰��� -------------------
    private void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction,
            towerTemplate.weapon[level].range, targetLayer);

        //���� �������� �������� ���� �߻�(�� ������Ʈ�� �����ִ� ��� �ٸ� ������Ʈ�� �������� ������ ���� ����)
        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].transform == attackTarget)
            {
                //������ ��������
                lineRenderer.SetPosition(0, spawnPoint.position);
                //������ ��ǥ����
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                //Ÿ�� ȿ�� ��ġ ����
                hitEffect.position = hit[i].point;
                //�� ü�� ���� �˰���(ƽ�� damage ����)
                attackTarget.GetComponent<EnemyHP>().TakeDamage(towerTemplate.weapon[level].damage * Time.deltaTime);
            }
        }
    }

    //---------------- Ÿ�� ���׷��̵� �ý��� -----------------
    public bool Upgrade()
    {
        //Ÿ�� ���׷��̵� ���� ������� �˻�
        if (PlayerGold.CurrentGold < towerTemplate.weapon[level + 1].cost)
            return false;

        level++;
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        PlayerGold.CurrentGold -= towerTemplate.weapon[level].cost;

        //���� �Ӽ��� �������̸�
        if (weaponType == WeaponType.Laser)
        {
            //���׷��̵帶�� ���� ����
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }
        return true;
    }

    //------------- Ÿ�� �Ǹ� �ý��� ---------------
    public void Sell()
    {
        //�Ǹ� �ݾ׸�ŭ ��带 �߰��ϰ� Ÿ�� �ʱ�ȭ �� Ÿ�� ����
        PlayerGold.CurrentGold += towerTemplate.weapon[level].sell;
        ownerTile.IsBulidTower = false;
        Destroy(gameObject);
    }
}
