using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CarMovement))]
public class CarAIController : MonoBehaviour
{
    public WaypointContainer WaypointContainer;
    public List<Transform> Waypoints;
    public int CurrentWaypoint;
    private CarMovement CarMovement;
    public float WaypointRange;
    private float CurrentAngle;
    // Start is called before the first frame update
    void Start()
    {
        CarMovement = GetComponent<CarMovement>();
        Waypoints = WaypointContainer.Waypoint;
        CurrentWaypoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Waypoints[CurrentWaypoint].position, transform.position) < WaypointRange)
        {
            CurrentWaypoint++;
            if(CurrentWaypoint == Waypoints.Count) CurrentWaypoint = 0;
        }
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        CurrentAngle = Vector3.SignedAngle(fwd, Waypoints[CurrentWaypoint].position-transform.position,Vector3.up);
        CarMovement.SetInput(1,CurrentAngle);
        Debug.DrawRay(transform.position, Waypoints[CurrentWaypoint].position - transform.position, Color.yellow);
    }
}
