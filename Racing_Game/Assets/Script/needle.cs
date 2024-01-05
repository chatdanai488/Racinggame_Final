using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class needle : MonoBehaviour
{
    public GameObject Car;
    private CarMovement Movement;
    private float speed;
    private float desiredpos;
    // Start is called before the first frame update
    void Start()
    {
        Movement = Car.GetComponent<CarMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = Movement.GetSpeed();
        Needle();
    }

    private void Needle()
    {
        float angle = 152.474f - (speed / 1200 * (295.139f - 152.474f));
        Debug.Log(angle);
        angle = Mathf.Clamp(angle, 295.139f-360f, 152.474f);
        transform.eulerAngles = new Vector3(0, 0, angle); 
    }
}
