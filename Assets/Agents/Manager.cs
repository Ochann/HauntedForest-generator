using System.Collections;
using System.Collections.Generic;
using NPBehave;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<GameObject> forestSpiritObjs;
    public List<GameObject> adventurerObjs;
    public List<GameObject> treasureObjs;

    public GameObject testPlayer;
    // Start is called before the first frame update
    void Start()
    {
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

    public void RemoveObjectByType(GameObject obj, string type)
    {
        List<GameObject> list = null;
        Root tree = null;
        switch (type)
        {
            case "adventurer":
                tree = obj.GetComponent<AdventurerBT>().GetBehaviourTree();
                list = adventurerObjs; break;
            case "forestSpirit":
                tree = obj.GetComponent<ForestSpiritBT>().GetBehaviourTree();
                list = forestSpiritObjs; break;
            case "treasure":
                list = treasureObjs; break;
        }
        if (tree != null) tree.Stop();
        list.Remove(obj);
        Destroy(obj);
    }

    public void RemoveAllObjects()
    {
        foreach (GameObject obj in forestSpiritObjs) 
        {
            ForestSpiritBT BT = obj.GetComponent<ForestSpiritBT>();
            if(BT.GetBehaviourTree() != null)
            {
                BT.GetBehaviourTree().Stop();
            }
            //Debug.Log("stopped BT");
            Destroy(obj, 0.1f);
        }
        forestSpiritObjs.Clear();

        foreach (GameObject obj in adventurerObjs)
        {
            AdventurerBT BT = obj.GetComponent<AdventurerBT>();
            if (BT.GetBehaviourTree() != null)
            {
                BT.GetBehaviourTree().Stop();
            }
            //Debug.Log("stopped BT");
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
}
