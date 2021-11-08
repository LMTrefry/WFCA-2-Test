using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*S0 right now there's a bug related to the visiting the corner bug.*/

public class New_Nav : MonoBehaviour
{

    public GameObject[] waypoints; //array of waypoints
    public List<Vector3> prevWaypointsList = new List<Vector3>(); //Records all waypoints generated 
    public List<Vector3> prevWaypointsVisited = new List<Vector3>(); //Records visited waypoints.

    public Vector3 targetPoint;

    OverlapWFC CallDraw; //References OverlapWFC

    public float speed = 50; //Speed

    public float distanceThreshold = 1; //Threshhold for Nav object distance

    public int count = 0; //Count
    public int countNowhere = 0; //Count if nav_obj has nowhere to go.

    public Transform output;

    void Start()
    {
        CallDraw = GameObject.FindGameObjectWithTag("tagOutput").GetComponent<OverlapWFC>(); //References output
        AddTagRecursively(output, "tagMakeArray");
        waypoints = GameObject.FindGameObjectsWithTag("tagMakeArray");

        for (int i = 0; i < waypoints.Length; i++) //Adds the elements of waypoints to prevWaypointsList
        {
            prevWaypointsList.Add(waypoints[i].transform.position); //Adds all generated waypoints to the list.
        }

        targetPoint = waypoints[0].transform.position; //Set targetpoint to element 0. This is what's causing it to start moving immediately, even with an empty array.
    }

    void Update()
    {

        callWFCA(); //Right now, the map regens, and the ball goes to the spot where a now deleted wyapoint is. Is that a problem? Experiment with where callWFCA is.

        if (Vector3.Distance(targetPoint, transform.position) < distanceThreshold) //if nav object closer than threshhold.
        {
            changeTarget(); //call changeTarget
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
        targetPoint = waypoints[randomIndex].transform.position; //Sets destination to random waypoints element.

        //Is this where we check the prevWaypointsVisited list? 
        //The waypoints is already determined, so we can choose a different one before adding it to the list if it's the same.
        for (int i = 0; i < prevWaypointsVisited.Count; i++) //iterates through prevWaypointsVisited
        {
            if (targetPoint == prevWaypointsVisited[i]) //if targetPoint is equal to any element of prevWaypoitnsVisited
            {
                randomIndex = Random.Range(0, waypoints.Length); //instead of getting caught in an infinite loop, we just choose a different waypoint that hasn't been visited.
                targetPoint = waypoints[randomIndex].transform.position;
            }
        } //What do we do if there's nowhere left to go? Call generate/run?

        prevWaypointsVisited.Add(waypoints[randomIndex].transform.position); //Adds the visited waypoint to the list.
    }

    void callWFCA()
    {

        if (count == 4) //if count is 3. May have to check count against waypoints.Length, or a var equal to that value.
        {

            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = null;
            }

            CallDraw.Generate(); //call generate (OverlapWFC)
            CallDraw.Run(); //call run (OverlapWFC)
            AddTagRecursively(output, "tagMakeArray"); //It includes the immediately previous waypoints aswell. It must be that they are assigned the tag then deleted.
            waypoints = GameObject.FindGameObjectsWithTag("tagMakeArray");
            count = 0; //reset count

            for (int i = 0; i < waypoints.Length; i++) //Adds the elements of waypoints to prevWaypointsList
            {
                prevWaypointsList.Add(waypoints[i].transform.position);
            }

        }
    }

    void AddTagRecursively (Transform trans, string tag) //This is recursive and should refer to the children of output (output-overlap in this case).
    {
        trans.gameObject.tag = tag;
        if (trans.childCount > 0)
            foreach (Transform t in trans)
                AddTagRecursively(t, tag);
    }
}
