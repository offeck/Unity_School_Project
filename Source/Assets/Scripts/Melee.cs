using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public GameObject Rightblade;
    public GameObject Leftblade;
    public GameObject c;
    private float[] Timer = new float[2];
    private float t = 6;
    // Start is called before the first frame update
    void Start()
    {
        Timer[0] = 0;
        Timer[1] = 0;
        Rightblade.SetActive(false);
        Leftblade.SetActive(false);
        c.GetComponent<FasterCside2>().putinqueue("_mrd");
        c.GetComponent<FasterCside2>().putinqueue("_mrd");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //left -> 1 right -> 0
        Timer[0] -= Time.deltaTime;
        Timer[1] -= Time.deltaTime;
        if (Timer[0] < t/3 && Rightblade.activeSelf)
        {
            Soundmanagerscript.playsound("off");// sound
            Rightblade.SetActive(false);
            c.GetComponent<FasterCside2>().putinqueue("_mrd");
        }
        if (Timer[1] < t/3  && Leftblade.activeSelf)
        {
            Soundmanagerscript.playsound("off");// sound
            Leftblade.SetActive(false);
            c.GetComponent<FasterCside2>().putinqueue("_mld");
        }
        if (Input.GetButton("Fire2") && !Rightblade.activeSelf && Timer[0] <= 0)
        {
            Soundmanagerscript.playsound("on");// sound
            Rightblade.SetActive(true);
            c.GetComponent<FasterCside2>().putinqueue("_mra");
            Timer[0] = t;
        }
        if (Input.GetButton("Fire1") && !Leftblade.activeSelf && Timer[1] <= 0)
        {
            Soundmanagerscript.playsound("on");// sound
            Leftblade.SetActive(true);
            c.GetComponent<FasterCside2>().putinqueue("_mla");
            Timer[1] = t;
        }
    }
    private void OnDisable()
    {
        Timer[0] = 0;
        Timer[1] = 0;
        Rightblade.SetActive(false);
        Leftblade.SetActive(false);
        c.GetComponent<FasterCside2>().putinqueue("_mrd");
        c.GetComponent<FasterCside2>().putinqueue("_mld");
    }
}
