using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MusicMenu : MonoBehaviour
{

    public GameObject idleButton;
    public EventSystem es;
    public GameObject button1;

    // Start is called before the first frame update
    void Start()
    {
        es = EventSystem.current;
    }

    public void OpenMenu(GameObject button)
    {
        Manager.instance.musicPanel.SetActive(true);
        LeanTween.moveLocalX(button1, 0, 0.5f).setOnComplete(delegate(){SelectNewButton(button);});
        
    }

    public void CloseMenu()
    {
        es.SetSelectedGameObject(idleButton);
        Manager.instance.RevertState();
        Manager.instance.musicPanel.SetActive(false);
    }
    
    private void SelectNewButton(GameObject button)
    {
        es.SetSelectedGameObject(button);
    }

    public void PlayButtonSong(string name)
    {
        AudioManager.instance.Stop(AudioManager.instance.currentSongs[0]);
        AudioManager.instance.Play(name);
    }
}
