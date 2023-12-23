using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float rotationspeed;
    public float positionSpeed;

    // Update is called once per frame
    void Update()
    {
        Position();
        Rotation();

    }
    private void Position()
    {
        var tarpos = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position,tarpos,positionSpeed*Time.deltaTime);
    }
    private void Rotation()
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction,Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation,rotation,rotationspeed*Time.deltaTime);
    }
}
