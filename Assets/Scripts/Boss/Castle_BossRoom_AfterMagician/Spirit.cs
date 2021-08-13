using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    private Vector3 moveVelocity;
    public Sprite spirit;
    public Sprite king;
    public float dashSpeed;
    public float changeDelay;
    private bool once;
    void Start()
    {
        moveVelocity = Vector3.right;
        once = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (/*대사가 끝나면*/true)
            transform.position += moveVelocity * dashSpeed * Time.deltaTime;

        //지팡이까지 이동하면
        if (transform.position.x > 0 && !once)
        {
            once = true;
            dashSpeed = 0f;
            //지팡이를 줍고
            GameObject staff = GameObject.Find("Staff");
            staff.transform.eulerAngles = Vector3.zero;
            //모습이 변하고
            ChangeToKing();
            //대사를 하고

            //씬 전환
        }
    }
    void ChangeToKing()
    {
        Invoke(nameof(King), changeDelay);
    }
    void King()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = king;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(123f / 255f, 160f / 255f, 248f / 255f, 1f);
        gameObject.transform.localScale = new Vector3(2, 2, 0);
    }

}
