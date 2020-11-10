using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryMenu : MonoBehaviour
{
    Inventory inventory;
    public GameObject inventoryMenu;

    public Transform itemsParent;

    ItemSlot[] slots;

    private void Start()
    {
        inventory = Inventory.instance;
        inventory.OnItemChangeCallback += OnUpdate;
        //creates an array with the number of components(inventory slots) that you have in the heirarchy
        slots = itemsParent.GetComponentsInChildren<ItemSlot>();
    }

    void Update()
    {
        //checks if inventory menus is brought up or put away
//        if(Input.GetKeyDown(KeyCode.I))  
//        {
//            inventoryMenu.SetActive(!inventoryMenu.activeSelf);  
//        }
    }
    
    private void OnUpdate()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.items.Count) //adds the total number of items currently in inventory into the slots
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot(); //clears unused slots
            }
        }
    }
}
