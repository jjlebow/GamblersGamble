using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : Interactable
{
    public Item item;

    protected override void Interact()
    {
        //Debug.Log("The item is being interacted with"); 
        Pickup();
    }

    private void Pickup()
    {
        //Debug.Log(("Picking up " + item.name));
        Inventory.instance.Add(item);
        Destroy(gameObject);
    }
}
