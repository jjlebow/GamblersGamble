using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : Interactable
{
    public GameObject button;
    private MusicMenu musicPanel;

    // Start is called before the first frame update
    void Start()
    {
        musicPanel = Manager.instance.musicPanel.transform.GetComponent<MusicMenu>();

        player = Manager.instance.player;
        button = Manager.instance.musicPanel.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    protected override void Interact()
    {
        if(player.canInteract)
        {
            

            Manager.instance.NewState(Manager.GameState.MENU);
            musicPanel.OpenMenu(button);
        }
    }
}
