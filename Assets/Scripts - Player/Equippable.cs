using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equippable")]

public class Equippable : Item
{
    public EquipType equipType;


    public override void Use()
    {
        Debug.Log("here3");
        //equips this item via the equip manager to the player and removes it from the inventory
        EquipManager.instance.Equip(this);
        RemoveFromInventory();
    }
}
public enum EquipType {ARM, LEG, CHEST};
