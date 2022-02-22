using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public float speed = 15f;
    public float deathtimer = 5f;
    public float dmgamount = 10f;
    private Rigidbody2D rg;
    //Animator anim;
    //public GameObject impacteffect;
    void Start()
    {
        rg = gameObject.GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        if (gameObject.name.Contains("Blue"))
        {
            rg.velocity = transform.up * speed;
        }
        else
        {
            rg.velocity = -1*transform.up * speed;
        }

    }
    void FixedUpdate()
    {
        deathtimer -= Time.deltaTime;
        if (deathtimer <= 0)
            destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject temp = col.gameObject;
        //Impactphysics.impact(hitinfo, gameObject, dmgamount);
        //Debug.Log("temp.tag " + temp.tag);
        Debug.Log("gameObject.name " + gameObject.name);
        Debug.Log("temp.name " + temp.name);
        Debug.Log(gameObject.name == temp.name);
        //Debug.Log(!gameObject.name.Contains(temp.tag));
        if (gameObject.name.Contains(temp.tag) || (gameObject.name == temp.name)) { }
        else
        {
            if (col.GetComponent<Life>() != null)
            {
                col.GetComponent<Life>().applyshotdmg();
                destroy(gameObject);
            }
            else if (col.GetComponent<MeteorScript>() != null)
            {
                col.GetComponent<MeteorScript>().applyshotdmg();
                destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    void destroy(GameObject tempobject)
    {
        //Instantiate(impacteffect, tempobject.transform.position, tempobject.transform.rotation);
        Destroy(tempobject);
    }
}
