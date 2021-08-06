
using UnityEngine;

public class SafetyChecks
{
    public static void CheckInRangeAndThrow(int v, Vector2 range, string name) {
        if(v < range.x || v > range.y) 
            throw new System.Exception("Property " + name + " must in range " + range.x + "," + range.y);
    }
}