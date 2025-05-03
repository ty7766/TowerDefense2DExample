using UnityEngine;

//타워 정보 클래스
[CreateAssetMenu]
public class TowerTemplate : ScriptableObject //(에셋 생성을 위함)
{
    public GameObject towerPrefab;  //타워 프리팹
    public Weapon[] weapon;         //타워 정보 구조체


    //----------------타워 정보 구조체 ---------------
    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;       //타워 이미지
        public float damage;        //타워 공격력
        public float rate;          //타워 공격속도
        public float range;         //타워 공격범위
        public int cost;            //필요 골드 (0레벨 : 건설, 1~레벨 : 업그레이드)
    }
}
