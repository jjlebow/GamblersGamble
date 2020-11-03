using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{   
	public GameObject startButton;
	public GameObject newGame;
	public GameObject options;
	public GameObject credits;
	public GameObject quit;

    public GameObject optionsPanel;

	private TMP_Text text;

	void Awake()
	{
		text = startButton.GetComponentInChildren<TMP_Text>();
	}

    public void Playgame()
    {
    	TransitionsManager.instance.FadeInScene("Hub");
        
    }


    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }

    public void StartGame()
    {
    	var color = text.color;
		var fadeoutColor = color;
		fadeoutColor.a = 0;
    	LeanTween.value(text.gameObject, UpdateValueCallback, color, fadeoutColor, 1f).setOnComplete(HideButton);
    	//LeanTween.alpha(startButton.GetComponentInChildren<TMP_Text>().gameObject, 0f, 3f).setOnComplete(HideButton);
    	LeanTween.moveX(newGame, -7f, 1.5f).setEase(LeanTweenType.easeOutBack).setDelay(1f);
    	LeanTween.moveX(options, -6.5f,1.5f).setEase(LeanTweenType.easeOutBack).setDelay(1.1f);
    	LeanTween.moveX(credits, -6f, 1.5f).setEase(LeanTweenType.easeOutBack).setDelay(1.2f);
    	LeanTween.moveX(quit, -5.5f, 1.5f).setEase(LeanTweenType.easeOutBack).setDelay(1.3f);
    }

    public void OpenSubMenu(GameObject menu)
    {
        LeanTween.scale(menu, new Vector3(0.25f, 0.5f, 0.5f), 0.5f).setEase(LeanTweenType.easeInCubic);
        LeanTween.move(menu, new Vector3(0f, 0f, 0f), 0.5f);
    }

    public void HideOptionsMenu()
    {
        LeanTween.scale(optionsPanel, new Vector3(0f,0f,0f), 1f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.move(optionsPanel, new Vector3(-6.5f, -2.5f, 0), 1f);
    }

    void UpdateValueCallback(Color val)
    {
    	text.color = val;
    }

    void HideButton()
    {
    	startButton.SetActive(false);
    }

}

