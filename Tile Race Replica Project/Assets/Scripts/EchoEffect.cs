using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    private float timeBetweenSpawns;
    [SerializeField] private float startTimeBetweenSpawns;
    [SerializeField] private GameObject echo;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameStarted)
        {
            if (timeBetweenSpawns <= 0)
            {
                GameObject trail = Instantiate(echo, transform.position, Quaternion.identity);
                Destroy(trail, 2);
                timeBetweenSpawns = startTimeBetweenSpawns;
            }
            else
            {
                timeBetweenSpawns -= Time.deltaTime;
            }
        }
       
    }
}
