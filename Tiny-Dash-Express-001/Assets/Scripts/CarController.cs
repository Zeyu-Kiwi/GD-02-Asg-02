using System;
using System.Collections.Generic;
using UnityEngine;
using static CarController;

public class CarController : MonoBehaviour
{
	public enum Axel
	{
		Front,
		Rear
	}

	[Serializable]
	public struct Wheel
	{
		public GameObject wheelModel;
		public WheelCollider wheelCollider;
		public GameObject wheelEffectObj;
		public ParticleSystem driftSmokeVFX;
		public Axel axel;
	}

	public float maxSpeed = 25f; // km/h / 3.6 = m/s (the value you put for maxSpeed)
	public float maxAcceleration = 30.0f;
	public float normalBrake = 50.0f;

	public float turnSansitivity = 1.0f;
	public float maxSteerAngle = 30.0f;

    private float forwardVelocity;

    public Vector3 _centerOfMass;

	public List<Wheel> wheels;

	float moveInput;
	float steerInput;

	private Rigidbody carRb;
    private CarLight carLights;

    public GameObject map;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
		carRb.centerOfMass = _centerOfMass;

        carLights = GetComponent<CarLight>();
    }

    void Update()
    {
        GetInputs();
		AnimateWheels();
		WheelEffect();
        MyMiniMap();

        forwardVelocity = Vector3.Dot(carRb.linearVelocity, transform.forward);
    }

   

    void FixedUpdate() //was LateUpdate
    {
		Move();
		Steer();
		Brake();
    }

    void GetInputs()
	{
		moveInput = Input.GetAxis("Vertical");
		steerInput = Input.GetAxis("Horizontal");
    }

    void Move()
    {

		float speed = carRb.linearVelocity.magnitude;
		float speedFactor = Mathf.Clamp01(1 - (speed / maxSpeed));

        

        foreach (var wheel in wheels)
		{
            
            if (wheel.axel == Axel.Rear)
            {

                wheel.wheelCollider.motorTorque = moveInput * 1000f * maxAcceleration * speedFactor;// * Time.deltaTime; AI suggested to remove Time.deltaTime
             
            }
		}	
    }

   

    void Steer()
	{
		foreach(var wheel in wheels)
		{
			if (wheel.axel == Axel.Front)
			{
				var _steerAngle = steerInput * turnSansitivity * maxSteerAngle;
				wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
			}
		}
	}

    void Brake()
    {

        bool isTurning = Mathf.Abs(steerInput) > 0.1f;
        bool isMoving = Mathf.Abs(moveInput) > 0.1f;
        bool isBraking = Input.GetKey(KeyCode.Space);

        foreach (var wheel in wheels)
        {
            Debug.Log("Brake Torque: " + wheel.wheelCollider.brakeTorque);

            //Car four status logic
            if (isBraking == true)
            {
                //turn on back light
                carLights.isBackLightOn = true;
                carLights.OperateBackLight();

                if (isTurning == true)
                {
                    if (isMoving == true)
                    {
                        // IDLE: slow down gradually when no input
                        wheel.wheelCollider.brakeTorque = 500f;

                        WheelFrictionCurve friction = wheel.wheelCollider.sidewaysFriction;
                        friction.stiffness = 1.8f;
                        wheel.wheelCollider.sidewaysFriction = friction;

                        //Debug.Log("Idle mode. Wheel stiffness: " + friction.stiffness + ". Brake Torque: " + wheel.wheelCollider.brakeTorque);
                    }
                    else if (isMoving == false && wheel.axel == Axel.Rear)
                    {
                        // DRIFT: rear wheels lose grip + slight brake
                        wheel.wheelCollider.brakeTorque = 5000f;

                        WheelFrictionCurve friction = wheel.wheelCollider.sidewaysFriction;
                        friction.stiffness = 0.1f;
                        wheel.wheelCollider.sidewaysFriction = friction;

                        if (carRb.linearVelocity.magnitude >= 10.0f)
                        {
                            wheel.driftSmokeVFX.Emit(1);
                        }
                            
                        

                        //Debug.Log("Drift mode. Wheel stiffness: " + friction.stiffness + ". Brake Torque: " + wheel.wheelCollider.brakeTorque);
                    }
                }
                else if (isTurning == false)
                {
                    if (isMoving == true)
                    {
                        // IDLE: slow down gradually when no input
                        wheel.wheelCollider.brakeTorque = 500f;

                        WheelFrictionCurve friction = wheel.wheelCollider.sidewaysFriction;
                        friction.stiffness = 1.8f;
                        wheel.wheelCollider.sidewaysFriction = friction;

                        //Debug.Log("Idle mode. Wheel stiffness: " + friction.stiffness + ". Brake Torque: " + wheel.wheelCollider.brakeTorque);
                    }
                    else if (isMoving == false)
                    {
                        // BRAKE: quickly comes to a stop
                        wheel.wheelCollider.brakeTorque = 300 * normalBrake;

                        WheelFrictionCurve friction = wheel.wheelCollider.sidewaysFriction;
                        friction.stiffness = 1.8f;
                        wheel.wheelCollider.sidewaysFriction = friction;
                        //Debug.Log("Brake mode. Wheel stiffness: " + friction.stiffness + ". Brake Torque: " + wheel.wheelCollider.brakeTorque);
                    }
                }
            }
            else if (isBraking == false)
            {
                //turn off back light
                carLights.isBackLightOn = false;
                carLights.OperateBackLight();

                if (isTurning == true)
                {
                    if (isMoving == true)
                    {
                        // Opposite direction detected
                        if ((moveInput > 0 && forwardVelocity < -0.5f) ||
                            (moveInput < 0 && forwardVelocity > 0.5f))
                        {
                            // Apply strong brake instead of reverse torque
                            wheel.wheelCollider.motorTorque = 0f;
                            wheel.wheelCollider.brakeTorque = 1500000f; // tweak this
                        }
                        else
                        {
                            // DRIVING: no braking, full grip
                            wheel.wheelCollider.brakeTorque = 0f;

                            WheelFrictionCurve friction = wheel.wheelCollider.sidewaysFriction;
                            friction.stiffness = 1.8f;
                            wheel.wheelCollider.sidewaysFriction = friction;
                            //Debug.Log("Drive mode. Wheel stiffness: " + friction.stiffness + ". Brake Torque: " + wheel.wheelCollider.brakeTorque);
                        }

                        //// DRIVING: no braking, full grip
                        //wheel.wheelCollider.brakeTorque = 0f;

                        //WheelFrictionCurve friction = wheel.wheelCollider.sidewaysFriction;
                        //friction.stiffness = 1.8f;
                        //wheel.wheelCollider.sidewaysFriction = friction;
                        ////Debug.Log("Drive mode. Wheel stiffness: " + friction.stiffness + ". Brake Torque: " + wheel.wheelCollider.brakeTorque);
                    }
                    else if (isMoving == false)
                    {
                        // IDLE: slow down gradually when no input
                        wheel.wheelCollider.brakeTorque = 500f;

                        WheelFrictionCurve friction = wheel.wheelCollider.sidewaysFriction;
                        friction.stiffness = 1.8f;
                        wheel.wheelCollider.sidewaysFriction = friction;

                        //Debug.Log("Idle mode. Wheel stiffness: " + friction.stiffness + ". Brake Torque: " + wheel.wheelCollider.brakeTorque);
                    }
                }
                else if (isTurning == false)
                {
                    if (isMoving == true)
                    {
                        // Opposite direction detected
                        if ((moveInput > 0 && forwardVelocity < -0.5f) ||
                            (moveInput < 0 && forwardVelocity > 0.5f))
                        {
                            // Apply strong brake instead of reverse torque
                            wheel.wheelCollider.motorTorque = 0f;
                            wheel.wheelCollider.brakeTorque = 1500000f; // tweak this
                        }
                        else
                        {
                            // DRIVING: no braking, full grip
                            wheel.wheelCollider.brakeTorque = 0f;

                            WheelFrictionCurve friction = wheel.wheelCollider.sidewaysFriction;
                            friction.stiffness = 1.8f;
                            wheel.wheelCollider.sidewaysFriction = friction;
                            //Debug.Log("Drive mode. Wheel stiffness: " + friction.stiffness + ". Brake Torque: " + wheel.wheelCollider.brakeTorque);
                        }

                        //// DRIVING: no braking, full grip
                        //wheel.wheelCollider.brakeTorque = 0f;

                        //WheelFrictionCurve friction = wheel.wheelCollider.sidewaysFriction;
                        //friction.stiffness = 1.8f;
                        //wheel.wheelCollider.sidewaysFriction = friction;
                        ////Debug.Log("Drive mode. Wheel stiffness: " + friction.stiffness + ". Brake Torque: " + wheel.wheelCollider.brakeTorque);
                    }
                    else if (isMoving == false)
                    {
                        // IDLE: slow down gradually when no input
                        wheel.wheelCollider.brakeTorque = 500f;

                        WheelFrictionCurve friction = wheel.wheelCollider.sidewaysFriction;
                        friction.stiffness = 1.8f;
                        wheel.wheelCollider.sidewaysFriction = friction;
                        //Debug.Log("Idle mode. Wheel stiffness: " + friction.stiffness + ". Brake Torque: " + wheel.wheelCollider.brakeTorque);
                    }
                }
            }
        }
    }

    void AnimateWheels()
	{
		foreach (var wheel in wheels)
		{
			Quaternion rot;
			Vector3 pos;
			wheel.wheelCollider.GetWorldPose(out pos, out rot);
			wheel.wheelModel.transform.position = pos;
			wheel.wheelModel.transform.rotation = rot;
		}
	}

    void WheelEffect()
    {
        foreach (var wheel in wheels)
        {
            var trail = wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>();

            WheelHit hit;
            if (wheel.wheelCollider.GetGroundHit(out hit))
            {
                float slip = Mathf.Abs(hit.sidewaysSlip);

                if (Input.GetKey(KeyCode.Space) && slip > 0.2f && carRb.linearVelocity.magnitude >= 10.0f) // tweak this value
                {
                    trail.emitting = true;
                    //wheel.driftSmokeVFX.Emit(1);
                }
                else
                {
                    trail.emitting = false;
                }
            }
            else
            {
                trail.emitting = false;
            }
        }
    }

    private void MyMiniMap()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !map.activeInHierarchy)
        {
            map.SetActive(true);
            AudioManager.instance.Play("Map");
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && map.activeInHierarchy)
        {
            map.SetActive(false);
            AudioManager.instance.Play("Map");
            Time.timeScale = 1f;
        }
    }
}
