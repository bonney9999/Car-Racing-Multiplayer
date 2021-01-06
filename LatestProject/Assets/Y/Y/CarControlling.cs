using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleInputNamespace;
using UnityEngine.UI;
using Photon.Pun;

public class CarControlling : MonoBehaviour
{
    public static CarControlling Instance;

    PhotonView PV;
    public Camera Mycam;
    public GameObject Win;

    //Gameplay & logic
    //checking if this is running in Android.

    public static bool isAndroid = false; // this is static so all scripts can access it.
    public GameObject MobileInputs;                                    // CarControlling.isAndroid ( this will give direct result in boolean)
    public Button Starting;
    public bool On;
   
    private float horizontalInput;
    private float verticalInput;
    private float steerAngle;
    static public bool isBreaking;
    public float breakPower = 300f;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    public float maxSteeringAngle = 30f;
    public float motorForce = 50f;
    public float brakeForce = 0f;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        // Platform will be checked when car is instantiate,
        // then script will awake and check the platform
       if(Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Running on Android");
            isAndroid = true;
            MobileInputs.SetActive(true);
            
        }
        else
        {
            Debug.Log("Running on Windows");
            isAndroid = false;
            MobileInputs.SetActive(false);
        }

        On = false;
        Win.SetActive(false);
    }
    private void Update()
    {
        //  if (PV.IsMine)
        //  {
        if (CarSounds.isEngineOn || CarSoundANDROID.isEngineOn || CarSoundWINDOW.isEngineOn)
        {

            HandleMotor();

        }
        GetInput(isAndroid);
        HandleSteering();
        UpdateWheels();
    }
    //else if (!PV.IsMine)
    //{
    //      Destroy(Mycam);
    //}


    public void startBrack()
    {
        //CarSounds.isEngineOn = true;
        print("Engine");
    }

    private void GetInput(bool platform)
    {

            //if android
            horizontalInput = SimpleInput.GetAxis("Horizontal");
            verticalInput = SimpleInput.GetAxis("Vertical"); 
            //isBreaking = Input.GetKey(KeyCode.Space);
            //isBreaking = SimpleInput.GetButton("shadedDark36"); //SimpleInput/Sprites/ShadedDark/Buttons (Location of this button)
            print("Touch Inputs Enable");
        
        
    }

    private void HandleSteering()
    {
        steerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        brakeForce = isBreaking ? breakPower : 0f;
        frontLeftWheelCollider.brakeTorque = brakeForce;
        frontRightWheelCollider.brakeTorque = brakeForce;
        rearLeftWheelCollider.brakeTorque = brakeForce;
        rearRightWheelCollider.brakeTorque = brakeForce;
    }

    private void UpdateWheels()
    {
        UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

    public void OnTriggerEnter(Collider other)
    {
       if (PV.IsMine)
        {
            if (other.gameObject.CompareTag("Finish"))
            {
                Win.SetActive(true);
            }
        }
    }

}
