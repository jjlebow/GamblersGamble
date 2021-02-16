﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    public PlayerController player;
    public GameObject bubble;
    public bool firstInteract = false;
    public string name;
    //protected EventSystem m_eventSystem;
    //public bool canInteract = false;

    //[SerializeField] private Sprite oldSprite;
    //[SerializeField] private Sprite newSprite = null;

    void Awake()
    {
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            bubble.SetActive(false);
            //canInteract = false;
            //GetComponent<SpriteRenderer>().sprite = oldSprite;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            bubble.SetActive(true);
            //canInteract = true;
            //GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }

    protected virtual void Interact()
    {
        Debug.Log("this line should not be printed");
    }

    private void Update()
    {


/*
        if(Input.GetKeyDown(KeyCode.E) && canInteract && Manager.instance.currentState == Manager.GameState.BATTLE)
        {
            Interact();
        }
*/
    }


}
