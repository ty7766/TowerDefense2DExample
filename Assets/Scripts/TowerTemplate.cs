using UnityEngine;

//타워 정보 클래스
[CreateAssetMenu]
public class TowerTemplate : ScriptableObject //(에셋 생성을 위함)
{
    public GameObject towerPrefab;  //타워 프리팹

    //타워 건설을 클릭했을 때 사용자에게 잘 클릭이 되어있는지 확인을 위한 프리팹
    public GameObject followTowerPrefab;        //임시 타워 프리팹
    public Weapon[] weapon;         //타워 정보 구조체


    //----------------타워 정보 구조체 ---------------
    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;       //타워 이미지
        public float damage;        //타워 공격력
        public float rate;          //타워 공격속도
        public float range;         //타워 공격범위
        public float slow;          //감속 수치 (0.2 = 20%) (슬로우타워 전용)
        public int cost;            //필요 골드 (0레벨 : 건설, 1~레벨 : 업그레이드)
        public int sell;            //타워 판매 금액
    }
}
