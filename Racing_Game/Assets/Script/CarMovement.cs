using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

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
    // Start is called before the first frame update

    private void Start()
    {
        source = GetComponent<AudioSource>();
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

    private void getInput()
    {
        hrinput = Input.GetAxis(hr);
        vtinput = Input.GetAxis(vt);
        isbreak = Input.GetKey(KeyCode.Space);

        if(vtinput == 1)
        {
            StartCoroutine(Accelerate(1));
        }
        else if(vtinput == -1)
        {
            StartCoroutine(Accelerate(-1));
        }
        else
        {
            vtspeed = 0;
        }
        
        if(vtspeed >= Maxvtspeed)
        {
            vtspeed = Maxvtspeed;
        }


    }
    private IEnumerator Accelerate(int speed)
    {
        yield return new WaitForSeconds(0.1f);
        vtspeed = vtspeed + 1;
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

    public void SetInput(int steer, float Angle)
    {
        vtinput = steer;
        steerAngle = Angle;
        HandleMotor();
    }

}
