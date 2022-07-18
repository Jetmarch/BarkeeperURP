using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RakeStack : MonoBehaviour
{
    [SerializeField] private float distanceBetweenObjects;

    [SerializeField] private Stack<GameObject> stack = new Stack<GameObject>();

    [SerializeField] private bool isGameRunning;

    [SerializeField] private Transform firstWaypointOnRack;

    [SerializeField] private SOEvent coinsUpdate;

    [SerializeField] private SOEvent itemFilledRight;
    [SerializeField] private SOEvent itemFilledWrong;

    [Header("Coins")]
    [SerializeField] private int maxCoinsOnCorrect;
    [SerializeField] private int minCoinsOnCorrect;

    [SerializeField] private int maxCoinsOnIncorrect;
    [SerializeField] private int minCoinsOnIncorrect;


    [Header("Animations")]
    [Range(0, 5f)]
    [SerializeField] private float pushOnRakeDuration;

    [Range(0, 1f)]
    [SerializeField] private float pushOnRakeRandomRotationDuration;

    [Range(0, 5f)] 
    [SerializeField] private float pushPunchScale;

    [Range(0, 5f)]
    [SerializeField] private float pushPunchDuration;

    [Range(0, 20)]
    [SerializeField] private int pushPunchVibrato;

    [Range(0, 20)]
    [SerializeField] private int pushPunchElastisity;

    [Space(10)]
    [Range(0, 5f)]
    [SerializeField] private float popPunchScale;

    [Range(0, 5f)]
    [SerializeField] private float popPunchDuration;

    [Range(0, 20)]
    [SerializeField] private int popPunchVibrato;

    [Range(0, 20)]
    [SerializeField] private int popPunchElastisity;


    public static RakeStack Instance { private set; get; }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isGameRunning = true;
        DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Push(GameObject item)
    {
        item.GetComponent<Rigidbody>().velocity = Vector3.zero;

        stack.Push(item);


        if (item.transform.localPosition.y <= -1.5f /* Ниже этого значения при поимке бокала происходит клиппинг анимации */)
        {
            item.transform.position = this.gameObject.transform.position;
        }
        else
        {
            item.transform.DOMove(this.gameObject.transform.position, pushOnRakeDuration);
        }

        item.transform.DORotate(new Vector3(0f, Random.rotation.eulerAngles.y, 0f), pushOnRakeRandomRotationDuration);

        item.transform.DOPunchScale(Vector3.up * pushPunchScale, pushPunchDuration, pushPunchVibrato, pushPunchElastisity);
        item.GetComponent<Rigidbody>().freezeRotation = true;


        MoveOtherGlassOnRack();
    }

    public void FillAndPop(Material liquidMaterial)
    {
        if(stack.Count <= 0)
        {
            return;
        }

        var item = stack.Pop();

        item.GetComponent<ItemController>().Fill(liquidMaterial);

        var sequence = DOTween.Sequence();

        sequence.Append(item.transform.DOPunchScale(Vector3.up * popPunchScale, popPunchDuration, popPunchVibrato, popPunchElastisity));
        sequence.AppendCallback(() => {
            item.SetActive(false);
        });

        var liquid = item.GetComponentInChildren<Tiket>().GetLiquid();

        if(liquidMaterial == liquid.liquid)
        {
            int coins = Random.Range(minCoinsOnCorrect, maxCoinsOnCorrect);
            itemFilledRight.Raise();
            coinsUpdate.Raise(new SOUpdateCoinsCounterEventArgs(coins));
        }
        else
        {
            int coins = Random.Range(minCoinsOnIncorrect, maxCoinsOnIncorrect);
            itemFilledWrong.Raise();
            coinsUpdate.Raise(new SOUpdateCoinsCounterEventArgs(coins));
        }

        MoveOtherGlassOnRack();
    }

    private void MoveOtherGlassOnRack()
    {
        int counter = 0;
        foreach (var item in stack)
        {
            item.transform.rotation = Quaternion.identity;
            Vector3 newPosition = new Vector3((this.gameObject.transform.position.x + distanceBetweenObjects) * counter,
               item.transform.position.y, this.gameObject.transform.position.z);

            item.transform.DOMove(newPosition, 0.3f);

            counter++;
        }
    }

    public void OnBottleClick(SOEventArgs e)
    {
        SOOnBottleClickArgs args = (SOOnBottleClickArgs)e;

        FillAndPop(args.colorOfLiquid);
    }

    public void OnStartGame()
    {
        ClearRake();
        isGameRunning = true;
    }

    public void OnGameRestart()
    {
        OnStartGame();
    }

    public void OnGameOver()
    {
        isGameRunning = false;

        ClearRake();
    }

    private void ClearRake()
    {
        foreach(var item in stack)
        {
            item.SetActive(false);
        }
        stack.Clear();
    }
}
