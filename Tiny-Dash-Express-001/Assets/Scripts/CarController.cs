using UnityEngine;
using System;
using System.Collections.Generic;

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
		public Axel axel;
	}

	public float maxSpeed = 25f; // km/h / 3.6 = m/s (the value you put for maxSpeed)
	public float maxAcceleration = 30.0f;
	public float brakeAcceleration = 50.0f;

	public float turnSansitivity = 1.0f;
	public float maxSteerAngle = 30.0f;

	public Vector3 _centerOfMass;

	public List<Wheel> wheels;

	float moveInput;
	float steerInput;

	private Rigidbody carRb;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
		carRb.centerOfMass = _centerOfMass;
    }

    void Update()
    {
        GetInputs();
		AnimateWheels();
		WheelEffect();
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
			wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * speedFactor;// * Time.deltaTime; AI suggested to remove Time.deltaTime
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
		if (Input.GetKey(KeyCode.Space))
		{
			foreach (var wheel in wheels)
			{
				wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration;// * Time.deltaTime;
			}
		}
		else
		{
			foreach( var wheel in wheels)
			{
				wheel.wheelCollider.brakeTorque = 0;
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

                if (Input.GetKey(KeyCode.Space) && slip > 0.2f) // tweak this value
                {
                    trail.emitting = true;
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
}
