using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

public class AdventurerBT : BaseBT
{
    // public params for test behaviour tree logic
    public bool hasTreasure = false;
    public float distTreasure;
    public float distSpirit;

    public float distTreasureAlert = 20f;
    public float distSpiritAlert = 20f;


    private BaseMovement ref_Move;
    private GameObject nearestForestSpirit;
    private GameObject nearestTreasure;

    private Color originalColor = new Color(127, 220, 238);


    // Start is called before the first frame update
    void Start()
    {
        ref_Move = GetComponent<BaseMovement>();
        SwitchBT(FinalBT());
    }


    public override void UpdateBlackboard()
    {
        nearestForestSpirit = ref_Move.GetNearestObjByType("forestSpirit");
        nearestTreasure = ref_Move.GetNearestObjByType("treasure");

        if(nearestForestSpirit != null) distSpirit = Vector3.Distance(transform.position, nearestForestSpirit.transform.position);
        if(nearestTreasure != null) distTreasure = Vector3.Distance(transform.position, nearestTreasure.transform.position);

        blackboard["hasTreasure"] = hasTreasure;
        blackboard["distSpirit"] = distSpirit;
        blackboard["distTreasure"] = distTreasure;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("on collide");
        if (other.gameObject.CompareTag("Treasure"))
        {
            hasTreasure = true;
            ref_Move.RemoveObject(other.gameObject, "treasure");
            ChangeColor(Color.red, originalColor);
            distTreasure = 100f;
        }

        else if(other.gameObject.CompareTag("ForestSpirit"))
        {
            if (hasTreasure)
            {
                ref_Move.RemoveObject(other.gameObject, "forestSpirit");
                distSpirit = 100f;
            }
            else
            {
                ref_Move.RemoveObject(this.gameObject, "adventurer");
            }
        }
    }

    public bool IfHasTreasure()
    {
        return hasTreasure;
    }

    /***      Actions       ***/
    private void Wander()
    {
        ref_Move.Wander();
        if (hasTreasure) ChangeColor(Color.red, originalColor); 
        else ChangeColor(originalColor, originalColor);
        //Debug.Log("ai moving...");
    }

    private void AttackSpirit()
    {
        //Debug.Log("ai attacking spirit...");
        ref_Move.ChaseObject(nearestForestSpirit);
    }

    private void RunAway()
    {
        //Debug.Log("ai running away...");
        ChangeColor(Color.white, originalColor);
        ref_Move.FleeFromObj(nearestForestSpirit);
    }

    private void PickUpTreasure()
    {
        //Debug.Log("ai picking treasure...");
        ref_Move.ChaseObject(nearestTreasure);
    }



    /***      Behaviour Node: MoveBehaviour      ***/
    // BT 1: 
    //private Node SetVelocity(float velocity)
    //{
    //    return new Action(() => Move(velocity));
    //}

    private Node WanderBehaviour()
    {
        return new Action(() => Wander());
    }

    // Move forward at full speed for a random time
    private Node RandomMove()
    {
        float waitTime = UnityEngine.Random.Range(2.0f, 5.0f);
        return new Sequence(WanderBehaviour(),
                            new Wait(waitTime),
                            WanderBehaviour());
    }
    private Node MoveBehaviour() 
    {
        Node seq = new Sequence(RandomMove());
        Node bb = new BlackboardCondition("distSpirit", 
            Operator.IS_GREATER, distSpiritAlert, Stops.IMMEDIATE_RESTART, seq);

        return bb;
    }

    /***      Behaviour Node: FleeBehaviour      ***/
    private Node FleeBehaviour()
    {
        Node bb = new BlackboardCondition("distSpirit",
            Operator.IS_SMALLER, distSpiritAlert, Stops.IMMEDIATE_RESTART, new Action(() => RunAway()));
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
            Operator.IS_SMALLER, distTreasureAlert, Stops.IMMEDIATE_RESTART, new Action(() => PickUpTreasure()));
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
