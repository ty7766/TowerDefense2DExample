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
    private GameObject projectilePrefab;                            //�߻�ü
    [SerializeField]
    private Transform spawnPoint;                                   //�߻�ü ���� ��ġ
    [SerializeField]
    private float attackRate = 0.5f;                                //���� �ӵ�
    [SerializeField]
    private float attackRange = 2.0f;                               //���� ����
    private WeaponState weaponState = WeaponState.SearchTarget;     //Ÿ�� ���� ���� (1. �� Ž��)
    private Transform attackTarget = null;                          //���� ���(2. �� ����)
    private EnemySpawner enemySpawner;                              //���ӿ� �����ϴ� �� ���� ȹ���

    public void SetUp(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;
        ChangeState(WeaponState.SearchTarget);      //���� ���¸� 1. �� Ž���� ����
    }

    //���� Ÿ���� �����Ÿ� ������ ������ 2. �� ���� ���·� �ٲٱ�
    public void ChangeState(WeaponState newState)
    { 
        //(��Ž��->������) (������->��Ž��)
        StopCoroutine(weaponState.ToString()); 
        weaponState = newState; //���� ����
        StartCoroutine(weaponState.ToString());
    }

    private void Update()
    {
        //�����Ÿ� �� ���� ������ Ÿ���� �ü��� �� �������� ������
        if (attackTarget != null)
            RotateToTarget();
    }

    private void RotateToTarget()
    {
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg; //�� ������ ���� ����
        transform.rotation = Quaternion.Euler(0,0,degree);  //���� ��ŭ Z�� ȸ��
    }

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
            //1. Target�� ������ Ž������ ��ȯ
            if(attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //2. Target�� Ÿ�� �����Ÿ� ���� ������ Ž������ ��ȯ
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if(distance <= attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //3. ���ݼӵ���ŭ ���
            yield return new WaitForSeconds(attackRate);

            //4. �߻�
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
    }
}
