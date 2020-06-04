using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equippable")]
public enum EquipType {ARM, LEG, CHEST};  //we are first defining a type EquipType here that is an enum container
public class Equippable : Item
{
    public EquipType equipType;   //we are defining a variable of that type here that can only be accessed in Item classes


    public override void Equip()
    {
        Debug.Log("here3");
        //equips this item via the equip manager to the player and removes it from the inventory
        EquipManager.instance.Equip(this);
        RemoveFromInventory();
    }
}

