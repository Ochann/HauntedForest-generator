using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Dictionary<string, GameObject> gameObjects;
    // Start is called before the first frame update
    void Start()
    {
        gameObjects = new Dictionary<string, GameObject>();
    }

    public void AddObject(GameObject obj)
    {
        gameObjects.Add(obj.name, obj);
        Debug.Log(obj.name);
    }

    public GameObject FindGameObjByName(string name)
    {
        GameObject obj;
        if(gameObjects.TryGetValue(name, out obj)) { return obj; }
        return null;
    }
   
}
