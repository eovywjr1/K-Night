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


    public bool talkIsEnd = false;
    void Start()
    {
        Time.timeScale = 1;
        moveVelocity = Vector3.right;
        once = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (talkIsEnd)
            transform.position += moveVelocity * dashSpeed * Time.deltaTime;

        //지팡이까지 이동하면
        if (transform.position.x > 0 && !once)
        {
            once = true;
            dashSpeed = 0f;
            //지팡이를 줍고
            GameObject staff = GameObject.Find("Staff");
            staff.transform.eulerAngles = Vector3.zero;
            //대사를 하고
            GameObject talkStart2 = GameObject.Find("TalkParent").transform.Find("TalkStart2").gameObject;
            talkStart2.SetActive(true);

            //씬 전환
        }
    }
}
