using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private EnemySpawner enemySpawner;  //현재 맵에 존재하는 적 리스트 정보 얻기

    [SerializeField]
    private int towerBuildGold = 50;    //타워 건설비
    [SerializeField]
    private PlayerGold playerGold;      //플레이어 골드에서 타워 건설비 차감용

    //---------- 타워 설치 --------------
    public void SpawnTower(Transform tileTransform)
    {
        //타워를 건설할 돈이 없으면 반응 무시
        if (towerBuildGold > playerGold.CurrentGold)
            return;

        //타일 정보 불러오기
        Tile tile = tileTransform.GetComponent<Tile>();
        //이미 타일에 타워가 있다면 반응 무시
        if (tile.IsBulidTower == true)
            return;

        //타워 건설 확정
        tile.IsBulidTower = true;
        //플레이어 골드 차감
        playerGold.CurrentGold -= towerBuildGold;
        //선택한 타일 위치에 타워 건설
        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        //생성된 타워 무기에 enemySpawner 정보 전달
        clone.GetComponent<TowerWeapon>().SetUp(enemySpawner);
    }
}
