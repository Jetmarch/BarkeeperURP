using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameDifficultyPreset currentGamePreset;

    [SerializeField] private GameDifficultyPreset defaultGamePreset;

    [SerializeField] private List<ObjectPool> pools;

    [SerializeField] private bool isGameRunning;

    [SerializeField] private List<LiquidType> liquids;

    [SerializeField] private SOEvent onItemSpawn;

    private Coroutine spawnCoroutine;

    void Start()
    {
        //InvokeRepeating("SpawnRandomItem", startDelay, spawnRate); // Redo with coroutine
    }

    public void OnStartGame(SOEventArgs e)
    {
        var startGameEvent = (SOStartGameEventArgs)e;

        currentGamePreset = startGameEvent.preset;

        isGameRunning = true;


        //spawnCoroutine = StartCoroutine(SpawnRandomItem());
    }

    private void OnStartGame()
    {
        isGameRunning = true;
      //  spawnCoroutine = StartCoroutine(SpawnRandomItem());
    }

    public void OnRoundOver()
    {
        isGameRunning = false;
      //  StopCoroutine(spawnCoroutine);
    }

    public void OnRestartGame()
    {
        StopAllCoroutines();
        OnStartGame();
    }

    public void OnMusicBeat()
    {
        //SpawnRandomItemWithChanceOfDouble();
        SpawnItem();
    }

    private void SpawnRandomItemWithChanceOfDouble()
    {
        float rndValue = Random.value;

        if (rndValue <= currentGamePreset.chanceOfDoubleSpawn)
        {
            SpawnItem();
            SpawnItem();
        }
        else
        {
            SpawnItem();
        }
    }

    private IEnumerator StartSpawnCicle()
    {
        //Добавить спав сразу нескольких заказов
        //Шанс зависит от сложности
        while (isGameRunning)
        {
            yield return new WaitForSeconds(currentGamePreset.spawnRate);

            float rndValue = Random.value;

            if(rndValue <= currentGamePreset.chanceOfDoubleSpawn)
            {
                SpawnItem();
                SpawnItem();
            }
            else
            {
                SpawnItem();
            }

        }
    }

    private void SpawnItem()
    {
        var rndItem = pools[Random.Range(0, pools.Count)].GetNewItem();
        rndItem.SetActive(true);
        Vector3 spawnPosition = new Vector3(Random.Range(currentGamePreset.spawnMinCoords.x, currentGamePreset.spawnMaxCoords.x),
                Random.Range(currentGamePreset.spawnMinCoords.y, currentGamePreset.spawnMaxCoords.y),
                Random.Range(currentGamePreset.spawnMinCoords.z, currentGamePreset.spawnMaxCoords.z));

        float tossForce = Random.Range(currentGamePreset.minTossForce, currentGamePreset.maxTossForce);
        float torqueForce = Random.Range(currentGamePreset.minTorqueForce, currentGamePreset.maxTorqueForce);

        rndItem.transform.position = spawnPosition;

        rndItem.GetComponent<Rigidbody>().velocity = Vector3.zero;

        rndItem.GetComponent<Rigidbody>().AddForce(Vector3.up * tossForce);
        rndItem.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0, 10f), Random.Range(0, 10f), Random.Range(0, 10f)) * torqueForce);

        var itemTiket = rndItem.GetComponentInChildren<Tiket>();
        itemTiket.SetLiquidType(liquids[Random.Range(0, liquids.Count)]);

        onItemSpawn.Raise();
    }

   
}
