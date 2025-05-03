using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate towerTemplate;    //Ÿ�� ����

    [SerializeField]
    private EnemySpawner enemySpawner;  //���� �ʿ� �����ϴ� �� ����Ʈ ���� ���

    [SerializeField]
    private PlayerGold playerGold;      //�÷��̾� ��忡�� Ÿ�� �Ǽ��� ������

    //---------- Ÿ�� ��ġ --------------
    public void SpawnTower(Transform tileTransform)
    {
        //Ÿ���� �Ǽ��� ���� ������ ���� ����
        if (towerTemplate.weapon[0].cost > playerGold.CurrentGold)
            return;

        //Ÿ�� ���� �ҷ�����
        Tile tile = tileTransform.GetComponent<Tile>();
        //�̹� Ÿ�Ͽ� Ÿ���� �ִٸ� ���� ����
        if (tile.IsBulidTower == true)
            return;

        //Ÿ�� �Ǽ� Ȯ��
        tile.IsBulidTower = true;
        //�÷��̾� ��� ����
        playerGold.CurrentGold -= towerTemplate.weapon[0].cost;
        //������ Ÿ�� ��ġ�� Ÿ�� �Ǽ� (Ÿ�Ϻ��� z�� -1 ��ġ�� ��ġ) (Ÿ���� �켱���õ�)
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate.towerPrefab, tileTransform.position, Quaternion.identity);
        //������ Ÿ�� ���⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().SetUp(enemySpawner, playerGold);
    }
}
