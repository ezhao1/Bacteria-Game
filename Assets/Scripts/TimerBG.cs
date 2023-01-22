//from video at https://youtu.be/6C1NPy321Nk
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBG : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points;
    public float amplitude = 1;
    public float frequency = 1;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;
    [Range(0, 2 * Mathf.PI)]
    public float radians;

    private float initialTime;
    public float bottomYCoord = -3;
    public float topYCoord = 10;
    private float yOffset;

    void Awake()
    {
        yOffset = bottomYCoord;
    }

    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
        initialTime = GameManager.Instance.targetTime;
        myLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        myLineRenderer.SetColors(Color.white, Color.white);
    }

    void Draw()
    {
        float xStart = xLimits.x;
        float Tau = 2 * Mathf.PI;
        float xFinish = xLimits.y;

        myLineRenderer.positionCount = points;
        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = amplitude * Mathf.Sin((Tau * frequency * x) + (Time.timeSinceLevelLoad * movementSpeed)) + Mathf.PerlinNoise(x*frequency,0);
            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y + yOffset, 0.1f));
        }
    }
    float GetTargetOffset()
    {
        return GameManager.Instance.targetTime / initialTime * (topYCoord - bottomYCoord) + bottomYCoord;
    }

    void Update()
    {
        float target = GetTargetOffset();
        yOffset = Mathf.Lerp(yOffset, target, Time.deltaTime);
        Draw();
    }
}