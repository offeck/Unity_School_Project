using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBlade : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject temp = col.gameObject;
        Debug.Log("temp.tag " + temp.tag);
        Debug.Log("gameObject.name " + gameObject.name);
        Debug.Log(gameObject.name.Contains(temp.tag));
        if (!gameObject.name.Contains(temp.tag) && temp.GetComponent<Life>() != null)
        {
            Soundmanagerscript.playsound("clash");// sound
            if (col.GetComponent<Life>() != null)
            {
                col.GetComponent<Life>().applymeleedmg();
            }
            else if (col.GetComponent<MeteorScript>() != null)
            {
                col.GetComponent<MeteorScript>().applymeleedmg();
            }
        }
    }
}
