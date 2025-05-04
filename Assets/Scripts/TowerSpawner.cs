using UnityEngine;
using System.Collections;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;          //타워 정보
    [SerializeField]
    private EnemySpawner enemySpawner;              //현재 맵에 존재하는 적 리스트 정보 얻기
    [SerializeField]
    private PlayerGold playerGold;                  //플레이어 골드에서 타워 건설비 차감용
    [SerializeField]
    private SystemTextViewer systemTextViewer;      //돈 부족, 건설 불가 와 같은 메세지 출력

    private bool isOnTowerButton = false;           //타워 건설 버튼 체크
    private GameObject followTowerClone = null;     //임시 타워 사용 완료시 삭제를 위한 변수
    private int towerType;                          //타워 속성

    //--------------- 타워 설치 체크 ---------------
    public void ReadyToSpawnTower(int type)
    {
        towerType = type;

        //버튼 중복 누르기 방지
        if (isOnTowerButton)
            return;

        //타워를 건설할 돈이 없으면 시스템 메시지 출력
        if (towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        isOnTowerButton = true;
        //마우스를 따라다니는 타워 생성
        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);
        //타워 건설을 취소할 수 있는 코루틴 함수
        StartCoroutine("OnTowerCancelSystem");
    }
    //---------- 타워 설치 --------------
    public void SpawnTower(Transform tileTransform)
    {
        //타워 건설 버튼을 눌렀을 때만 건설 가능
        if (!isOnTowerButton)
            return;

        //타일 정보 불러오기
        Tile tile = tileTransform.GetComponent<Tile>();
        //이미 타일에 타워가 있다면 시스템 메시지 출력
        if (tile.IsBulidTower == true)
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }
        //다시 타워 건설 버튼을 눌러서 타워를 건설하도록 변수 설정
        isOnTowerButton = false;
        //타워 건설 확정
        tile.IsBulidTower = true;
        //플레이어 골드 차감
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        //선택한 타일 위치에 타워 건설 (타일보다 z축 -1 위치에 배치) (타워가 우선선택됨)
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
        //생성된 타워 무기에 enemySpawner 정보 전달
        clone.GetComponent<TowerWeapon>().SetUp(enemySpawner, playerGold, tile);

        //타워 배치가 완료되면 임시 타워 삭제
        Destroy(followTowerClone);
        //타워 건설 취소할 수 있는 코루틴 함수 중지
        StopCoroutine("OnTowerCancelSystem");
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            //ECS or 마우스 우클 하면 타워 건설 취소
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
