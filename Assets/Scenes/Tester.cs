using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tester : MonoBehaviour
{
	//public RawImage image;

	//private Texture2D tex; 


	void DestroyObj()
	{
		gameObject.SetActive(false);
	}
    // Start is called before the first frame update
    void Start()
    {
    	//tex = (Texture2D)image.texture;
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0.2f, 3f).setOnComplete(DestroyObj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*  also needs to change import settings of the sprite to be RGB 32 and allow read/write for this to "work"
    public IEnumerator Pixelate()
    {
    	int y = 0;
    	while(y < tex.height)
    	{
    		for(int x = 0; x < tex.width; x++)
    		{
    			Color color = tex.GetPixel(x, y);
    			if(color.a !=0f)
    			{
    				color.a = 0f;
    				tex.SetPixel(x, y, color);
    				tex.Apply();
    				yield return null;//new WaitForSeconds(0.001f);
    			}
    		}
    		y = y+ 1;
    	}
    }*/
}
