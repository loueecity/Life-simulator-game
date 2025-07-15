using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ResourceDetailsList", menuName = "Scriptable Objects/Levels/XPLevels")]
public class SO_Levels : ScriptableObject
{
    [SerializeField] int[] xpForLevels;

    public int[] getIntArrayLevels()
    {
        return xpForLevels;
    }
}
