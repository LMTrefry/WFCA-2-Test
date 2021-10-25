using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_Nav : MonoBehaviour
{

    public GameObject[] waypoints; //array of waypoints
    Vector3 targetPoint;

    OverlapWFC CallDraw; //References OverlapWFC

    public float speed = 50; //Speed

    public float distanceThreshold = 1; //Threshhold for Nav object distance

    public int count = 0; //Count

    public Transform output;

    void Start() //This is all out of order.
    {
        waypoints = GameObject.FindGameObjectsWithTag("tagMakeArray");
        //waypoints = GameObject.GetComponentsInChildren(typeof(GameObject)); //An object reference is required.

        CallDraw = GameObject.FindGameObjectWithTag("tagOutput").GetComponent<OverlapWFC>(); //References output

        targetPoint = waypoints[0].transform.position; //Set targetpoint to element 0. This is what's causing it to start moving immediately, even with an empty array.

        AddTagRecursively(output, "tagMakeArray"); //It doesn't do this on start. Must investigate why.
    }


    void Update()
    {

        callWFCA(); //Right now, the map regens, and the ball goes to the spot where a now deleted wyapoint is. Is that a problem? Experiment whit where callWFCA is.

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
    }

    void callWFCA()
    {
        if (count == 3) //if count is 3. May have to check count against waypoints.Length, or a var equal to that value.
        {
            CallDraw.Generate(); //call generate (OverlapWFC)
            CallDraw.Run(); //call run (OverlapWFC)
            //fillArray();
            AddTagRecursively(output, "tagMakeArray"); //It includes the immediately previous waypoints aswell. It must be that they are assigned the tag then deleted.
            waypoints = GameObject.FindGameObjectsWithTag("tagMakeArray");
            count = 0; //reset count
        }
    }

    /*void fillArray() //Does this have to be a recursive loop?
    {
        foreach(GameObject output in transform) //Right now the transform and gameobject beign referred to is the NavObj.
        {
            output.gameObject.tag = "tagMakeArray";
        }
        //gameObject.tag = "tagMakeArray";
        waypoints = GameObject.FindGameObjectsWithTag("tagMakeArray");
    }*/

    void AddTagRecursively (Transform trans, string tag) //This is recursive and should refer to the children of output (output-overlap in this case).
    {
        trans.gameObject.tag = tag;
        if (trans.GetChildCount() > 0)
            foreach (Transform t in trans)
                AddTagRecursively(t, tag);
    }
}
