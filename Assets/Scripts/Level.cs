using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Create Level", order = 1)]
public class Level : ScriptableObject
{
    public int additionalTime;
    public int requiredToClear;
    public int width;
    public int height;
    public float changingAppleChance;
    public float negativeAppleChance;
    public float halvingAppleChance;


}