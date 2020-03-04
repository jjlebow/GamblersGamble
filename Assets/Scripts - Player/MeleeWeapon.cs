using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/MeleeWeapon")]
public class MeleeWeapon : Equippable
{
    public int damageMod;
    public int startTime;
    public int coolDown;
    public int activeFrames;
    public bool specialUnlocked;

    protected BoxCollider2D weaponHitbox;
    //see if you can add a hitbox variable here that can be assigned per weapon type in each child.

    public override void Attack(){}
}
