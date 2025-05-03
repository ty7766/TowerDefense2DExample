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
        //MainCamera �±׸� ������ �ִ� ������Ʈ ã�� -> Camera�� ����
        mainCamera = Camera.main;
    }

    /* �˰���
     * Ÿ���� 3D�� ����
     * ī�޶󿡼� �� ���콺 �����͸� �����ϴ� ����(ȭ���� �������� �����Ѵ�) ����
     * 1. ���콺 ���ʹ�ư ������
     * 2. ī�޶� -> ���콺 ������ �� �����ϴ� ����(ȭ���� �������� �մ�) �߻�
     * 3. Tile�� ������ ���˵Ǹ� �ش� Tile�� Tower�� ����
     * */
    private void Update()
    {
        //���콺 ���ʹ�ư ������ ��
        if (Input.GetMouseButton(0))
        {
            //ī�޶� ��ġ�������� ȭ���� ���콺 ��ġ������ �����ϴ� ���� ����
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //������ ���̸� �ִ�� �ø��� ������ ���ߵ� ������Ʈ ���� -> hit�� ����
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //Ÿ���� �����ϸ� �ش� Ÿ�� ���� ���
                if (hit.transform.CompareTag("Tower"))
                {
                    Debug.Log("Ÿ�� Ŭ����: " + hit.transform.name);
                    towerDataViewer.OnPanel(hit.transform);
                }

                //���ߵ� ������Ʈ �±װ� "Tile" �̸� 
                if (hit.transform.CompareTag("Tile"))
                {
                    Debug.Log("Ÿ�� Ŭ����: ");
                    towerSpawner.SpawnTower(hit.transform);
                }
            }
        }

    }
}
