using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rapier : MeleeWeapon
{

    public void Start()
    {
        //weaponHitbox = GameObject.GetComponent<BoxCollider2D>();
    }
    public override void Attack()
    {
        base.Attack(); //in case you want to add something that applies to all attacks
        //do animations in here too
    }
}
