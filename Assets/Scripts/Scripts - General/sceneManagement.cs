using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class sceneManagement : MonoBehaviour
{
    private string scName;
    public static sceneManagement instance;
    //public bool sceneFlag = false;
    public sceneMusic[] relations;
    sceneMusic s = null;
    private List<sceneMusic> soundList = new List<sceneMusic>();
    private List<string> removeSongs = new List<string>();
    private List<string> playSongs = new List<string>();
    private List<string> songs = new List<string>();
    private IEnumerator playRandom;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        playRandom = FindObjectOfType<AudioManager>().PlayRandom();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)   //ISSUE: when playing two songs before a stop is called, the first fade in coroutine
    {                                                       //is hidden behind the second and cannot be stopped when a stop corotuine is called
        soundList.Clear();                  //sets the list back to empty every time a scene loads
        removeSongs.Clear();
        playSongs.Clear();
        songs.Clear();
        s = Array.Find(relations, x => x.sceneName == scene.name && !soundList.Contains(x));
        while(s != null)  
        {
            s = Array.Find(relations, x => x.sceneName == scene.name && !soundList.Contains(x));
            if(s != null)    
                soundList.Add(s);
        }
        foreach(sceneMusic x in soundList)
        {
            if(AudioManager.instance.currentSongs.Contains(x.songName) == false)
                playSongs.Add(x.songName);
            songs.Add(x.songName);
        }
        foreach(string y in AudioManager.instance.currentSongs)
        {
            if(songs.Contains(y) == false)
                removeSongs.Add(y);
        }
        FindObjectOfType<AudioManager>().DialogueTransitionSong(removeSongs, playSongs);


        TransitionsManager.instance.FadeOut();
        
        if(scene.name.Split(' ')[0] == "Battle")
        {
            Manager.instance.bossDamageable = GameObject.FindGameObjectWithTag("Boss").GetComponent<Damageable>();
            Manager.instance.battleMenuUI.SetActive(true);
            //Manager.instance.battleMenu.SetActive(false);
            //Manager.instance.deckPanel.SetActive(true);
            //Manager.instance.discardPanel.SetActive(true);
            Manager.instance.goodHealthPanel.SetActive(true);
            Manager.instance.badHealthPanel.SetActive(true);
            Manager.instance.player.gameObject.SetActive(true);
            Manager.instance.NewState(Manager.GameState.BATTLE);
            //TransitionsManager.instance.FadeOut();
        }
        else if(scene.name.Split(' ')[0] == "Hub")
        {
            
            Manager.instance.battleMenuUI.SetActive(false);
            
            Manager.instance.goodHealthPanel.SetActive(false);
            Manager.instance.badHealthPanel.SetActive(false);
            Manager.instance.player.gameObject.SetActive(true);
            Manager.instance.NewState(Manager.GameState.BATTLE);
            //TransitionsManager.instance.FadeOut();


        }
        else if(scene.name == "LoadScene")
        {
            Manager.instance.player.gameObject.SetActive(false);
            Manager.instance.deckPanel.SetActive(false);
            Manager.instance.discardPanel.SetActive(false);
            DialogueManager.instance.gameObject.SetActive(false);
            Manager.instance.goodHealthPanel.SetActive(false);
            Manager.instance.badHealthPanel.SetActive(false);
            Manager.instance.battleMenuUI.SetActive(false);
            Manager.instance.battleMenu.SetActive(false);
            Manager.instance.shopPanel.SetActive(false);

        }
        


        Manager.instance.startingPoint = GameObject.FindGameObjectWithTag("StartingPosition").transform.position;

        Manager.instance.player.transform.position = Manager.instance.startingPoint;
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OpenScene(string sceneName)
    {
        TransitionsManager.instance.FadeInScene(sceneName);
    }


}
