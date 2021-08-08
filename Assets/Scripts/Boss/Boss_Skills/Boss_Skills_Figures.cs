using UnityEngine;

public class Boss_Skills_Figures : MonoBehaviour
{
    public Player player;
    protected int damage;
    void Start()
    {
        player = FindObjectOfType<Player>();
    }
}
