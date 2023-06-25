using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Runtime
{
    public class StringFormatting : MonoBehaviour
    {
        public static string IntToRomanNumbers(int number)
        {
            return number switch
            {
                1 => "I",
                2 => "II",
                3 => "III",
                4 => "IV",
                _ => ""
            };
        }
    }
}
