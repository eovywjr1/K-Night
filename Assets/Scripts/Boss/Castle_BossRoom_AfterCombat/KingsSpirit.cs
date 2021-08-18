using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingsSpirit : MonoBehaviour
{
    public Amulet amulet;
    bool animationEnd = false;
    bool resurrection = false;
    void Update()
    {
        if(amulet.gameObject.activeSelf == false && amulet.getAmulet)
        {
            this.transform.position = Vector3.Lerp(this.transform.position,Vector3.down*3f, 0.5f * Time.deltaTime);
        }
        if (this.transform.position.y < -2.5)
        {
            animationEnd = true;
        }
        if(animationEnd)
        {
            //¿Õ ÀÏ¾î³ª!
            GameObject magician = GameObject.Find("Magician_Dead");
            if (magician.transform.eulerAngles.z >= 1f)
            {
                Debug.Log("¾È¿òÁ÷ÀÌ³Ä?");
                magician.transform.Rotate(Vector3.back * Time.deltaTime * 50);
                magician.transform.position += new Vector3(0, 0.001f, 0);
            }
            //´ë»ç ÄÑ
            GameObject talkStart2 = GameObject.Find("TalkParent").transform.Find("TalkStart2").gameObject;
            talkStart2.SetActive(true);
            //¿µÈ¥ ²¨
            this.gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -0.01f);
        }
        if(this.gameObject.GetComponent<SpriteRenderer>().color.a == 0)
        {
            resurrection = true;
        }
        if (resurrection)
        {
            this.gameObject.SetActive(false);
        }
    }
}
