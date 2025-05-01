using UnityEngine;

//타워 중복 방지 코드
public class Tile : MonoBehaviour
{
    public bool IsBulidTower { set; get;}

    private void Awake()
    {
        IsBulidTower = false;
    }
}
