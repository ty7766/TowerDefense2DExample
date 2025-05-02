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

    //-----------플레이어 UI 표시-------------
    private void Update()
    {
        //HP 표시
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;

        //Gold 표시
        textPlayerGold.text = playerGold.CurrentGold.ToString();

        //Wave 표시
        textWave.text = waveSystem.CurrentWave + "/" + waveSystem.MaxWave;

        //EnemyCount 표시
        textEnemyCount.text = enemySpawner.CurrentEnemyCount + "/" + enemySpawner.MaxEnemyCount;
    }
}
