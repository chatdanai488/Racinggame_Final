using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCheck : MonoBehaviour
{
    public GameObject Car;
    private CarAIController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = Car.GetComponent<CarAIController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            controller.ResetCarPosition();
        }
    }
}
