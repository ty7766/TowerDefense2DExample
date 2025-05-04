using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Ÿ�� ���� UI
public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private Button buttonUpgrade;
    [SerializeField]
    private SystemTextViewer systemTextViewer;

    private TowerWeapon currentTower;

    private void Awake()
    {
        OffPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    //----------------- Ÿ�� ���� ���̱� --------------------
    public void OnPanel(Transform towerWeapon)
    {
        //����ؾ��ϴ� Ÿ�� ���� ����
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        gameObject.SetActive(true);
        UpdateTowerData();

        //ũ�� ������ ���� ���� �̹����� �����ֱ�
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }

    //-------------- Ÿ�� ���� ����� ------------------
    public void OffPanel()
    {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }

    //----------- ��� �ؽ�Ʈ ----------------
    private void UpdateTowerData()
    {
        //Ÿ�� ���� ����ϴ� ��ġ�� �ٸ��� �ϱ�
        if (currentTower.WeaponType == WeaponType.Cannon || currentTower.WeaponType == WeaponType.Laser)
        {
            imageTower.rectTransform.sizeDelta = new Vector2(88, 59);
            textDamage.text = "Damage : " + currentTower.Damage;
        }
        else
        {
            imageTower.rectTransform.sizeDelta = new Vector2(59, 59);
            textDamage.text = "Slow : " + currentTower.Slow * 100 + "%";
        }

        imageTower.sprite = currentTower.TowerSprite;
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;

        //Ÿ�� ������ �ִ밡 �Ǹ� ���׷��̵� ��ư ��Ȱ��ȭ
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnClickEventTowerUpgrade()
    {
        //Ÿ�� ���׷��̵尡 �Ǿ����� Ȯ��
        bool isSuccess = currentTower.Upgrade();

        if (isSuccess)
        {
            //Ÿ�� ���� �� ���� �̹��� ����
            UpdateTowerData();
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

    public void OnClickEventTowerSell()
    {
        //Ÿ�� �Ǹ� �� Ÿ���� UI ����
        currentTower.Sell();
        OffPanel();
    }
}
