using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class carSounds : MonoBehaviour
{

    public float minSpeed;
    public float maxSpeed;
    private float currentSpeed;

    private Rigidbody carRb;
    private AudioSource carAudio;

    public float minPitch;
    public float maxPitch;
    private float pitchFromCar;

    private void Start()
    {
        carAudio = GetComponent<AudioSource>();
        carRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        EngineSound();
    }

    void EngineSound()
    {
        float moveInput = Input.GetAxis("Vertical");
        currentSpeed = carRb.linearVelocity.magnitude;

        // Base pitch from speed (car is moving)
        float pitchFromCar = currentSpeed / 20f;
        pitchFromCar = Mathf.Clamp01(pitchFromCar);

        // Extra pitch from acceleration (player pressing W/S)
        float accelFactor = Mathf.Abs(moveInput);

        float targetPitch = minPitch
                            + pitchFromCar * (maxPitch - minPitch) * 0.7f
                            + accelFactor * (maxPitch - minPitch) * 0.3f;

        carAudio.pitch = Mathf.Lerp(carAudio.pitch, targetPitch, Time.deltaTime * 5f);
    }
}
