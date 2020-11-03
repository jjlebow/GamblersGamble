using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{   
	public GameObject startButton;
	public GameObject newGame;
	public GameObject options;
	public GameObject credits;
	public GameObject quit;
    //public Button playButton;


    public GameObject optionsPanel;
    public GameObject creditsPanel;

	private TMP_Text text;

	void Awake()
	{
		text = startButton.GetComponentInChildren<TMP_Text>();
	}


    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }

    public void StartGame(Button button)
    {
    	var color = text.color;
		var fadeoutColor = color;
		fadeoutColor.a = 0;
    	LeanTween.value(text.gameObject, UpdateValueCallback, color, fadeoutColor, 1f).setOnComplete(HideButton);
    	//LeanTween.alpha(startButton.GetComponentInChildren<TMP_Text>().gameObject, 0f, 3f).setOnComplete(HideButton);
    	LeanTween.moveX(newGame, -7f, 1.5f).setEase(LeanTweenType.easeOutBack).setDelay(1f);
    	LeanTween.moveX(options, -6.5f,1.5f).setEase(LeanTweenType.easeOutBack).setDelay(1.1f);
    	LeanTween.moveX(credits, -6f, 1.5f).setEase(LeanTweenType.easeOutBack).setDelay(1.2f);
    	LeanTween.moveX(quit, -5.5f, 1.5f).setEase(LeanTweenType.easeOutBack).setDelay(1.3f).setOnComplete(delegate(){SelectNewButton(button);});
    }


    public void OpenOptionsPanel(Button button)
    {
        //LeanTween.scale(menu, new Vector3(0.25f, 0.5f, 0.5f), 0.5f).setEase(LeanTweenType.easeInCubic);
        LeanTween.alphaCanvas(optionsPanel.GetComponent<CanvasGroup>(), 1f, 0.65f).setEase(LeanTweenType.easeInExpo);
        LeanTween.move(optionsPanel, new Vector3(0f, 0f, 0f), 0.65f).setEase(LeanTweenType.easeOutSine).setOnComplete(delegate(){SelectNewButton(button);});
    }

    public void HideOptionsPanel(Button button)
    {
        //LeanTween.scale(optionsPanel, new Vector3(0f,0f,0f), 1f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.alphaCanvas(optionsPanel.GetComponent<CanvasGroup>(), 0f, 0.65f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.move(optionsPanel, new Vector3(0f, 5f, 0), 0.65f).setEase(LeanTweenType.easeInSine).setOnComplete(delegate(){SelectNewButton(button);});
    }

    public void OpenCreditsPanel(Button button)
    {
        //LeanTween.scale(menu, new Vector3(0.25f, 0.5f, 0.5f), 0.5f).setEase(LeanTweenType.easeInCubic);
        LeanTween.alphaCanvas(creditsPanel.GetComponent<CanvasGroup>(), 1f, 0.65f).setEase(LeanTweenType.easeInExpo);
        LeanTween.move(creditsPanel, new Vector3(0f, 0f, 0f), 0.65f).setEase(LeanTweenType.easeOutSine).setOnComplete(delegate(){SelectNewButton(button);});
    }

    public void HideCreditsPanel(Button button)
    {
        //LeanTween.scale(optionsPanel, new Vector3(0f,0f,0f), 1f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.alphaCanvas(creditsPanel.GetComponent<CanvasGroup>(), 0f, 0.65f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.move(creditsPanel, new Vector3(0f, 5f, 0), 0.65f).setEase(LeanTweenType.easeInSine).setOnComplete(delegate(){SelectNewButton(button);});
    }


    void UpdateValueCallback(Color val)
    {
    	text.color = val;
    }

    void HideButton()
    {
    	startButton.SetActive(false);
    }



    public void SelectNewButton(Button button)
    {
        button.Select();
    }


    

}

