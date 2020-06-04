using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicFunctions : MonoBehaviour
{

	//Finds and returns the uppermost parent of an object
    public static Transform FindParent(Transform hitParent)
    {
    	while(true)
    	{
    		if(hitParent.parent != null)
    			hitParent = hitParent.parent;
    		else
    			break;
    	}
    	return hitParent;
    }
}
