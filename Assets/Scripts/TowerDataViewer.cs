using UnityEngine;
using UnityEngine.UI;
using TMPro;
//타워 정보 UI
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

    //----------------- 타워 정보 보이기 --------------------
    public void OnPanel(Transform towerWeapon)
    {
        //출력해야하는 타워 정보 갱신
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        gameObject.SetActive(true);
        UpdateTowerData();

        //크기 조정한 공격 범위 이미지를 보여주기
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }

    //-------------- 타워 정보 숨기기 ------------------
    public void OffPanel()
    {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }

    //----------- 출력 텍스트 ----------------
    private void UpdateTowerData()
    {
        imageTower.sprite = currentTower.TowerSprite;
        textDamage.text = "Damage : " + currentTower.Damage;
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;

        //타워 레벨이 최대가 되면 업그레이드 버튼 비활성화
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnClickEventTowerUpgrade()
    {
        //타워 업그레이드가 되었는지 확인
        bool isSuccess = currentTower.Upgrade();

        if (isSuccess)
        {
            //타워 정보 및 범위 이미지 갱신
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
        //타워 판매 및 타워의 UI 삭제
        currentTower.Sell();
        OffPanel();
    }
}
