using UnityEngine;

//Ÿ�� ���� Ŭ����
[CreateAssetMenu]
public class TowerTemplate : ScriptableObject //(���� ������ ����)
{
    public GameObject towerPrefab;  //Ÿ�� ������

    //Ÿ�� �Ǽ��� Ŭ������ �� ����ڿ��� �� Ŭ���� �Ǿ��ִ��� Ȯ���� ���� ������
    public GameObject followTowerPrefab;        //�ӽ� Ÿ�� ������
    public Weapon[] weapon;         //Ÿ�� ���� ����ü


    //----------------Ÿ�� ���� ����ü ---------------
    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;       //Ÿ�� �̹���
        public float damage;        //Ÿ�� ���ݷ�
        public float rate;          //Ÿ�� ���ݼӵ�
        public float range;         //Ÿ�� ���ݹ���
        public float slow;          //���� ��ġ (0.2 = 20%) (���ο�Ÿ�� ����)
        public int cost;            //�ʿ� ��� (0���� : �Ǽ�, 1~���� : ���׷��̵�)
        public int sell;            //Ÿ�� �Ǹ� �ݾ�
    }
}
