using UnityEngine;

//Ÿ�� ���� Ŭ����
[CreateAssetMenu]
public class TowerTemplate : ScriptableObject //(���� ������ ����)
{
    public GameObject towerPrefab;  //Ÿ�� ������
    public Weapon[] weapon;         //Ÿ�� ���� ����ü


    //----------------Ÿ�� ���� ����ü ---------------
    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;       //Ÿ�� �̹���
        public float damage;        //Ÿ�� ���ݷ�
        public float rate;          //Ÿ�� ���ݼӵ�
        public float range;         //Ÿ�� ���ݹ���
        public int cost;            //�ʿ� ��� (0���� : �Ǽ�, 1~���� : ���׷��̵�)
    }
}
