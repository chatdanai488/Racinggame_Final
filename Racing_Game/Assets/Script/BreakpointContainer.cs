using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakpointContainer : MonoBehaviour
{
    public List<Transform> Breakpoint;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            Breakpoint.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
