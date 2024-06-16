using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpeedoHandler : MonoBehaviour
{
    public GameObject bg;
    public GameObject needle;
    public GameObject needle_overlay;
    private int maxRotation = 320;
    private int maxSpeed = 250;

    private float circleRatio = 360 / 320;

    private float getRotationForSpeed(int value)
    {
        float percentSpeed = value * 100 / maxSpeed;
        float rotation = maxRotation * percentSpeed / 100;
        return rotation * circleRatio;
    }

    private void rotateNeedleToSpeed(int speed)
    {
        Vector3 targetRotation = new Vector3(0f, 180f, getRotationForSpeed(speed));
        needle.transform.rotation = Quaternion.Euler(targetRotation);
        Debug.Log("Needle rotated to " + getRotationForSpeed(speed) + " degrees.");
    }


    private void sweepNeedle()
    {
        StartCoroutine(SweepNeedleCoroutine(maxRotation));
    }

    void Start()
    {
        Debug.Log("Script Started!");
        Debug.Log(circleRatio);
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            rotateNeedleToSpeed(0);
            sweepNeedle();
        }
    }

    private IEnumerator SweepNeedleCoroutine(float targetRotation)
    {
        // Calculate the current rotation
        rotateNeedleToSpeed(0);
        float currentRotation = needle.transform.rotation.eulerAngles.z;

        // Calculate the difference in rotation we need to cover
        float angleDiff = Mathf.DeltaAngle(currentRotation, targetRotation);

        // Define speed and duration for the sweep
        float sweepSpeed = 40f; // Adjust speed as needed
        float duration = Mathf.Abs(angleDiff) / sweepSpeed;

        // Time elapsed
        float timeElapsed = 0f;

        // While time elapsed is less than duration, interpolate the rotation
        while (timeElapsed < duration)
        {
            // Calculate the interpolation ratio based on elapsed time and duration
            float t = timeElapsed / duration;

            float smoothT = Mathf.SmoothStep(1f, 0f, t);

            // float newRotation = Mathf.Lerp(currentRotation, targetRotation, t);
            float newRotation = Mathf.Lerp(targetRotation, currentRotation, smoothT);
            // Quaternion newRotation = Quaternion.Euler(0f, 180f, Mathf.Lerp(currentRotation, targetRotation, t));
            // needle.transform.rotation = newRotation;
            needle.transform.rotation = Quaternion.Euler(0f, 180f, newRotation);


            // Increment time elapsed by the time since last frame
            timeElapsed += Time.deltaTime;

            // Yield control until the next frame
            yield return null;
        }

        timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            // Calculate the interpolation ratio based on elapsed time and duration
            float t = timeElapsed / duration;
            
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            float newRotation = Mathf.Lerp(targetRotation, currentRotation, smoothT);
            needle.transform.rotation = Quaternion.Euler(0f, 180f, newRotation);
            // Quaternion newRotation = Quaternion.Euler(0f, 180f, Mathf.Lerp(targetRotation, currentRotation, t));
            // needle.transform.rotation = newRotation;

            // Increment time elapsed by the time since last frame
            timeElapsed += Time.deltaTime;

            // Yield control until the next frame
            yield return null;
        }

        // Ensure the needle reaches exactly the target rotation
        needle.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }
}