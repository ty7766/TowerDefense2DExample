using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;   //���� ���������� ��� ���̺� ����
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1;

    //���̺� ���� ��� ������Ƽ
    public int CurrentWave => currentWaveIndex + 1; //���� ���̺� (������ 0�̱⶧���� + 1)
    public int MaxWave => waves.Length;


    //------------ ���̺� ���� ���� -------------
    public void StartWave()
    {
        //���� �ʿ� ���� ����, ���� ���̺갡 ������
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
            //���� ���̺� ���� ����
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
}
}
//------------ ���̺� ����ü -----------
[System.Serializable]
public struct Wave
{
    public float spawnTime;             //���� ���̺� �� ���� �ֱ�
    public int maxEnemyCount;           //���� ���̺� �� ����
    public GameObject[] enemyPrefabs;   //���� ���̺� �� ����
}
