using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionsManager : MonoBehaviour
{
	public static TransitionsManager instance;

	void Awake()
	{
		instance = this;
	}


	public void FadeInScene(string sceneName)
    {
    	this.transform.GetChild(0).gameObject.SetActive(true);
    	LeanTween.alpha(this.transform.GetChild(0).gameObject.GetComponent<RectTransform>(), 1f,2f).setOnComplete(() => LoadScene(sceneName));
    }
    public void FadeOut()
    {
    	LeanTween.alpha(this.transform.GetChild(0).gameObject.GetComponent<RectTransform>(), 0f,2f).setOnComplete(HideFadeScreen);
    }

    //private helper functions for the above functions
    private void LoadScene(string sceneName)
    {
    	SceneManager.LoadScene(sceneName);
    }

    private void HideFadeScreen()
    {
    	this.transform.GetChild(0).gameObject.SetActive(false);
    }
    
    
    

    // Start is called before the first frame update
}
