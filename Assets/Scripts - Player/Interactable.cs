using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public PlayerController player;
    protected bool canInteract = false;

    private Sprite oldSprite;
    [SerializeField] private Sprite newSprite = null;

    void Awake()
    {
        oldSprite = GetComponent<SpriteRenderer>().sprite;
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            canInteract = false;
            GetComponent<SpriteRenderer>().sprite = oldSprite;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            canInteract = true;
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canInteract)
        {
            Interact();
        }
    }

    protected virtual void Interact()
    {
        Debug.Log("This should not print");
    }

}
