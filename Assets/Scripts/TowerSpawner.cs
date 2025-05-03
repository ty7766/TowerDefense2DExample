using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate towerTemplate;    //Ÿ�� ����

    [SerializeField]
    private EnemySpawner enemySpawner;  //���� �ʿ� �����ϴ� �� ����Ʈ ���� ���

    [SerializeField]
    private PlayerGold playerGold;      //�÷��̾� ��忡�� Ÿ�� �Ǽ��� ������

    [SerializeField]
    private SystemTextViewer systemTextViewer;      //�� ����, �Ǽ� �Ұ� �� ���� �޼��� ���

    //---------- Ÿ�� ��ġ --------------
    public void SpawnTower(Transform tileTransform)
    {
        //Ÿ���� �Ǽ��� ���� ������ �ý��� �޽��� ���
        if (towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        //Ÿ�� ���� �ҷ�����
        Tile tile = tileTransform.GetComponent<Tile>();
        //�̹� Ÿ�Ͽ� Ÿ���� �ִٸ� �ý��� �޽��� ���
        if (tile.IsBulidTower == true)
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        //Ÿ�� �Ǽ� Ȯ��
        tile.IsBulidTower = true;
        //�÷��̾� ��� ����
        playerGold.CurrentGold -= towerTemplate.weapon[0].cost;
        //������ Ÿ�� ��ġ�� Ÿ�� �Ǽ� (Ÿ�Ϻ��� z�� -1 ��ġ�� ��ġ) (Ÿ���� �켱���õ�)
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate.towerPrefab, position, Quaternion.identity);
        //������ Ÿ�� ���⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().SetUp(enemySpawner, playerGold, tile);
    }
}
