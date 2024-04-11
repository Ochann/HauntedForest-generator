using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

public class AdventurerBT : BaseBT
{
    // temp params for test behaviour tree logic
    public bool hasTreasure = false;
    public float distTreasure = 50f;
    public float distSpirit = 50f;


    private BaseMovement ref_Move;


    // Start is called before the first frame update
    void Start()
    {
        ref_Move = GetComponent<BaseMovement>();

        SwitchBT(FinalBT());
    }

    private void OnDestroy()
    {
        if(behaviourTree != null) behaviourTree.Stop();
    }

    public override void UpdateBlackboard()
    {
        blackboard["hasTreasure"] = hasTreasure;
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
        ref_Move.ChaseObject("Player");
    }

    private void RunAway()
    {
        //Debug.Log("ai running away...");
    }

    private void PickUpTreasure()
    {
        //Debug.Log("ai picking treasure...");
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
        Node bb = new BlackboardCondition("distSpirit",
            Operator.IS_SMALLER, 40f, Stops.IMMEDIATE_RESTART, new Action(() => RunAway()));
        return bb;
    }

    /***      Behaviour Node: AttackBehaviour      ***/
    private Node AttackBehaviour()
    {
        return new Action(() => AttackSpirit());
    }

    private Node SeekTreasureBehaviour()
    {
        Node bb = new BlackboardCondition("distTreasure",
            Operator.IS_SMALLER, 40f, Stops.IMMEDIATE_RESTART, new Action(() => PickUpTreasure()));
        return bb;
    }


    // 1.adventurer with treasure will attack forest spirit while seeing it
    // 2.wander if not see
    private Node WithTreasureNode()
    {
        Node sel = new Selector(MoveBehaviour(),
                                AttackBehaviour());
        Node bb = new BlackboardCondition("hasTreasure",
            Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART, sel);

        return bb;
    }

    // 1.adventurer without treasure will run away from forest spirit while seeing it
    // 2.close and pick up treasure if see
    // 3.wander if not see anything
    // Q: how to determine the priority of flee/seek treasure actions?
    private Node WithoutTreasureNode()
    {  
        Node sel1 = new Selector(SeekTreasureBehaviour(), new Sequence(RandomMove()));
        Node sel = new Selector(FleeBehaviour(), sel1);

        return sel;
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
