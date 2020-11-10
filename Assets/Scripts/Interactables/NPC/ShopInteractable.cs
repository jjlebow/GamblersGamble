using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopInteractable : Interactable
{
    public GameObject button;

    // Start is called before the first frame update
    void Start()
    {
        player = Manager.instance.player;
        button = Manager.instance.shopPanel.transform.Find("ShopPanel/Shop/ShopCard/Button").gameObject;
    }

    protected void Interact()
    {
        Debug.Log(button.name);
        if(player.canInteract)
        {
            m_eventSystem = EventSystem.current;
            Manager.instance.shopPanel.SetActive(true);
            //Deck.instance.DrawCards(Deck.instance.drawnCards);
            Manager.instance.NewState(Manager.GameState.MENU);
            m_eventSystem.SetSelectedGameObject(button);

        }
    }
  // Update is called once per frame
    void Update()
    {        //display a new menu type that will show cards available for purchase and shows cards 
        //currently in your deck as well all in the same menu
    }
}
