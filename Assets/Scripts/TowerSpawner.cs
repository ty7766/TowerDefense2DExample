using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    public void SpawnTower(Transform tileTransform)
    {
        //Ÿ�� ���� �ҷ�����
        Tile tile = tileTransform.GetComponent<Tile>();
        //�̹� Ÿ�Ͽ� Ÿ���� �ִٸ� ���� ����
        if (tile.IsBulidTower == true)
            return;

        tile.IsBulidTower = true;

        //Instantiate�� towerPrefab�� tile�� ��ġ�� ����
        Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
    }
}
