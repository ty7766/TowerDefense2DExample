using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private EnemySpawner enemySpawner;  //���� �ʿ� �����ϴ� �� ����Ʈ ���� ���

    [SerializeField]
    private int towerBuildGold = 50;    //Ÿ�� �Ǽ���
    [SerializeField]
    private PlayerGold playerGold;      //�÷��̾� ��忡�� Ÿ�� �Ǽ��� ������

    //---------- Ÿ�� ��ġ --------------
    public void SpawnTower(Transform tileTransform)
    {
        //Ÿ���� �Ǽ��� ���� ������ ���� ����
        if (towerBuildGold > playerGold.CurrentGold)
            return;

        //Ÿ�� ���� �ҷ�����
        Tile tile = tileTransform.GetComponent<Tile>();
        //�̹� Ÿ�Ͽ� Ÿ���� �ִٸ� ���� ����
        if (tile.IsBulidTower == true)
            return;

        //Ÿ�� �Ǽ� Ȯ��
        tile.IsBulidTower = true;
        //�÷��̾� ��� ����
        playerGold.CurrentGold -= towerBuildGold;
        //������ Ÿ�� ��ġ�� Ÿ�� �Ǽ�
        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        //������ Ÿ�� ���⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().SetUp(enemySpawner);
    }
}
