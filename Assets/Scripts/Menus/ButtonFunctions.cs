using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {

    	TransitionsManager.instance.FadeInScene(sceneName);
        gameObject.GetComponent<Button>().interactable = false;
    }

    public void ButtonAnimation(Image image)
    {
        LeanTween.scaleY(image.gameObject, 0.25f, 0.25f);
    }

    public void ReverseButtonAnimation(Image image)
    {
    	LeanTween.scaleY(image.gameObject, 0f, 0.25f);
    }
}
