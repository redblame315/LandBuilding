using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static string FloatString(string _inputString)
    {
        return string.IsNullOrEmpty(_inputString) ? "0" : _inputString;
    }
}
