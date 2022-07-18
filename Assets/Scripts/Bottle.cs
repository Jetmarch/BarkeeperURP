using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bottle : MonoBehaviour
{
    [SerializeField] private LiquidType liquid;
    [SerializeField] private SOEvent onBottleClickEvent;
    [SerializeField] private KeyCode activateButton;

    [SerializeField] private bool isGameRunning;

    private void Awake()
    {
        DOTween.Init();
        isGameRunning = true;
    }

    private void Start()
    {
        GetComponent<MeshRenderer>().material = liquid.liquid;
    }

    private void OnMouseDown()
    {
        ActivateBottle();
    }

    private void ActivateBottle()
    {
        this.transform.DOPunchScale(Vector3.one * 0.02f, 0.1f);

        if (isGameRunning)
        {
            FillFirstGlass();
        }
    }

    public void FillFirstGlass()
    {
        onBottleClickEvent.Raise(new SOOnBottleClickArgs() { colorOfLiquid = this.liquid.liquid });
    }

    public void DisableBottle()
    {
        isGameRunning = false;
    }

    public void EnableBottle()
    {
        isGameRunning = true;
    }

    private void Update()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (Input.GetKeyDown(activateButton))
        {
            ActivateBottle();
        }
#endif
    }
}



/*
 * Заметка на будущее
 *  Собрать всех наследников SOEventArgs в одном файле, а не раскидывать по разным
 */
public class SOOnBottleClickArgs : SOEventArgs
{
    public Material colorOfLiquid;
}