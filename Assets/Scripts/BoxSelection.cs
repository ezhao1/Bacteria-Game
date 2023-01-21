using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSelection : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] Vector2 initialMousePosition, currentMousePosition;
    private BoxCollider2D boxColl;

    // Start is called before the first frame update
    void Start()
    {
        //_lineRenderer.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //on click
        if (Input.GetMouseButtonDown(0))
        {
            _lineRenderer.positionCount = 4;
            initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _lineRenderer.SetPosition(0, new Vector2(initialMousePosition.x, initialMousePosition.y));
            _lineRenderer.SetPosition(1, new Vector2(initialMousePosition.x, initialMousePosition.y));
            _lineRenderer.SetPosition(2, new Vector2(initialMousePosition.x, initialMousePosition.y));
            _lineRenderer.SetPosition(3, new Vector2(initialMousePosition.x, initialMousePosition.y));

            //boxColl = gameObject.AddComponent<BoxCollider2D>();
            //boxColl.isTrigger = true;
           // boxColl.offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }

        //when being held down
        if (Input.GetMouseButton(0))
        {
            currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(currentMousePosition);
            Debug.Log(_lineRenderer);
            _lineRenderer.SetPosition(0, new Vector2(initialMousePosition.x, initialMousePosition.y));
            _lineRenderer.SetPosition(1, new Vector2(initialMousePosition.x, currentMousePosition.y));
            _lineRenderer.SetPosition(2, new Vector2(currentMousePosition.x, currentMousePosition.y));
            _lineRenderer.SetPosition(3, new Vector2(currentMousePosition.x, initialMousePosition.y));

            transform.position = (currentMousePosition + initialMousePosition) / 2;

            //boxColl.size = new Vector2(
                //Mathf.Abs(initialMousePosition.x - currentMousePosition.x),
                //Mathf.Abs(initialMousePosition.y - currentMousePosition.y));
        }

        if (Input.GetMouseButtonUp(0))
        {
            _lineRenderer.positionCount = 0;
            //Destroy(boxColl);
            transform.position = Vector3.zero;
        }
    }
}
