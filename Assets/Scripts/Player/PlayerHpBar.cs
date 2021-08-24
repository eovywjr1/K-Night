using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    [SerializeField]
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        this.gameObject.GetComponent<Slider>().value = player.hp / 100.0f;
    }
}
