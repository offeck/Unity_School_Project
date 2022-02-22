using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5.0f;
    private float dashtimer = 0;
    private float translation;
    private float rotation;

    void FixedUpdate()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        dashtimer -= Time.deltaTime;
        if (Input.GetButton("Jump") && dashtimer <= 0)
        {
            dashtimer = 2;
            translation = Input.GetAxis("Vertical") * speed * 10;
            rotation = Input.GetAxis("Horizontal") * speed * 10;
        }
        else
        {
            translation = Input.GetAxis("Vertical") * speed;
            rotation = Input.GetAxis("Horizontal") * speed;
        }

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(rotation, 0, 0);

        // Rotate around our y-axis
        transform.Translate(0, translation, 0);
    }
}
