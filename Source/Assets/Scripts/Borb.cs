using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borb : MonoBehaviour
{
    public float deathtimer = 10f;
    public Sprite bs;
    public Sprite rs;

    // Update is called once per frame
    void FixedUpdate()
    {
        deathtimer -= Time.deltaTime;
        if (deathtimer <= 0)
            Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D hitinfo)
    {
        if (hitinfo.GetComponent<Shooting>() != null)
        {
            hitinfo.GetComponent<Shooting>().enabled = true;
            if (hitinfo.GetComponent<Melee>() != null)
            {
                hitinfo.GetComponent<Melee>().enabled = false;
            }
        }
        if (hitinfo.name == "Spaceship" )
        {
            hitinfo.GetComponent<SpriteRenderer>().sprite = bs;
        }
        else if (hitinfo.name == "EnemySpaceship")
        {
            hitinfo.GetComponent<SpriteRenderer>().sprite = rs;
        }
        Destroy(gameObject);
    }
}
