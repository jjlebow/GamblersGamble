using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{


    public Equippable[] currentEquipment;  //unserialize this when testing is done

    public delegate void OnEquipmentChanged(Equippable newItem, Equippable oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;

    #region Singleton
    

    public static EquipManager instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found");
            return;
        }
        instance = this;
    }

    #endregion

    void Start()
    {
        //numSlots will contain the size of the enum EquipType defined in Equippable.cs
        //and initializes an array of equippables of this size
        int numSlots = System.Enum.GetNames(typeof(EquipType)).Length;
        currentEquipment = new Equippable[numSlots];  //this represents what the player currently has equipped
        inventory = Inventory.instance;
        //if(currentEquipment[(int)System.Enum.equipType.ARM] == null)
        //{
            //Debug.Log("we are here");
        //}
    }

    public void Equip(Equippable newItem)
    {
        //this turns the enums into integer values which allows us to index the equipment type based on the parameter
        int slotIndex = (int)newItem.equipType;
        Equippable oldItem = null;

        //this swaps the currently equipped item in the inventory if one is equipped
        if(currentEquipment[slotIndex] != null)
        {   
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);  //re-adds the old item to the inventory after its unequipped
        }

        if(onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        inventory.Remove(newItem);  //this removes the item you are trying to equip from your inventory
        //if one is not equipped it just equips the new item
        currentEquipment[slotIndex] = newItem;
    }

    public void Unequip(int slotIndex)
    {
        Equippable oldItem = null;
        if(currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;
        }

        if(onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(null, oldItem);
        }
    }
}
