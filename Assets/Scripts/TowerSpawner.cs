using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate towerTemplate;    //타워 정보

    [SerializeField]
    private EnemySpawner enemySpawner;  //현재 맵에 존재하는 적 리스트 정보 얻기

    [SerializeField]
    private PlayerGold playerGold;      //플레이어 골드에서 타워 건설비 차감용

    //---------- 타워 설치 --------------
    public void SpawnTower(Transform tileTransform)
    {
        //타워를 건설할 돈이 없으면 반응 무시
        if (towerTemplate.weapon[0].cost > playerGold.CurrentGold)
            return;

        //타일 정보 불러오기
        Tile tile = tileTransform.GetComponent<Tile>();
        //이미 타일에 타워가 있다면 반응 무시
        if (tile.IsBulidTower == true)
            return;

        //타워 건설 확정
        tile.IsBulidTower = true;
        //플레이어 골드 차감
        playerGold.CurrentGold -= towerTemplate.weapon[0].cost;
        //선택한 타일 위치에 타워 건설 (타일보다 z축 -1 위치에 배치) (타워가 우선선택됨)
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate.towerPrefab, tileTransform.position, Quaternion.identity);
        //생성된 타워 무기에 enemySpawner 정보 전달
        clone.GetComponent<TowerWeapon>().SetUp(enemySpawner, playerGold);
    }
}
