using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public enum WeaponState { SearchTarget = 0, AttackToTarget}

//Ÿ���� �ൿ
//1. �� Ž��
//2. �� ����

public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate towerTemplate;                            // Ÿ�� ����
    [SerializeField]
    private GameObject projectilePrefab;                            //�߻�ü
    [SerializeField]
    private Transform spawnPoint;                                   //�߻�ü ���� ��ġ

    private int level = 0;                                          //Ÿ�� ����
    private WeaponState weaponState = WeaponState.SearchTarget;     //Ÿ�� ���� ���� (1. �� Ž��)
    private Transform attackTarget = null;                          //���� ���(2. �� ����)
    private EnemySpawner enemySpawner;                              //���ӿ� �����ϴ� �� ���� ȹ���
    private SpriteRenderer spriteRenderer;                          //Ÿ�� ������Ʈ �̹��� �����
    private PlayerGold PlayerGold;                                  //�÷��̾� ��� ����

    //Property
    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;


    //---------- �ʱ�ȭ -------------
    public void SetUp(EnemySpawner enemySpawner, PlayerGold playerGold)
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.PlayerGold = playerGold;
        this.enemySpawner = enemySpawner;

        ChangeState(WeaponState.SearchTarget);      //���� ���¸� [�� Ž��] ���·� ����
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
            //���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��� ũ�� ����
            float closestDistSqr = Mathf.Infinity;

            //�ʿ� �����ϴ� ��� �� �˻�
            for(int i = 0; i < enemySpawner.EnemyList.Count; ++i)
            {
                //���� Ÿ���� �Ÿ� ���
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                //���� �Ÿ� �� ���� �ְ�, ������� �˻��� ������ �Ÿ��� ������
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

    //------------- �� ���� �ý��� --------------
    private IEnumerator AttackToTarget()
    {
        while(true)
        {
            //1. Target�� ������ Ž������ ��ȯ
            if(attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //2. Target�� Ÿ�� �����Ÿ� ���� ������ Ž������ ��ȯ
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if(distance > towerTemplate.weapon[level].range)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //3. ���ݼӵ���ŭ ���
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            //4. �߻�
            SpawnProjectile();
        }
    }

    //------------- źȯ ���� ---------------
    private void SpawnProjectile()
    {
        //������ �߻�ü���� attackTarget ���� ����
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Projectile>().SetUp(attackTarget, towerTemplate.weapon[level].damage);
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

        return true;
    }
}
