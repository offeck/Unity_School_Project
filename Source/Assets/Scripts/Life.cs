using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    private int hp;
    private float meleetimer = 0;
    private string color;
    private int maxlife = 100;
    public Slider Slider;
    public Text txthp;
    public GameObject c;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxlife;
        Slider.maxValue = hp;
        Slider.value = hp;
        txthp.text = hp.ToString();
    }
    public void sethp(int newhp)
    {
        hp = newhp;
    }
    public int getmaxlife()
    {
        return maxlife;
    }
    public int gethp()
    {
        return hp;
    }
    private void FixedUpdate()
    {
        //color = gameObject.tag;
        meleetimer -= Time.deltaTime;
    }
    public void applymeteordmg(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
            EndGame();
        Debug.Log(hp);
        Slider.value = hp;
        txthp.text = hp.ToString();
    }
    public void applyshotdmg()
    {
        hp -= 5;
        if (hp <= 0)
            EndGame();
        Debug.Log(hp);
        Slider.value = hp;
        txthp.text = hp.ToString();
    }
    public void applymeleedmg()
    {
        if (meleetimer <= 0)
        {
            hp -= 15;
            if (hp <= 0)
                EndGame();
            Debug.Log(hp);
            Slider.value = hp;
            txthp.text = hp.ToString();
            meleetimer = 1;
        }
    }
    public bool isalive()
    {
        return hp != 0;
    }
    public void EndGame()
    {
        txthp.text = "0";
        if (c.activeSelf)
        {
            if(gameObject.tag == "Blue")
                c.GetComponent<FasterCside2>().gameresult("lost");
            else
                c.GetComponent<FasterCside2>().gameresult("won");
        }
        //Debug.Log(gameObject.name + " Lost.");
        //Application.Quit();
    }    

}
