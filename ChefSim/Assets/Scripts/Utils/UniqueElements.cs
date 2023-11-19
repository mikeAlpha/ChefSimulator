using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueElements
{
    public static string[] SelectRandomUniqueValues(string[] array, int count)
    {
        System.Random random = new System.Random();
        array = array.OrderBy(x => random.Next()).ToArray();
        int numberOfElementsToSelect = count;
        string[] selectedValues = array.Take(numberOfElementsToSelect).ToArray();
        return selectedValues;
    }
}
