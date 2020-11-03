using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAudio : MonoBehaviour
{
	public AudioClip sound;
    public AudioClip selectSound;
	private Button button {get{return GetComponent<Button>();}}
	private AudioSource source {get{return GetComponent<AudioSource>();}}


    // Start is called before the first frame update
    void Start()
    {
    	gameObject.AddComponent<AudioSource>();
        //button.onClick.AddListener(() => PlaySound());
    }

    // Update is called once per frame
    public void PlaySound()
    {
        source.clip = sound;
        source.volume = 0.2f;
        source.playOnAwake = false;
    	source.PlayOneShot(sound);
    }

    public void PlaySelectSound()
    {
        source.clip = selectSound;
        source.volume = 1;
        source.playOnAwake = false;
        source.PlayOneShot(selectSound);
    }

}
