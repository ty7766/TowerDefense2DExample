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

    //Wave UI
    [SerializeField]
    private TextMeshProUGUI textWave;
    [SerializeField]
    private WaveSystem waveSystem;

    //EnemyCount UI
    [SerializeField]
    private TextMeshProUGUI textEnemyCount;
    [SerializeField]
    private EnemySpawner enemySpawner;

    //-----------�÷��̾� UI ǥ��-------------
    private void Update()
    {
        //HP ǥ��
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;

        //Gold ǥ��
        textPlayerGold.text = playerGold.CurrentGold.ToString();

        //Wave ǥ��
        textWave.text = waveSystem.CurrentWave + "/" + waveSystem.MaxWave;

        //EnemyCount ǥ��
        textEnemyCount.text = enemySpawner.CurrentEnemyCount + "/" + enemySpawner.MaxEnemyCount;
    }
}
