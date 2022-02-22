using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spaceship;
    public GameObject enemyspaceship;
    public GameObject borb;
    public GameObject morb;
    public float spawnrange;
    private float nospawnti = 0;
    private int b = 5;
    private int m = 7;

    private float ti;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (nospawnti > 0)
        {
            nospawnti -= Time.fixedDeltaTime;
        }
        ti = Time.time;
        //if ((t > 5) && (Random.Range(0, 20) == 4))
        if ((Mathf.RoundToInt(ti) % b == 0) && (nospawnti <= 0))
        {
            nospawnti = 1;
            Vector3 position = new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-4.0f, 4.0f), 0);
                while((Vector3.Distance(spaceship.transform.position, position) < spawnrange) || (Vector3.Distance(enemyspaceship.transform.position, position)<spawnrange))
            {
                position = new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-4.0f, 4.0f), 0);
            }
                Instantiate(borb, position, Quaternion.identity);
            b = b * 2;
        }
        if ((Mathf.RoundToInt(ti) % m == 0) && (nospawnti <= 0))
        {
            nospawnti = 1;
            Vector3 position = new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-4.0f, 4.0f), 0);
            while ((Vector3.Distance(spaceship.transform.position, position) < spawnrange) || (Vector3.Distance(enemyspaceship.transform.position, position) < spawnrange))
            {
                position = new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-4.0f, 4.0f), 0);
            }
            Instantiate(morb, position, Quaternion.identity);
            m = m * 2;
        }
    }
}
