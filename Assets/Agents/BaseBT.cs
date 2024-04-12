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

    private void OnDestroy()
    {
        
    }

    public Root GetBehaviourTree() { return behaviourTree; }

    public void ChangeColor(Color newColor, Color original)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = original;
        spriteRenderer.color = newColor;
    }

    public virtual void UpdateBlackboard() { }
}
