using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        //MainCamera 태그를 가지고 있는 오브젝트 찾기 -> Camera로 전달
        mainCamera = Camera.main;
    }

    /* 알고리즘
     * 타일을 3D로 설정
     * 카메라에서 내 마우스 포인터를 관통하는 광선(화면을 수직으로 관통한다) 생성
     * 1. 마우스 왼쪽버튼 누르면
     * 2. 카메라 -> 마우스 포인터 를 관통하는 광선(화면을 수직으로 뚫는) 발사
     * 3. Tile과 광선이 접촉되면 해당 Tile에 Tower를 생성
     * */
    private void Update()
    {
        //마우스 왼쪽버튼 눌렀을 때
        if (Input.GetMouseButton(0))
        {
            //카메라 위치에서부터 화면의 마우스 위치까지를 관통하는 광선 생성
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //광선의 길이를 최대로 늘리고 광선에 적중된 오브젝트 검출 -> hit에 저장
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //타워를 선택하면 해당 타워 정보 출력
                if (hit.transform.CompareTag("Tower"))
                {
                    Debug.Log("타워 클릭됨: " + hit.transform.name);
                    towerDataViewer.OnPanel(hit.transform);
                }

                //적중된 오브젝트 태그가 "Tile" 이면 
                if (hit.transform.CompareTag("Tile"))
                {
                    Debug.Log("타일 클릭됨: ");
                    towerSpawner.SpawnTower(hit.transform);
                }
            }
        }

    }
}
