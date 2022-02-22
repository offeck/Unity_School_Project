using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Sliderscript : MonoBehaviour
{
    private Slider s;
    // Update is called once per frame
    void Start()
    {
        //s.value = Soundmanagerscript.getvolume();
        s = gameObject.GetComponent<Slider>();
        s.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
    public void ValueChangeCheck()
    {
        Soundmanagerscript.setvolume(s.value);
    }
}
