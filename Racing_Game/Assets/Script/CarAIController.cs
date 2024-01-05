using System;
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

    public GameObject Car;


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

        Vector3 eulerangles = transform.rotation.eulerAngles;
        float TrueeulerX = eulerangles.x;
        float TrueeulerZ = eulerangles.z;
        if(TrueeulerX > 300f)
        {
            TrueeulerX = TrueeulerX - 360f;
        }
        if(TrueeulerZ > 300f)
        {
            TrueeulerZ = TrueeulerZ - 360f;
        }
        
        if (!(TrueeulerX > -45f && TrueeulerX < 45f) || !!(TrueeulerZ > -45f && TrueeulerZ < 45f))
        {
            Car.transform.rotation = Quaternion.Euler(0, eulerangles.y, 0);
        }
        TrueeulerX = Mathf.Clamp(TrueeulerX, -45f, 45f);
        TrueeulerZ = Mathf.Clamp(TrueeulerZ, -45f, 45f);
        Car.transform.rotation = Quaternion.Euler(TrueeulerX, eulerangles.y, TrueeulerZ);
        
        
        Debug.DrawRay(transform.position, Waypoints[CurrentWaypoint].position - transform.position, Color.yellow);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("BreakPoint"))
        {
            CarMovement.SetInput(1, CurrentAngle, false, 1000);
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
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("OutBounds"))
        {
            ResetCarPosition();
        }
    }
    private IEnumerator EngineStart()
    {
        yield return new WaitForSeconds(0.2f);
        isbreak = false;
        
    }
    private void ResetCar(Vector3 eulerAngles)
    {
        Car.transform.position = new Vector3(transform.position.x, 100, transform.position.z);
        Car.transform.rotation = new Quaternion(0,-eulerAngles.y,0,0);
    }
    public void ResetCarPosition()
    {
        Car.transform.position = Waypoints[CurrentWaypoint].position;
    }
}
