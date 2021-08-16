using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Magician_Camera : MonoBehaviour
{
    public Boss_Magician boss;

    private void Update()
    {
        if (boss.dead)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(0,this.transform.position.y,this.transform.position.z), 2 * Time.deltaTime);
        }
    }
}
