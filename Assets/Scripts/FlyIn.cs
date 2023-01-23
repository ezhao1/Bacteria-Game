using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyIn : MonoBehaviour
{
    public Vector3 flyFrom;
    private Vector3 initialPosition;
    

    public float lerpFactor = 5f;

    public float delayTime = 0.1f;
    private float elapsedTime = 0f;

    void Awake()
    {
        initialPosition = transform.position;
        transform.position = flyFrom + transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime < delayTime)
        {

            elapsedTime += Time.deltaTime;
        } else
        {

            transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * lerpFactor);
        }
        
    }
}
