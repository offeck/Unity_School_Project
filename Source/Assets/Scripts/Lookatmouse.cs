using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookatmouse : MonoBehaviour
{

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, gameObject.transform.position.y));
        Vector3 forward = mouseWorld - gameObject.transform.position;
        forward.Set(forward.x, forward.y, 0);
        gameObject.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}
