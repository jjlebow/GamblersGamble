using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopInteractable : Interactable
{
    public GameObject button;
    
    private ShopMenu shopPanel;
    

    // Start is called before the first frame update
    void Start()
    {
        shopPanel = Manager.instance.shopPanel.transform.Find("ShopPanel").GetComponent<ShopMenu>();
        //Manager.instance.shopPanel.SetActive(true);
        //shopPanel = Manager.instance.shopPanel.GetComponent<ShopMenu>();
        //Manager.instance.shopPanel.SetActive(false);
        player = Manager.instance.player;
        button = Manager.instance.shopPanel.transform.Find("ShopPanel/ShopGroup/Shop/ShopCard/Button").gameObject;
        //button = Manager.instance.shopPanel.transform.Find("ShopPanel/ConfirmButton").gameObject;
    }

    protected override void Interact()
    {
        if(player.canInteract)
        {

            if(!firstInteract)
            {
                DialogueManager.instance.LoadDialogue("Dialogue/NPC", name, "INTRO");
                firstInteract = true;
            }
            else
            {
                shopPanel.OpenMenu(button);
                Manager.instance.NewState(Manager.GameState.MENU);
            }

        }
    }
  // Update is called once per frame
    void Update()
    {        //display a new menu type that will show cards available for purchase and shows cards 
        //currently in your deck as well all in the same menu
    }
}
