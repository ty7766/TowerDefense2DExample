using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private EnemySpawner enemySpawner;  //현재 맵에 존재하는 적 리스트 정보 얻기

    public void SpawnTower(Transform tileTransform)
    {
        //타일 정보 불러오기
        Tile tile = tileTransform.GetComponent<Tile>();
        //이미 타일에 타워가 있다면 반응 무시
        if (tile.IsBulidTower == true)
            return;

        tile.IsBulidTower = true;
        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        //생성된 타워 무기에 enemySpawner 정보 전달
        clone.GetComponent<TowerWeapon>().SetUp(enemySpawner);
    }
}
