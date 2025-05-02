using UnityEngine;
using TMPro;
public class TextTMPViewer : MonoBehaviour
{
    //HP UI
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;
    [SerializeField]
    private PlayerHP playerHP;

    //Gold UI
    [SerializeField]
    private TextMeshProUGUI textPlayerGold;
    [SerializeField]
    private PlayerGold playerGold;

    //-----------플레이어 현재 체력 & 골드 표시-------------
    private void Update()
    {
        //HP 표시
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;

        //Gold 표시
        textPlayerGold.text = playerGold.CurrentGold.ToString();
    }
}
