using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Liquid type", menuName = "Liquid type")]
public class LiquidType : ScriptableObject
{
    public string liquidName;
    public Material liquid;
}
