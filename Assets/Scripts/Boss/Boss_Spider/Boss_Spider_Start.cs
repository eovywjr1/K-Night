using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Spider_Start : MonoBehaviour
{
    [SerializeField]
    private float flyingSpeed;
    private Rigidbody2D rigid;
    public GameObject realboss;
    public int damage_Stone;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        damage_Stone = realboss.GetComponent<Boss_form>().damage_Stone;
        Vector3 vec = new Vector3(1, 1, 0);
        rigid.AddForce(vec * flyingSpeed, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //¶¥(Ground¿Í Á¢ÃË½Ã)
        if (collision.CompareTag("Ground"))
        {
            this.gameObject.SetActive(false);
            realboss.SetActive(true);
        }
    }
}
     