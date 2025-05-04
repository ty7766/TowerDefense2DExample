using UnityEngine;
using System.Collections;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;          //Ÿ�� ����
    [SerializeField]
    private EnemySpawner enemySpawner;              //���� �ʿ� �����ϴ� �� ����Ʈ ���� ���
    [SerializeField]
    private PlayerGold playerGold;                  //�÷��̾� ��忡�� Ÿ�� �Ǽ��� ������
    [SerializeField]
    private SystemTextViewer systemTextViewer;      //�� ����, �Ǽ� �Ұ� �� ���� �޼��� ���

    private bool isOnTowerButton = false;           //Ÿ�� �Ǽ� ��ư üũ
    private GameObject followTowerClone = null;     //�ӽ� Ÿ�� ��� �Ϸ�� ������ ���� ����
    private int towerType;                          //Ÿ�� �Ӽ�

    //--------------- Ÿ�� ��ġ üũ ---------------
    public void ReadyToSpawnTower(int type)
    {
        towerType = type;

        //��ư �ߺ� ������ ����
        if (isOnTowerButton)
            return;

        //Ÿ���� �Ǽ��� ���� ������ �ý��� �޽��� ���
        if (towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        isOnTowerButton = true;
        //���콺�� ����ٴϴ� Ÿ�� ����
        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);
        //Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ�
        StartCoroutine("OnTowerCancelSystem");
    }
    //---------- Ÿ�� ��ġ --------------
    public void SpawnTower(Transform tileTransform)
    {
        //Ÿ�� �Ǽ� ��ư�� ������ ���� �Ǽ� ����
        if (!isOnTowerButton)
            return;

        //Ÿ�� ���� �ҷ�����
        Tile tile = tileTransform.GetComponent<Tile>();
        //�̹� Ÿ�Ͽ� Ÿ���� �ִٸ� �ý��� �޽��� ���
        if (tile.IsBulidTower == true)
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }
        //�ٽ� Ÿ�� �Ǽ� ��ư�� ������ Ÿ���� �Ǽ��ϵ��� ���� ����
        isOnTowerButton = false;
        //Ÿ�� �Ǽ� Ȯ��
        tile.IsBulidTower = true;
        //�÷��̾� ��� ����
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        //������ Ÿ�� ��ġ�� Ÿ�� �Ǽ� (Ÿ�Ϻ��� z�� -1 ��ġ�� ��ġ) (Ÿ���� �켱���õ�)
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
        //������ Ÿ�� ���⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().SetUp(enemySpawner, playerGold, tile);

        //Ÿ�� ��ġ�� �Ϸ�Ǹ� �ӽ� Ÿ�� ����
        Destroy(followTowerClone);
        //Ÿ�� �Ǽ� ����� �� �ִ� �ڷ�ƾ �Լ� ����
        StopCoroutine("OnTowerCancelSystem");
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            //ECS or ���콺 ��Ŭ �ϸ� Ÿ�� �Ǽ� ���
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                Destroy(followTowerClone);
                break;
            }

            yield return null;
        }
    }
}
