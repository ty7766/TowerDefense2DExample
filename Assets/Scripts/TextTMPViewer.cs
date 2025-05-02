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

    //-----------�÷��̾� ���� ü�� & ��� ǥ��-------------
    private void Update()
    {
        //HP ǥ��
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;

        //Gold ǥ��
        textPlayerGold.text = playerGold.CurrentGold.ToString();
    }
}
