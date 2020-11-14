using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteractable : Interactable
{

    protected override void Interact()
    {
        if(player.canInteract)
        {
        	//Manager.instance.currentState = Manager.GameState.DIALOGUE;
            DialogueManager.instance.LoadDialogue("Dialogue", this.name, "AUTUMN 1 TRUE END");
            Manager.instance.NewState(Manager.GameState.DIALOGUE);
        }
    }

    void Update()
    {
    }

    
}
