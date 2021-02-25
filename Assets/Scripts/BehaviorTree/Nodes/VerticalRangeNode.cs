using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalRangeNode : Node
{
    private float range;
    private Transform target;
    private Transform origin;

    public VerticalRangeNode(float range, Transform target, Transform origin)
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        float verticalDistance = Mathf.Abs(target.position.y - origin.position.y);
        return verticalDistance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
