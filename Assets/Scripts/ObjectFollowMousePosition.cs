using UnityEngine;

public class ObjectFollowMousePosition : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //화면 마우스 좌표 기준, 게임 월드 좌표 구하기
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        transform.position = mainCamera.ScreenToWorldPoint(position);

        //z위치를 0으로 설정
        //현재 선택한 타워 오브젝트가 마우스를 따라다니도록 설정
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
