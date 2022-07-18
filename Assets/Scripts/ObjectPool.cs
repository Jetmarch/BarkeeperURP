using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private List<GameObject> objects = new List<GameObject>();
    [SerializeField] private int poolSize;


    private void Start()
    {
        for(int i = 0; i < poolSize; i++)
        {
            var obj = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, this.gameObject.transform);
            objects.Add(obj);
            obj.SetActive(false);
        }
    }

    public GameObject GetNewItem()
    {
        foreach(var obj in objects)
        {
            if(obj.activeSelf == false)
            {
                return obj;
            }
        }

        return objects[0];
    }

    public void OnRestart()
    {
        for (int i = 0; i < poolSize; i++)
        {
            objects[i].SetActive(false);
        }
    }
}
