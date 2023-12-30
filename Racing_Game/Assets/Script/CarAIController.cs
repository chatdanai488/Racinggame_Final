using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CarMovement))]
public class CarAIController : MonoBehaviour
{
    public WaypointContainer WaypointContainer;
    public List<Transform> Waypoints;
    public int CurrentWaypoint;
    public BreakpointContainer BreakpointContainer;
    public List<Transform> BreakPoints;
    public int CurrentBreakpoint;
    private CarMovement CarMovement;
    public float WaypointRange;
    private float CurrentAngle;


    private bool isbreak;
    // Start is called before the first frame update
    void Start()
    {
        CarMovement = GetComponent<CarMovement>();
        Waypoints = WaypointContainer.Waypoint;
        BreakPoints = BreakpointContainer.Breakpoint;
        CurrentWaypoint = 0;
        CurrentBreakpoint = 0;
        isbreak = false;
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

        if (!isbreak)
        {
            CarMovement.SetInput(1,CurrentAngle,false,3000);
        }

        Quaternion eulerangles = transform.rotation;
        
        if(eulerangles.x > 45 || eulerangles.x < -45 || eulerangles.z > 45 || eulerangles.z < -45)
        {
            ResetCar(eulerangles);
        }
        
        Debug.Log(eulerangles);
        Debug.DrawRay(transform.position, Waypoints[CurrentWaypoint].position - transform.position, Color.yellow);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("BreakPoint"))
        {
            CarMovement.SetInput(1, CurrentAngle, true, 150000);
            isbreak = true;
        }
        if (other.gameObject.CompareTag("SlowField"))
        {
            CarMovement.SetInput(0.01f, CurrentAngle, false, 150000);
            isbreak = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BreakPoint"))
        {
            StartCoroutine(EngineStart());
;       }
        if (other.gameObject.CompareTag("SlowField"))
        {
            StartCoroutine(EngineStart());
            ;
        }
    }
    private IEnumerator EngineStart()
    {
        yield return new WaitForSeconds(0.2f);
        isbreak = false;
        
    }
    private void ResetCar(Quaternion eulerAngles)
    {
        transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        eulerAngles = new Quaternion(0,eulerAngles.y,0,0);
    }
}
