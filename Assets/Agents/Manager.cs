using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    //public Dictionary<string, GameObject> gameObjects;

    public List<GameObject> forestSpiritObjs;
    public List<GameObject> adventurerObjs;
    public List<GameObject> treasureObjs;

    public GameObject testPlayer;
    // Start is called before the first frame update
    void Start()
    {
        //gameObjects = new Dictionary<string, GameObject>();

        forestSpiritObjs = new List<GameObject>();
        adventurerObjs = new List<GameObject>();
        treasureObjs = new List<GameObject>();
    }

    public void AddForestSpirit(GameObject obj)
    {
        forestSpiritObjs.Add(obj);
    }

    public void AddAdventurer(GameObject obj)
    {
        adventurerObjs.Add(obj);
    }

    public void AddTreasure(GameObject obj)
    {
        treasureObjs.Add(obj);
    }

    public void RemoveAllObjects()
    {
        //foreach (KeyValuePair<string, GameObject> entry in gameObjects)
        //{
        //    Destroy(entry.Value, 0.1f);
        //}
        //gameObjects.Clear();
        foreach (GameObject obj in forestSpiritObjs) 
        {
            Destroy(obj, 0.1f);
        }
        forestSpiritObjs.Clear();

        foreach (GameObject obj in adventurerObjs)
        {
            Destroy(obj, 0.1f);
        }
        adventurerObjs.Clear();

        foreach (GameObject obj in treasureObjs)
        {
            Destroy(obj, 0.1f);
        }
        treasureObjs.Clear();
    }


    public List<Transform> GetTransforms()
    {
        List<Transform> targets = new List<Transform>();

        foreach (GameObject obj in forestSpiritObjs)
        {
            Transform transform = obj.transform;
            targets.Add(transform);
        }
        foreach (GameObject obj in adventurerObjs)
        {
            Transform transform = obj.transform;
            targets.Add(transform);
        }
        foreach (GameObject obj in treasureObjs)
        {
            Transform transform = obj.transform;
            targets.Add(transform);
        }

        targets.Add(testPlayer.transform);

        return targets;
    }

    //public GameObject FindGameObjByName(string name)
    //{
    //    GameObject obj;
    //    if(gameObjects.TryGetValue(name, out obj)) { return obj; }
    //    return null;
    //}

    //public void AddObject(GameObject obj)
    //{
    //    gameObjects.Add(obj.name, obj);
    //    Debug.Log(obj.name);
    //}

}
