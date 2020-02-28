using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    //"new" overrides reserved definition of name and allows us to use this instead
    new public string name = "New Item";  
    public Sprite icon = null;
    //public Sprite sprite? for the actual sprite render in the game?
    public bool isDefaultItem = false;

    public virtual void Use(){}
    public virtual void Attack(){}

    public void RemoveFromInventory()
    {
        //removes the item that was used from the inventory
        Inventory.instance.Remove(this);
    }
}

