using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

public class ForestSpiritBT : BaseBT
{
    // temp params for test behaviour tree logic
    public float distAdventurer = 50f;


    private BaseMovement ref_Move;
    // Start is called before the first frame update
    void Start()
    {
        ref_Move = GetComponent<BaseMovement>();
        SwitchBT(FinalBT());
    }

    public override void UpdateBlackboard()
    {
        blackboard["distAdventurer"] = distAdventurer;
    }

    private void ChaseAdventurer()
    {
        Debug.Log("ai attacking spirit...");
    }

    private void Wander()
    {
        Debug.Log("ai wandering...");
    }

    private Root FinalBT()
    {
        Node bb = new BlackboardCondition("distAdventurer",
            Operator.IS_SMALLER, 40f, Stops.IMMEDIATE_RESTART, new Action(() => ChaseAdventurer()));

        Node sel = new Selector(bb,
                                new Action(() => Wander()));

        Node service = new Service(0.2f, UpdateBlackboard, sel);
        Root root = new Root(service);
        return root;
    }
}
