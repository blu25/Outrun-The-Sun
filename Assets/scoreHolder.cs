using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class scoreHolder
{
	// Your score
    public static int place = 0;
    public static float distance = 0;
    public static float time = 0;

    // High score
    public static float maxDistance = 0;
    public static float maxTime = 0;

    public static string GetOrdinalSuffix(int num) {
        string number = num.ToString();
        if (number.EndsWith("11")) return "th";
        if (number.EndsWith("12")) return "th";
        if (number.EndsWith("13")) return "th";
        if (number.EndsWith("1")) return "st";
        if (number.EndsWith("2")) return "nd";
        if (number.EndsWith("3")) return "rd";
        return "th";
    }
}
