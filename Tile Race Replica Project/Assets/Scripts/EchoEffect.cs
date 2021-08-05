using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
  
    [SerializeField] private float startTimeBetweenSpawns;

    [SerializeField] private GameObject echo;

    private float timeBetweenSpawns;



    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameStarted)
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
