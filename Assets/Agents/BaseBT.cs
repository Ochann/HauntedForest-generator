using System.Collections;
using System.Collections.Generic;
using NPBehave;
using UnityEngine;

public class BaseBT : MonoBehaviour
{
    public Root behaviourTree;
    public Blackboard blackboard;

    public void SwitchBT(Root t)
    {
        if (behaviourTree != null) behaviourTree.Stop();
        behaviourTree = t;
        blackboard = behaviourTree.Blackboard;
        behaviourTree.Start();
    }

    public virtual void UpdateBlackboard() { }
}
