using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    public void SpawnTower(Transform tileTransform)
    {
        //타일 정보 불러오기
        Tile tile = tileTransform.GetComponent<Tile>();
        //이미 타일에 타워가 있다면 반응 무시
        if (tile.IsBulidTower == true)
            return;

        tile.IsBulidTower = true;

        //Instantiate로 towerPrefab을 tile의 위치에 생성
        Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
    }
}
