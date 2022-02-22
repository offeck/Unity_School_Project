using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    private Rigidbody2D rg;
    public int speed;
    private int hp;
    private float meleetimer = 0;
    private int[] dmgarr = { 25, 20, 15, 10 };

    // Start is called before the first frame update
    public void startmoving()
    {
        hp = 75 / speed;
        float lscale = 1f / speed;
        gameObject.transform.localScale = new Vector3(lscale, lscale, 1);
        rg = gameObject.GetComponent<Rigidbody2D>();
        rg.velocity = speed * gameObject.transform.right*-1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject temp = collision.gameObject;
        if (temp.GetComponent<Life>() != null)
        {
            temp.GetComponent<Life>().applymeteordmg(dmgarr[speed-1]);
            if(temp.GetComponent<Life>().isalive())
                Destroy(gameObject);
        }
        else if (temp.name.Contains("charged") || (temp.name == "Borders")) { }
        else
            Destroy(temp);

    }
    public void applymeteordmg(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
            Destroy(gameObject);
        Debug.Log(hp);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        meleetimer -= Time.deltaTime;
        Vector3 pos = gameObject.transform.position;
        if (pos.x > 8 || pos.x < -8)
            Destroy(gameObject);
    }

    public void applyshotdmg()
    {
        hp -= 10;
        if (hp <= 0)
            Destroy(gameObject);
        Debug.Log(hp);
    }
    public void applymeleedmg()
    {
        if (meleetimer <= 0)
        {
            hp -= 15;
            if (hp <= 0)
                Destroy(gameObject);
            Debug.Log(hp);
            meleetimer = 1;
        }
    }
}
