﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChangeInteractable : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        player = Manager.instance.player;
    }

    protected override void Interact()
    {
    	if(canInteract && Input.GetKeyDown(KeyCode.E))
    	{
    	   TransitionsManager.instance.FadeInScene(this.name);
    	}
    }}
