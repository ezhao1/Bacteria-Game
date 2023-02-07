using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyIn : MonoBehaviour
{
    public Vector2 flyFrom;
    private Vector3 initialPosition;
    

    public float lerpFactor = 5f;

    public float delayTime = 0.1f;
    private float elapsedTime = 0f;

    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        initialPosition = rect.anchoredPosition;
        rect.anchoredPosition = flyFrom + rect.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime < delayTime)
        {

            elapsedTime += Time.deltaTime;
        } else
        {

            rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, initialPosition, Time.deltaTime * lerpFactor);
        }
        
    }
}
