using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressBar : MonoBehaviour
{
    RectTransform t;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        t.sizeDelta = Vector2.Lerp(t.sizeDelta, new Vector2(1600f * GameManager.Instance.ApplesClearedInLevel / GameManager.Instance.CurrentLevelSettings.requiredToClear, 30), Time.deltaTime * 5f); ;
    }
}
