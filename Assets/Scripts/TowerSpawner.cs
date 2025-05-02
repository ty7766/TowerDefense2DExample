using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private EnemySpawner enemySpawner;  //���� �ʿ� �����ϴ� �� ����Ʈ ���� ���

    public void SpawnTower(Transform tileTransform)
    {
        //Ÿ�� ���� �ҷ�����
        Tile tile = tileTransform.GetComponent<Tile>();
        //�̹� Ÿ�Ͽ� Ÿ���� �ִٸ� ���� ����
        if (tile.IsBulidTower == true)
            return;

        tile.IsBulidTower = true;
        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        //������ Ÿ�� ���⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().SetUp(enemySpawner);
    }
}
