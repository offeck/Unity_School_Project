using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Soundmanagerscript : MonoBehaviour
{
    public static AudioClip lightsaberclash, lightsaberon, lightsaberoff, lasershot, theme;
    static AudioSource aso;
    static AudioSource aso2;
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        aso = gameObject.AddComponent<AudioSource>();
        aso2 = gameObject.AddComponent<AudioSource>();
        //aso = GetComponent<AudioSource>();
        lightsaberclash = Resources.Load<AudioClip>("Lightsaber-clash");
        lightsaberon = Resources.Load<AudioClip>("Lightsaber-on");
        lightsaberoff = Resources.Load<AudioClip>("Lightsaber-off");
        lasershot = Resources.Load<AudioClip>("Lasershot");
        theme = Resources.Load<AudioClip>("Theme");
        aso2.loop = true;
        aso2.clip = theme;
        aso2.Play();
    }
    public static void setvolume(float vol)
    {
        aso2.volume = vol;
        aso.volume = vol;
    }
    public static float getvolume()
    {
        return aso.volume;
    }
    // Update is called once per frame
    public static void playsound(string str)
    {
        switch (str)
        {
            case "clash":
                aso.PlayOneShot(lightsaberclash);
                break;
            case "on":
                aso.PlayOneShot(lightsaberon);
                break;
            case "off":
                aso.PlayOneShot(lightsaberoff);
                break;
            case "shot":
                aso.PlayOneShot(lasershot);
                break;
        }
    }
}
