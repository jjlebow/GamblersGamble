using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rapier : MeleeWeapon
{
    public void Start()
    {
        equipType = EquipType.ARM;
        weaponHitbox = gameObject.GetComponent<BoxCollider2D>();
    }
    public override void Use()
    {
        EquipManager.instance.currentEquipment[(int)EquipType.ARM].Use();  //this is the line of code that will call the Use() function on the item stored in the ARM index of the inventory. 
        //move this line to the player controller when youve found where to put it. 


        //make some kind of timer maybe relating to a timer class that makes use of the 3 cooldown times and this function triggers annimatinos, sounds, etc. 
        weaponHitbox.gameObject.SetActive(true);  //.SetActive(!activeSelf);
        base.Attack(); //in case you want to add something that applies to all attacks
        //do animations in here too
    }
}
