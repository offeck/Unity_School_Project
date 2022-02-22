using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectile;
    public GameObject rightpos;
    public GameObject leftpos;
    public GameObject c;
    private float[] firerate = new float[2];
    private float fr = 1;
    // Start is called before the first frame update
    void Start()
    {
        firerate[0] = 0;
        firerate[1] = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //left -> 1 right -> 0
        firerate[0] -= Time.deltaTime;
        firerate[1] -= Time.deltaTime;
        if (Input.GetButton("Fire2") && (firerate[0]<=0))
        {
            Soundmanagerscript.playsound("shot");// sound
            Instantiate(projectile, rightpos.transform.position, Quaternion.identity);
            if (c.activeSelf)
            {
                c.GetComponent<FasterCside2>().putinqueue("_pr");
            }
            firerate[0] = fr;
        }
        else if (Input.GetButton("Fire1")  && (firerate[1] <= 0))
        {
            Soundmanagerscript.playsound("shot");// sound
            Instantiate(projectile, leftpos.transform.position, Quaternion.identity);
            if (c.activeSelf)
            {
                c.GetComponent<FasterCside2>().putinqueue("_pl");
            }
            firerate[1] = fr;
        }
    }
}
