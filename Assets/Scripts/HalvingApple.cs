using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalvingApple : Apple
{
    public override void OnClear()
    {
        if (this.Value == 1)
        {
            AnimateDelete();
        }
        else
        {
            this.Value -= this.Value/2;
            BeginShake();
        }
    }


    //Shaking code stolen from https://forum.unity.com/threads/shake-an-object-from-script.138382/

    [Header("Info")]
    private Vector3 _startPos;
    private float _timer;
    private Vector3 _randomPos;

    [Header("Shake Settings")]
    [Range(0f, 2f)]
    public float _shakeTime = 0.2f;
    [Range(0f, 2f)]
    public float _shakeDistance = 0.1f;
    [Range(0f, 0.1f)]
    public float _delayBetweenShakes = 0f;

    private void Awake()
    {
        base.Awake();
        _startPos = transform.position;
    }

    private void OnValidate()
    {
        if (_delayBetweenShakes > _shakeTime)
            _delayBetweenShakes = _shakeTime;
    }

    public void BeginShake()
    {
        _startPos = transform.position;
        StopAllCoroutines();
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        _timer = 0f;

        while (_timer < _shakeTime)
        {
            _timer += Time.deltaTime;

            _randomPos = _startPos + (Random.insideUnitSphere * _shakeDistance);

            transform.position = _randomPos;

            if (_delayBetweenShakes > 0f)
            {
                yield return new WaitForSeconds(_delayBetweenShakes);
            }
            else
            {
                yield return null;
            }
        }

        transform.position = _startPos;
    }
}
