using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shardController : MonoBehaviour
{
    GameObject player;
    float speed = 1;
    float moveToPlayerDistance = 2;
    float lifeTimer = 30;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= moveToPlayerDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destroy(this.gameObject);
        }
        
    }
}
