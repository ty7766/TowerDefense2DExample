using UnityEngine;
using TMPro;
public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;
    [SerializeField]
    private PlayerHP playerHP;

    //�÷��̾� ���� ü�� ǥ��
    private void Update()
    {
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
    }
}
