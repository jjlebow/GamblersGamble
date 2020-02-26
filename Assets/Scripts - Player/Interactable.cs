using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public PlayerController player;
    private bool canInteract = false;


    void OnTriggerEnter2D(Collider2D col)
    {

        if(col.gameObject.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && canInteract)
        {
            Interact();
        }
    }

    public virtual void Interact()
    {
        Debug.Log("This should not print");
    }

}
