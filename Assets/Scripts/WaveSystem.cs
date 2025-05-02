using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;   //현재 스테이지의 모든 웨이브 정보
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1;

    //웨이브 정보 출력 프로퍼티
    public int CurrentWave => currentWaveIndex + 1; //현재 웨이브 (시작이 0이기때문에 + 1)
    public int MaxWave => waves.Length;


    //------------ 웨이브 정보 제공 -------------
    public void StartWave()
    {
        //현재 맵에 적이 없고, 남은 웨이브가 있으면
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
            //현재 웨이브 정보 제공
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
}
}
//------------ 웨이브 구조체 -----------
[System.Serializable]
public struct Wave
{
    public float spawnTime;             //현재 웨이브 적 생성 주기
    public int maxEnemyCount;           //현재 웨이브 적 개수
    public GameObject[] enemyPrefabs;   //현재 웨이브 적 종류
}
