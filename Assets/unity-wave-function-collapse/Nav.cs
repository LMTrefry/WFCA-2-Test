using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav : MonoBehaviour
{

    public GameObject[] waypoints;
    int current = 0;
    public float rotSpeed;
    public float speed;
    private float WPRadius = 1;
    public GameObject output;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //waypoints = output.GetComponentInChildren<GameObject[]>(); //output not in monobehaviour?
        //GetComponent only goes one layer deep, must be input (we need to get input to stop deleting).

        if (Vector3.Distance(waypoints[current].transform.position, transform.position) < WPRadius)
        {
            current++;
            if (current >= waypoints.Length)
            {
                current = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speed);

    }
}

//What do I need to figure out?
//1. How to reference clones.
//2. How to reference array items from a different script.
//3. How to make the size of the waypoints array dependent on the amount of clones present.

//Should we use getchild like in OverlapWFC line 91?

// Numbers 1 and 2 are tied together. 

//So my issue right now is that I want to read the children of...
