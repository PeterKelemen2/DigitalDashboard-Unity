using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpeedoHandler : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject bg;
    public GameObject needle;
    public GameObject needle_overlay;
    private int maxRotation = 320;
    private int maxSpeed = 250;

    private float circleRatio = 360 / 320;

    private bool isMovingNeedle = false;
    private bool isSweeping = false;


    private float getRotationForSpeed(int value)
    {
        float percentSpeed = value * 100 / maxSpeed;
        float rotation = maxRotation * percentSpeed / 100;
        return rotation * circleRatio;
    }

    private void rotateNeedleToSpeed(int speed)
    {
        float targetRotation = getRotationForSpeed(speed);
        float currentRotation = needle.transform.rotation.eulerAngles.z;
        // needle.transform.rotation = Quaternion.Euler(targetRotation);
        StartCoroutine(MoveNeedle(currentRotation, targetRotation, canUpdateStatus: true));
        Debug.Log("Needle moving to " + speed + "km/h (" + getRotationForSpeed(speed) + "°)");
    }


    private void sweepNeedle()
    {
        StartCoroutine(SweepNeedleCoroutine(maxRotation));
    }

    void Start()
    {
        Debug.Log("Script Started!");
        Debug.Log(circleRatio);
        gameManager = FindObjectOfType<GameManager>();
        // gameManager.changeStatusText("This is from SpeedoHandler!");
        // gameManager.GetComponent<GameManager>().changeStatusText("ASdasdasd");
    }

    void Update()
    {
        if (Input.GetKeyDown("space") && !isMovingNeedle)
        {
            // rotateNeedleToSpeed(0);
            // sweepNeedle();
            gameManager.changeStatusText("Moving needle!");
            rotateNeedleToSpeed(Random.Range(1, maxSpeed - 1));
        }

        if (Input.GetKeyDown("e") && !isMovingNeedle && !isSweeping)
        {
            gameManager.changeStatusText("Sweeping needle!");
            StartCoroutine(SweepSequence());
        }
    }


    private IEnumerator SweepSequence()
    {
        isSweeping = true;
        yield return StartCoroutine(MoveNeedle(needle.transform.rotation.eulerAngles.z, 0f, 0.2f));
        yield return StartCoroutine(MoveNeedle(needle.transform.rotation.eulerAngles.z, 0f, 0.2f));
        yield return StartCoroutine(MoveNeedle(0f, maxRotation, 0.5f));
        yield return StartCoroutine(MoveNeedle(maxRotation, 0f, 0.5f, canUpdateStatus: true));
        isSweeping = false;
        yield return null;
    }

    private IEnumerator MoveNeedle(float currentRot, float targetRot, float duration = 0.3f,
        bool canUpdateStatus = false)
    {
        isMovingNeedle = true;

        // float duration = 0.3f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;

            float smoothT = Mathf.SmoothStep(1f, 0f, t);

            float newRotation = Mathf.Lerp(targetRot, currentRot, smoothT);
            needle.transform.rotation = Quaternion.Euler(0f, 180f, newRotation);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        isMovingNeedle = false;
        if (canUpdateStatus)
        {
            gameManager.changeStatusText("Waiting for input...");
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