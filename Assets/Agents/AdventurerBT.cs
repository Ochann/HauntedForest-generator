using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

public class AdventurerBT : MonoBehaviour
{
    public bool hasTreasure = false;
    //public bool seeSpirit = false;
    //public bool seeTreasure = false;

    public float distTreasure = 50f;
    public float distSpirit = 50f;

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

    /*
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
    }*/

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
        blackboard["hasTreasure"] = hasTreasure;
        //blackboard["seeSpirit"] = seeSpirit;
        //blackboard["seeTreasure"] = seeTreasure;

        blackboard["distSpirit"] = distSpirit;
        blackboard["distTreasure"] = distTreasure;
    }

    /***      Actions       ***/
    private void Move(float velocity)
    {
        ref_Move.AIMove(velocity);
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

    /***      Behaviour Node: MoveBehaviour      ***/
    // BT 1: 
    private Node SetVelocity(float velocity)
    {
        return new Action(() => Move(velocity));
    }

    // Move forward at full speed for a random time
    private Node RandomMove()
    {
        float waitTime = UnityEngine.Random.Range(0.1f, 1.0f);
        return new Sequence(SetVelocity(1f),
                            new Wait(waitTime),
                            SetVelocity(0));
    }
    private Node MoveBehaviour() 
    {
        Node seq = new Sequence(RandomMove());
        Node bb = new BlackboardCondition("distSpirit", 
            Operator.IS_GREATER, 40f, Stops.IMMEDIATE_RESTART, seq);

        return bb;
    }

    /***      Behaviour Node: FleeBehaviour      ***/
    private Node FleeBehaviour()
    {
        return new Action(() => RunAway());
    }

    /***      Behaviour Node: AttackBehaviour      ***/
    private Node AttackBehaviour()
    {
        return new Action(() => AttackSpirit());
    }

    private Node SeekTreasureBehaviour()
    {
        return new Action(() => PickUpTreasure());
    }


    // 1.adventurer with treasure will attack forest spirit while seeing it
    // 2.wander if not see
    private Node WithTreasureNode()
    {
        Node sel = new Selector(MoveBehaviour(),
                                AttackBehaviour());
        Node bb = new BlackboardCondition("hasTreasure",
            Operator.IS_EQUAL, 1, Stops.IMMEDIATE_RESTART, sel);

        return bb;
    }

    // 1.adventurer without treasure will run away from forest spirit while seeing it
    // 2.close and pick up treasure if see
    // 3.wander if not see anything
    // how to determine the priority of flee/seek treasure actions?
    private Node WithoutTreasureNode()
    {
        Node sel = new Selector(MoveBehaviour(),
                                FleeBehaviour(),
                                SeekTreasureBehaviour());
        return sel;
    }

    private Root FullBT()
    {
        Node sel = new Selector(MoveBehaviour(),
                                FleeBehaviour(),
                                AttackBehaviour(),
                                SeekTreasureBehaviour());
        Node service = new Service(0.2f, UpdateBlackboard, sel);
        Root root = new Root(service);
        return root;
    }

    private Root FinalBT()
    {
        Node sel = new Selector(WithTreasureNode(),
                                WithoutTreasureNode());
        Node service = new Service(0.2f, UpdateBlackboard, sel);
        Root root = new Root(service);
        return root;
    }

}
