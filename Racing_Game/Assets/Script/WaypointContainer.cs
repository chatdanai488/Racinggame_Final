using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointContainer : MonoBehaviour
{
    public List<Transform> Waypoint;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            Waypoint.Add(child);
        }
        Waypoint.Remove(Waypoint[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
