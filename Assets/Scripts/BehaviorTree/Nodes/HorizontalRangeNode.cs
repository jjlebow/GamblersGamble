using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRangeNode : Node
{
    private float range;
    private Transform target;
    private Transform origin;

    public HorizontalRangeNode(float range, Transform target, Transform origin)
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        float horizontalDistance = Mathf.Abs(target.position.x - origin.position.x);
        return horizontalDistance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
