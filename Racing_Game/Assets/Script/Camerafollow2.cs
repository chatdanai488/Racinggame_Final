using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow2 : MonoBehaviour
{
    public Transform target;
    public Transform third;
    public Transform first;
    
    public float speed;
    Vector3 dir;
    public bool iskeyC;
    public float i;


    // Update is called once per frame
    private void FixedUpdate()
    {
        getInput();
        changecamera();
        
    }

    

    private void getInput() 
    {
        iskeyC = Input.GetKeyUp(KeyCode.C);

    }

    private void changecamera() 
    {


        if (iskeyC)
        {
            i += 1;
        }


        if (i%2==0)
        {
            follow1p();
        }
        else
        {
            follow3p();
        }

    }

    private void follow1p()
    {
        gameObject.transform.position = first.position;
        dir = target.eulerAngles;
        transform.localEulerAngles = dir;
       
    }


    private void follow3p()
    {
        gameObject.transform.position = Vector3.Lerp(transform.position, third.transform.position, Time.deltaTime * speed);
        gameObject.transform.LookAt(target.transform.position);
    }
}
