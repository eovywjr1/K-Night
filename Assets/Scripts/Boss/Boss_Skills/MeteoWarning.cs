using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoWarning : MonoBehaviour
{
    private float warningTime;
    private void Awake()
    {
        warningTime = GameObject.Find("Boss").GetComponent<Boss_Magician>().warningTime;
    }
    void Start()
    {
        Destroy(gameObject, warningTime);
    }
}
