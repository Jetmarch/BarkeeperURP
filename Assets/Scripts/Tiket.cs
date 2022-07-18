using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiket : MonoBehaviour
{
    [SerializeField] private LiquidType liquid;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private void Awake()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    private void OnDisable()
    {
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
    }

    public void SetLiquidType(LiquidType liquid)
    {
        this.liquid = liquid;

        GetComponent<MeshRenderer>().material = liquid.liquid;
    }

    public LiquidType GetLiquid()
    {
        return liquid;
    }
}
