using UnityEngine;


//-----적의 체력바 UI-----
public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f;        //적 <-> UI 거리
    private Transform targetTransform;                      //적 오브젝트
    private RectTransform rectTransform;                    //UI위치 정보 제어

    public void SetUp(Transform target)
    {
        targetTransform = target;
        rectTransform = GetComponent<RectTransform>();
    }

    //적의 위치가 갱신된 이후에 UI를 적용시키기 위해서 LateUpdate() 사용
    private void LateUpdate()
    {
        //쫓을 적이 없으면 UI도 삭제
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        //월드좌표를 기준으로 적 위치를 가져오고 UI를 distance만큼 떨어지게 부착
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position = screenPosition + distance;
    }
}
