using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    [SerializeField]
    private int currentGold = 100;  //¼ÒÁö±Ý

    public int CurrentGold
    {
        set => currentGold = Mathf.Max(0,value);
        get => currentGold;
    }
}
