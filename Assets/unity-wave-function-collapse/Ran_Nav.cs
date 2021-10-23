using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ran_Nav : MonoBehaviour
{

    public Transform[] waypoints; //array of waypoints
    Vector3 targetPoint;

    OverlapWFC CallDraw; //References OverlapWFC

    public float speed = 50; //Speed

    public float distanceThreshold = 1; //Threshhold for Nav object distance

    public int count = 0; //Count

    void Start()
    {
        CallDraw = GameObject.FindGameObjectWithTag("tagOutput").GetComponent<OverlapWFC>(); //References output
        targetPoint = waypoints[0].position; //Set targetpoint to element 0.
    }


    void Update()
    {

        callWFCA(); //Right now, the map regens, and the ball goes to the spot where a now deleted wyapoint is. Is that a problem? Experiment whit where callWFCA is.

        if (Vector3.Distance(targetPoint, transform.position) < distanceThreshold) //if nav object closer than threshhold.
        {
            changeTarget(); //call change target
            count++; //increment
        }
        moveTowards(); //call movetowards
    }

    void moveTowards()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, Time.deltaTime * speed);
    }

    void changeTarget()
    {
        int randomIndex = Random.Range(0, waypoints.Length); //chooses random elements.
        targetPoint = waypoints[randomIndex].position; //Sets destination to random waypoints element.
    }

    void callWFCA()
    {
        if (count == 3) //if count is 3. May have to check count against waypoints.Length, or a var equal to that value.
        {
            CallDraw.Generate(); //call generate (OverlapWFC)
            CallDraw.Run(); //call run (OverlapWFC)
            count = 0; //reset count
        }
    }

}
