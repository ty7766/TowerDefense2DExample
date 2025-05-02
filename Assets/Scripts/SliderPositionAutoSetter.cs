using UnityEngine;


//-----���� ü�¹� UI-----
public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f;        //�� <-> UI �Ÿ�
    private Transform targetTransform;                      //�� ������Ʈ
    private RectTransform rectTransform;                    //UI��ġ ���� ����

    public void SetUp(Transform target)
    {
        targetTransform = target;
        rectTransform = GetComponent<RectTransform>();
    }

    //���� ��ġ�� ���ŵ� ���Ŀ� UI�� �����Ű�� ���ؼ� LateUpdate() ���
    private void LateUpdate()
    {
        //���� ���� ������ UI�� ����
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        //������ǥ�� �������� �� ��ġ�� �������� UI�� distance��ŭ �������� ����
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position = screenPosition + distance;
    }
}
