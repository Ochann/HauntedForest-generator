using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

public class ForestSpiritBT : BaseBT
{
    // temp params for test behaviour tree logic
    public float distAdventurer;

    public float distAdventurerAlert = 10f;

    public bool hasTreasure = false;

    //private Color originalColor = new Color(255, 179, 207);
    private Color originalColor;


    private BaseMovement ref_Move;

    private GameObject nearestAdventurer;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        ref_Move = GetComponent<BaseMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        SwitchBT(FinalBT());

        
    }



    public override void UpdateBlackboard()
    {
        nearestAdventurer = ref_Move.GetNearestObjByType("adventurer");

        if (nearestAdventurer != null)
        {
            distAdventurer = Vector3.Distance(transform.position, nearestAdventurer.transform.position);
            hasTreasure = nearestAdventurer.GetComponent<AdventurerBT>().IfHasTreasure();
        }

        blackboard["distAdventurer"] = distAdventurer;
        blackboard["hasTreasure"] = hasTreasure;
    }

    private void ChaseAdventurer()
    {
        Debug.Log("ai attacking adventurer...");
        ChangeColor(Color.red, originalColor);
        ref_Move.ChaseObject(nearestAdventurer);
    }

    private void Wander()
    {
        Debug.Log("ai wandering...");
        ChangeColor(originalColor, originalColor);
        ref_Move.Wander();
    }

    private void Flee()
    {
        Debug.Log("ai flee...");
        ChangeColor(Color.white, originalColor);
        ref_Move.FleeFromObj(nearestAdventurer);
    }

    private Node WanderBehaviour()
    {
        return new Action(() => Wander());
    }

    private Node FleeBehaviour()
    {
        return new Action(() => Flee());
    }

    // Move forward at full speed for a random time
    private Node RandomMove()
    {
        float waitTime = UnityEngine.Random.Range(2.0f, 5.0f);
        return new Sequence(WanderBehaviour(),
                            new Wait(waitTime),
                            WanderBehaviour());
    }

    private Node InteractWithAdventurer()
    {
        Node bb = new BlackboardCondition("hasTreasure",
            Operator.IS_EQUAL, false, Stops.IMMEDIATE_RESTART, new Action(() => ChaseAdventurer()));

        Node sel = new Selector(bb, FleeBehaviour());

        return sel;
    }

    private Root FinalBT()
    {
        Node bb = new BlackboardCondition("distAdventurer",
            Operator.IS_SMALLER, distAdventurerAlert, Stops.IMMEDIATE_RESTART, InteractWithAdventurer());

        Node sel = new Selector(bb,
                                RandomMove());

        Node service = new Service(0.2f, UpdateBlackboard, sel);
        Root root = new Root(service);
        return root;
    }
}
