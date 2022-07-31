using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemController : MonoBehaviour
{
    [SerializeField] private SOEvent onDestroyByDZone;

    [SerializeField] private GameObject liquid;

    [SerializeField] private bool isOnRake;

    [SerializeField] private bool isGameRunning;


    private MeshRenderer liquidMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        isOnRake = false;
        isGameRunning = true;
        liquid.SetActive(false);
        liquidMeshRenderer = liquid.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
       // if (isOnRake == true) return;
        if (isGameRunning == false) return;

        //RakeStack.Instance.Push(this.gameObject);
        //isOnRake = true;

        

        //Анимация или партиклы здесь
        gameObject.SetActive(false);
        liquid.SetActive(false);
    }

    public void Fill(Material liquid)
    {
        this.liquid.SetActive(true);

        liquidMeshRenderer.material = liquid;
    }

    public void OnGameOver()
    {
        isGameRunning = false;
    }

    public void OnRestartGame()
    {
        isGameRunning = true;
    }

    public void OnDisable()
    {
        isOnRake = false;
        GetComponent<Rigidbody>().freezeRotation = false;
        isGameRunning = true;
        liquid.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DZone"))
        {
            if(!liquid.activeSelf)
            {
                onDestroyByDZone.Raise();
            }

            gameObject.SetActive(false);
            liquid.SetActive(false);
        }
    }
}
