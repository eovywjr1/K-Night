using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnergyBall : Boss_Skills_Figures
{
    private float throwSpeed;
    private Vector3 direction;
    private Rigidbody2D rigid;

    private void Awake()
    {
        damage = GameObject.Find("Boss").GetComponent<Boss_form>().damage_EnergyBall;
        throwSpeed = GameObject.Find("Boss").GetComponent<Boss_form>().energyBallSpeed;
        direction = GameObject.Find("Boss").GetComponent<Boss_form>().direction;
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        StartCoroutine(nameof(Throw));
    }
    IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.5f);
        rigid.AddForce(direction * throwSpeed, ForceMode2D.Impulse);
    }

    //플레이어에게 데미지
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Player"){
            Debug.Log("에너지 볼 맞음");
            player.HpDecrease(damage);
        }
        if (collision.gameObject.tag == "Wall"){
            if (SceneManager.GetActiveScene().name == nameof(Boss_Spider_Reprise)){
                Debug.Log("벽에 튕김");
                rigid.velocity *= -1;
            }
            else if(SceneManager.GetActiveScene().name == nameof(Boss_Magician))
            {
                Debug.Log("벽에 튕김");
                Destroy(gameObject);
            }
        }
    }
}
