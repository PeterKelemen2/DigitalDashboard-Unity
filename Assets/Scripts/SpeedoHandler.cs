using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedoHandler : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject bg;
    public GameObject needle;
    public GameObject needle_overlay;
    public GameObject accent;
    private int maxRotation = 320;
    private int maxSpeed = 250;
    private int maxRevs = 6;

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

    void Start()
    {
        Debug.Log("Script Started!");
        switch (gameObject.tag)
        {
            case "Speedo":
                maxRotation = 320;
                maxSpeed = 250;
                break;
            case "Revs":
                maxRotation = 280;
                maxSpeed = 242;
                maxRevs = 6;
                SetAccentTransparency();
                break;
        }

        Debug.Log("This is a:" + gameObject.tag);
        Debug.Log(circleRatio);
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown("space") && !isMovingNeedle)
        {
            gameManager.changeStatusText("Moving needle!");
            rotateNeedleToSpeed(Random.Range(1, maxSpeed - 1));
            if (gameObject.CompareTag("Revs"))
            {
                SetAccentTransparency();
                Debug.Log("Rotation %: " + getRotationPercentage());
            }
        }

        if (Input.GetKeyDown("e") && !isMovingNeedle && !isSweeping)
        {
            gameManager.changeStatusText("Sweeping needle!");
            StartCoroutine(SweepSequence());
        }

        if (gameObject.CompareTag("Revs"))
        {
            if (getRotationPercentage() > 30f)
            {   
                float newAlpha = (getRotationPercentage() - 30f) / (80f - 30f);
                accent.GetComponent<RevCounterAccentHandler>().SetTransparency(newAlpha);
            }
        }
    }


    private float getRotationPercentage()
    {
        float currentRotation = needle.transform.rotation.eulerAngles.z;
        return currentRotation * 100 / maxRotation;
    }

    private void SetAccentTransparency()
    {
        if (accent.GetComponent<RevCounterAccentHandler>().VibeCheck())
        {
            accent.GetComponent<RevCounterAccentHandler>().SetTransparency(0.0f);
        }
    }

    private IEnumerator SweepSequence()
    {
        isSweeping = true;
        float sweepDuration = 3f;
        yield return StartCoroutine(MoveNeedle(needle.transform.rotation.eulerAngles.z, 0f, 0.2f));
        yield return StartCoroutine(MoveNeedle(needle.transform.rotation.eulerAngles.z, 0f, 0.2f));
        yield return StartCoroutine(MoveNeedle(0f, maxRotation, sweepDuration / 2));
        yield return StartCoroutine(MoveNeedle(maxRotation, 0f, sweepDuration / 2, canUpdateStatus: true));
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
}