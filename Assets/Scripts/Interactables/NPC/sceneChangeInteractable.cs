using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChangeInteractable : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void Interact()
    {
    	if(canInteract && Input.GetKeyDown(KeyCode.E))
    	{
    		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    	}
    }}
