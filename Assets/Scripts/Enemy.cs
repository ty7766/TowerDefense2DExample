using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Mech : Enemy Obj�� WayPoint�� Ȯ���ϸ� ���� ������ WayPoint�� �̵�
    private int wayPointCount;      //�̵� ��� ����
    private Transform[] wayPoints;  //�̵� ��� ����
    private int currentIndex = 0;
    private Movement2D movement2D;
    private EnemySpawner enemySpawner;      //�� ������ ������ ���� �ʰ� EnemySpawner�� ����

    [SerializeField]
    private float enemyRotation;

    public void SetUp(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        //�� �̵� ��� WayPoint ���� ����
        //Enemy�� �ùٸ� ������ WayPoint�� �ϳ��� ���󰡸鼭 Goal������ ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        //���� ��ġ�� ù ��° WayPoint ��ġ�� ����(Start���� ������)
        transform.position = wayPoints[currentIndex].position;

        //�� �̵�/��ǥ���� ���� �ݺ��Լ�
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        //���� ���� �ٷ� ���� WayPoints �� ���� ���� �̵� ����
        NextMoveTo();

        while(true)
        {
            transform.Rotate(Vector3.forward * enemyRotation);

            //Enemy�� ��ǥ ��ġ�� �ٴٸ��� ������ ���� �̵����� ����
            //Enemy�� �� ��Ż ����
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed)
                NextMoveTo();

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        //���� �̵��� WayPoint�� ������ ���� WayPoint �̵�
        //�ƴϸ� ������Ʈ ����
        if(currentIndex < wayPointCount - 1)
        {
            //���� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
            transform.position = wayPoints[currentIndex].position;
            //���� WayPoints�� �̵� ���� ����
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
            OnDie();
    }

    public void OnDie()
    {
        //���� ������ �� ����Ʈ������ ���� �ؾߵǱ� ������ ���⼭ �������� �ʰ� EnemySpawner�� �ѱ��
        enemySpawner.DestroyEnemy(this);
    }
}
