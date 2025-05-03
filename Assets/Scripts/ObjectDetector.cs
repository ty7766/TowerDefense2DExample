using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;  //���콺 Ŭ���� ������Ʈ ����

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
        //���콺�� UI�� �ӹ��� ���� ���� �Ʒ� �ڵ尡 ������� �ʵ��� ��
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        //���콺 ���ʹ�ư ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            //ī�޶� ��ġ�������� ȭ���� ���콺 ��ġ������ �����ϴ� ���� ����
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //������ ���̸� �ִ�� �ø��� ������ ���ߵ� ������Ʈ ���� -> hit�� ����
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;

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
        else if (Input.GetMouseButtonUp(0)) 
        { 
            //���콺�� ������ �� ������ ������Ʈ�� ���ų� ������ ������Ʈ Ÿ���� �ƴϸ� �г� ��Ȱ��
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                towerDataViewer.OffPanel();
            }
            hitTransform = null;
        }

    }
}
