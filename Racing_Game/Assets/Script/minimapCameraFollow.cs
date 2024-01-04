using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapCameraFollow : MonoBehaviour
{
    public Transform target;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x,transform.position.y,target.position.z);
        transform.eulerAngles = new Vector3(90,target.eulerAngles.y,0);
    }
}
