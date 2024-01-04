using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
public class CarMovement : MonoBehaviour
{
    private const string hr = "Horizontal";
    private const string vt = "Vertical";

    private float hrinput;
    private float vtinput;
    public float Maxvtspeed;
    private float steerAngle;
    public float CRbreakforce;
    public bool isbreak;
    public float vtspeed;

    public AudioClip idle;
    private AudioSource source;

    [SerializeField] private float MotorForce;
    [SerializeField] private float breakforce;
    [SerializeField] private float MaxsteerAngle;

    [SerializeField] private WheelCollider FLWC;
    [SerializeField] private WheelCollider FRWC;
    [SerializeField] private WheelCollider RLWC;
    [SerializeField] private WheelCollider RRWC;

    [SerializeField] private Transform FLTF;
    [SerializeField] private Transform FRTF;
    [SerializeField] private Transform RLTF;
    [SerializeField] private Transform RRTF;

    public string PlayerOrAI;

    public WaypointContainer WaypointContainer;
    public List<Transform> Waypoints;
    public int CurrentWaypoint;
    public float WaypointRange;

    public TextMeshProUGUI Score;
    // Start is called before the first frame update

    private void Start()
    {
        source = GetComponent<AudioSource>();
        if (PlayerOrAI == "Player")
        {
            Waypoints = WaypointContainer.Waypoint;
            CurrentWaypoint = 0;
        }
        
    }
    private void FixedUpdate()
    {
        if (PlayerOrAI == "Player")
        {
            getInput();
        }
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        
    }
    private void Update()
    {
        if (PlayerOrAI == "Player")
        {
            AddScore();
        }
    }
    private void getInput()
    {
        hrinput = Input.GetAxis(hr);
        vtinput = Input.GetAxis(vt);
        isbreak = Input.GetKey(KeyCode.Space);

        if(vtinput == 1)
        {
            StartCoroutine(Accelerate(vtinput));
        }
        else if(vtinput == -1)
        {
            StartCoroutine(Accelerate(vtinput));
        }
        else
        {
            vtspeed = 0;
            StartCoroutine(Accelerate(-1));
        }
        
        


    }
    private IEnumerator Accelerate(float speed)
    {
        yield return new WaitForSeconds(0.1f);
        vtspeed = vtspeed + speed;
        if (vtspeed >= Maxvtspeed)
        {
            vtspeed = Maxvtspeed;
        }
    }
    public void HandleMotor()
    {

        FLWC.motorTorque = MotorForce * (vtinput +(vtspeed/10000)) ;
        FRWC.motorTorque = MotorForce * (vtinput + (vtspeed / 10000)) ;
        print(FLWC.motorTorque);
    

       
        if (isbreak )
        {
            CRbreakforce = breakforce;
            ApplyBreaking();
            vtspeed = 0;
        }
        else
        {
            CRbreakforce = 0f;
            ApplyBreaking();
        }
    }

    private void ApplyBreaking()
    {
        FLWC.brakeTorque = CRbreakforce;
        FRWC.brakeTorque = CRbreakforce;
        RLWC.brakeTorque = CRbreakforce;
        RRWC.brakeTorque = CRbreakforce;
    }

    private void HandleSteering()
    {
        if (PlayerOrAI == "Player")
        {
            steerAngle = MaxsteerAngle * hrinput;
        }
        FRWC.steerAngle = steerAngle;
        FLWC.steerAngle = steerAngle;

    }

    private void UpdateWheels()
    {
        UpdateSingleWheels(FLWC, FLTF);
        UpdateSingleWheels(FRWC, FRTF);
        UpdateSingleWheels(RLWC, RLTF);
        UpdateSingleWheels(RRWC, RRTF);

    }
    private void UpdateSingleWheels(WheelCollider FLWC, Transform FLTF) 
    {
        Vector3 pos;
        Quaternion rot;
        FLWC.GetWorldPose(out pos, out rot);
        FLTF.rotation = rot;
        FLTF.position = pos;
    }

    public void SetInput(float steer, float Angle, bool brake, float AIbreakforce)
    {
        if (brake)
        {
            vtinput = 1;
            steerAngle = Angle;
            vtspeed = 1000;
            isbreak = brake;
            breakforce = AIbreakforce;
        }
        else
        {
            vtinput = steer;
            steerAngle = Angle;
            isbreak = brake;
            breakforce = AIbreakforce;
        }
        StartCoroutine(Accelerate(vtinput));

    }

    private void AddScore()
    {
        int CurrentScore = int.Parse(Score.text);
        if (Vector3.Distance(Waypoints[CurrentWaypoint].position, transform.position) < WaypointRange)
        {
            CurrentWaypoint++;
            if (CurrentWaypoint == Waypoints.Count) CurrentWaypoint = 0;
            CurrentScore = CurrentScore + 120;
            Score.text = CurrentScore.ToString();
        }
    }
}
