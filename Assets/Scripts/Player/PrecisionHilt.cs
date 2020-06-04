using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionHilt : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
    	transform.parent.gameObject.GetComponent<PrecisionAttack>().hilt = true;
    	transform.parent.gameObject.GetComponent<PrecisionAttack>().DamageCheck(col);
    }
}
