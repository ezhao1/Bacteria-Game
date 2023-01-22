using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingApple : Apple
{
    private float smoothing = 5.0f;

    public override void OnMove()
    {
        Value = UnityEngine.Random.Range(1, 10);
        transform.rotation = Quaternion.Euler(0, 0, 180);
    }
    public void Update()
    {
        base.Update();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 5f);
    }
}
