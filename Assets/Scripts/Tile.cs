using UnityEngine;

//Ÿ�� �ߺ� ���� �ڵ�
public class Tile : MonoBehaviour
{
    public bool IsBulidTower { set; get;}

    private void Awake()
    {
        IsBulidTower = false;
    }
}
