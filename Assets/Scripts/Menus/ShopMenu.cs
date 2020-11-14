using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopMenu : MonoBehaviour
{

    public GameObject descriptionBox;
    public GameObject shopImage;
    public GameObject deckGroup;
    public GameObject idleButton;
    private EventSystem es;
    // Start is called before the first frame update
    void Start()
    {
        es = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseMenu()
    {
        es.SetSelectedGameObject(idleButton);
        LeanTween.alphaCanvas(shopImage.GetComponent<CanvasGroup>(), 0f, 0.65f).setEase(LeanTweenType.easeOutQuint).setDelay(0.5f);
        LeanTween.moveLocal(shopImage, new Vector3(850f, 0f, 0f), 0.65f).setEase(LeanTweenType.easeInSine).setDelay(0.5f);
        LeanTween.moveLocalY(descriptionBox, -400, 0.65f).setEase(LeanTweenType.easeInSine);
        LeanTween.alphaCanvas(descriptionBox.GetComponent<CanvasGroup>(), 0f, 0.65f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.moveLocal(deckGroup, new Vector3(-450f, 0f, 0), 0.65f).setEase(LeanTweenType.easeInSine).setOnComplete(delegate(){MenuCleanup();});
        LeanTween.alphaCanvas(deckGroup.GetComponent<CanvasGroup>(), 0f, 0.65f).setEase(LeanTweenType.easeOutQuint);

    }

    public void OpenMenu(GameObject button)
    {
        Manager.instance.shopPanel.SetActive(true);
        LeanTween.alphaCanvas(shopImage.GetComponent<CanvasGroup>(), 1f, 0.65f).setEase(LeanTweenType.easeInExpo);
        LeanTween.moveLocal(shopImage, new Vector3(0f, 0f, 0f), 0.65f).setEase(LeanTweenType.easeOutSine);
        LeanTween.alphaCanvas(deckGroup.GetComponent<CanvasGroup>(), 1f, 0.65f).setEase(LeanTweenType.easeInExpo).setDelay(0.5f);
        LeanTween.moveLocal(deckGroup, new Vector3(0,0,0), 0.65f).setEase(LeanTweenType.easeOutSine).setDelay(0.5f);
        LeanTween.alphaCanvas(descriptionBox.GetComponent<CanvasGroup>(), 1f, 0.65f).setEase(LeanTweenType.easeInExpo).setDelay(0.5f);
        LeanTween.moveLocalY(descriptionBox, -140f, 0.65f).setEase(LeanTweenType.easeOutSine).setDelay(0.5f).setOnComplete(delegate(){SelectNewButton(button);});
    }


    private void SelectNewButton(GameObject button)
    {
        es.SetSelectedGameObject(button);
    }

    private void MenuCleanup()
    {
        Manager.instance.RevertState();
        Manager.instance.shopPanel.SetActive(false);
        //es.SetSelectedGameObject(idleButton);
    }

}
