using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Skills : MonoBehaviour
{
    public Player player;
    protected int damage;
    void Start()
    {
        player = FindObjectOfType<Player>();
    }
}
