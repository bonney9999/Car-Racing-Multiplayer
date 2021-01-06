using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityOfPlayer : MonoBehaviour
{
    bool stickDownLast;
    float val = 0;
    bool isthat = false;
    private void Update()
    {
        //Rigidbody rb = GetComponent<Rigidbody>();
        //Vector3 vel = rb.velocity;
        //if (vel.magnitude > 0)
        //{


            if (Input.GetAxis("Vertical") == 1.0)
            {

            }
            else
            {
               
            }


           


            ////if(Input.GetAxis("Vertical") > 0)
            //if(Input.GetKeyDown(KeyCode.W))
            //{
            //    Debug.Log("Car is moving forward");
            //}
            ////else if(Input.GetAxis("Vertical") < 0)
            //else if(Input.GetKeyDown(KeyCode.S))
            //{
            //    Debug.Log("Car is moving backward");
            //}

        
    }
    void useit()
    {
        if (Input.GetAxis("Vertical") < 0)
        {
            if (!stickDownLast)
               //work

            stickDownLast = true;
        }
        else
            stickDownLast = false;
    }
}
