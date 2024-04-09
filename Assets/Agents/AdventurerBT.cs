using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

public class AdventurerBT : MonoBehaviour
{
    public bool hasTreasure = false;
    public bool seeSpirit = false;
    public bool seeTreasure = false;

    private Root behaviourTree;
    private Blackboard blackboard;

    private AdventurerMove ref_Move;

    private enum Status
    {
        Wander,
        Flee,
        Attack,
        Seek
    }

    private Status curStatus = Status.Wander;

    // Start is called before the first frame update
    void Start()
    {
        ref_Move = GetComponent<AdventurerMove>();
        SwitchBT(FullBT());
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateStatus();
        //SwitchBT(ChooseBT(curStatus));
    }

    private void UpdateStatus()
    {
        if (hasTreasure)
        {
            if (seeSpirit) curStatus = Status.Attack;
            else curStatus = Status.Wander;
        }
        else
        {
            if (seeSpirit) curStatus = Status.Flee;
            else if (seeTreasure) curStatus = Status.Seek;
            else curStatus = Status.Wander; 
        }
    }

    private void SwitchBT(Root t)
    {
        if(behaviourTree != null) behaviourTree.Stop();
        behaviourTree = t;
        blackboard = behaviourTree.Blackboard;
        behaviourTree.Start();
    }

    //private Root ChooseBT(Status status)
    //{
    //    switch(status)
    //    {
    //        case Status.Wander:
    //            return WanderBT();

    //        case Status.Flee:
    //            return FleeBT();

    //        case Status.Attack:
    //            return AttackBT();

    //        case Status.Seek:
    //            return SeekTreasureBT();

    //        default: return WanderBT();
    //    }
    //}

    private void UpdateBlackboard()
    {

    }

    /***      Actions       ***/
    private void Wander()
    {
        ref_Move.AIMove();
        //Debug.Log("ai wandering...");
    }

    private void AttackSpirit()
    {
        Debug.Log("ai attacking spirit...");
    }

    private void RunAway()
    {
        Debug.Log("ai running away...");
    }

    private void PickUpTreasure()
    {
        Debug.Log("ai picking treasure...");
    }

    /***      Behaviour Trees      ***/
    // BT 1: 
    private Node WanderBehaviour() 
    {
        return new Action(() => Wander());
    }

    private Node FleeBehaviour()
    {
        return new Action(() => RunAway());
    }

    private Node AttackBehaviour()
    {
        return new Action(() => AttackSpirit());
    }

    private Node SeekTreasureBehaviour()
    {
        return new Action(() => PickUpTreasure());
    }

    private Root FullBT()
    {
        Node sel = new Selector(WanderBehaviour(),
                                FleeBehaviour(),
                                AttackBehaviour(),
                                SeekTreasureBehaviour());
        Node service = new Service(0.2f, UpdateBlackboard, sel);
        Root root = new Root(service);
        return root;
    }

}
