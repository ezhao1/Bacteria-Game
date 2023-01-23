using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBoxHighlightTrail : MonoBehaviour
{
    private Vector3 initialScale;
    // Start is called before the first frame update
    private Color initialColor = Color.white;
    private Color endColor = new Color(1,1,1,0); 
    public float fadeTime = 0.1f;
    private float elapsedTime = 0f;
    void Start()
    {
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        transform.localScale = Vector3.Lerp(transform.localScale, initialScale * 1.5f, Time.deltaTime * 5);
        gameObject.GetComponent<Renderer>().material.color = Color.Lerp(initialColor, endColor, elapsedTime / fadeTime * 2);
        if (elapsedTime >= fadeTime)
        {
            Destroy(gameObject);
        }
    }
}
