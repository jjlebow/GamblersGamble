﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



/*
In order to use this class properly, create a Resources folder in your assets, and make and Art folder. Within that folder make a folder titled BG and a 
folder titled Portraits. Make all dialogue into CSV files that follow the appropriate format and leave these outside of your art folder, but not inside of 
another folder. 
*/

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GameObject dialoguePanel;
    //public GameObject stagePanel;  replace this with some kind of menu that you interact with. 
    public GameObject namePanel;
    public GameObject spritePanel;
    public GameObject bgPanel;
    public GameObject bgPanel2;

    private Queue<Dialogue> textOutput;    //this is a queue of all text that is going to be output
    private int initialText = 0;    //if initialText is -1, then a sentence can start as the dialoguePanel is set to true
    private bool spaceDelay = false;    //Pressing spacebar while this is false will progress the dialogue to the next line. Pressing it while this is true will display the full sentence immediately 
    private Dialogue nextLine;  //holds the next line on the queue, used to print letter by letter or replace the current text
    private Coroutine lastRoutine = null;   //used to hold pointer to coroutine call responsible for printing letter by letter
    //private bool introTransition = false;
    public bool dialogueAvailable = false;

    public UnityEvent DoneWithDialogue;

    public bool hasDoneIntro = false;
    public bool doneWithGame = false;
    private Sprite currentSprite = null;
    [HideInInspector] public Sprite currentBG = null;
    private string currentSpeaker = "";

    int maxLines = 0;
    int count = 0;
    bool ShouldBePlayingDialogue = false;
    [HideInInspector] public bool isIntro = true;    //allows us to do things that are specific to post introduction in stagehub script

    private void Awake()
    {
        DoneWithDialogue = new UnityEvent();
        //EndOfWeek = new UnityEvent();
        //AllWeeksFinished = new UnityEvent();
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    //Immediately starts the game off by playing the introduction 
    void Start()
    {
        textOutput = new Queue<Dialogue>();
        if (!hasDoneIntro)
        {
            LoadDialogue("Dialogue", "Introduction", "INTRODUCTION");
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(PauseOnMap.mapPaused == false && ShouldBePlayingDialogue)
        {
            if (textOutput.Count >= 1)
            {
                dialogueAvailable = true;
                dialoguePanel.SetActive(true);            
                if (initialText == 0)
                {
                    initialText++;
                    DisplayNextSentence();
                }
                //Debug.Log(maxLines - count);
                if (Input.GetKeyDown(KeyCode.Space) && !spaceDelay)
                {
                    DisplayNextSentence();
                    count -= 1;
                }
                else if (Input.GetKeyDown(KeyCode.Space) && spaceDelay)
                    DisplayFullSentence();
                else if (Input.GetKeyDown(KeyCode.P) && (maxLines - count > 0)) //press P to skip dialogue, FOR T E S T I N G P U R P O S E S
                {
                    ShouldBePlayingDialogue = false;
                    spaceDelay = false;
                    dialogueAvailable = false;
                    if (!introTransition)
                    {
                        StartCoroutine(TransitionManager.instance.screenFadeIn);
                        introTransition = true;
                    }
                    if (!hasDoneIntro)
                    {
                        HideBackground();
                        hasDoneIntro = true;
                    }
                    initialText = 0;
                    HidePanels();
                    textOutput.Clear();
                    currentSprite = null;
                    bgPanel2.GetComponent<Image>().sprite = null;
                    if (!doneWithGame)
                    {
                        NextWeek();
                        DoneWithDialogue.Invoke();
                    }
                    else
                    {
                        HideBackground();
                        AllWeeksFinished.Invoke();
                    }
                }

            }
            else if (textOutput.Count == 0)     //if we're on the last sentence, we want to wait for the player to close the dialogue box
            {
                if(Input.GetKeyDown(KeyCode.Space) && !spaceDelay)
                {
                    ShouldBePlayingDialogue = false;
                    HidePanels();
                    if(!hasDoneIntro)
                    {
                        HideBackground();
                        hasDoneIntro = true;
                    }
                    initialText = 0;
                    dialogueAvailable = false;
                    currentSprite = null;
                    bgPanel2.GetComponent<Image>().sprite = null;
                    if (!doneWithGame)
                    {
                        NextWeek();
                        DoneWithDialogue.Invoke();
                    }
                    else
                    {
                        HideBackground();
                        AllWeeksFinished.Invoke();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Space) && spaceDelay)
                {
                    ShouldBePlayingDialogue = false;
                    if (!hasDoneIntro)
                    {
                        HideBackground();
                        hasDoneIntro = true;
                    }
                    DisplayFullSentence();
                    HidePanels();
                    initialText = 0;
                    dialogueAvailable = false;
                    currentSprite = null;
                    bgPanel2.GetComponent<Image>().sprite = null;
                    if (!doneWithGame)
                    {
                        NextWeek();
                        DoneWithDialogue.Invoke();
                    }
                    else
                    {
                        HideBackground();
                        AllWeeksFinished.Invoke();
                    }
                }
            }
        }*/

        if(ShouldBePlayingDialogue)
        {
            if (textOutput.Count >= 1)
            {
                dialogueAvailable = true;
                dialoguePanel.SetActive(true);            
                if (initialText == 0)
                {
                    initialText++;
                    DisplayNextSentence();
                }
                //Debug.Log(maxLines - count);
                //pressing spacebar while spaceDelay is false means the sentence is done printing and will advance to the next line. 
                //also reduces the count of the queue which signifies moving on to the next line. 
                if (Input.GetKeyDown(KeyCode.Space) && !spaceDelay)
                {
                    DisplayNextSentence();
                    count -= 1;
                }
                //pressing spacebar while the spaceDelay is true means the sentence is not done printing yet and will just immediately print the rest of the current line. 
                else if (Input.GetKeyDown(KeyCode.Space) && spaceDelay)
                    DisplayFullSentence();

            }
            //this is the last line of the queue in the current dialogue so after this all panels must close
            else if (textOutput.Count == 0) 
            {
                //also the two cases for whether spaceDelay is true or false
                if(Input.GetKeyDown(KeyCode.Space) && !spaceDelay)
                {
                    ShouldBePlayingDialogue = false;
                    HidePanels();
                    if(!hasDoneIntro)
                    {
                        HideBackground();
                        hasDoneIntro = true;
                    }
                    initialText = 0;
                    dialogueAvailable = false;
                    currentSprite = null;
                    bgPanel2.GetComponent<Image>().sprite = null;
                    Manager.instance.RevertState();
                }
                //dont know if this is accurate. i dont think this should necessarily hidePanels yet, we are just skipping the last dialgoue i think. 
                else if (Input.GetKeyDown(KeyCode.Space) && spaceDelay)
                {
                    ShouldBePlayingDialogue = false;
                    if (!hasDoneIntro)
                    {
                        HideBackground();
                        hasDoneIntro = true;
                    }
                    DisplayFullSentence();
                    HidePanels();
                    initialText = 0;
                    dialogueAvailable = false;
                    currentSprite = null;
                    bgPanel2.GetComponent<Image>().sprite = null;
                    Manager.instance.RevertState();
                }
            }
        }
    }
/*
    public void NextWeek()
    {
        if (SaveFileManager.instance.finishedAStage)
        {
            EndOfWeek.Invoke();
        }
    }
*/

    
    /*Funciton is called to hide all UI dialogue panels*/
    public void HidePanels()
    {
        dialoguePanel.SetActive(false);
        namePanel.SetActive(false);
        spritePanel.SetActive(false);
    }

    //function is called to hide the background UI
    public void HideBackground()
    {
        bgPanel.SetActive(false);
        bgPanel2.SetActive(false);
    }

    //function is called to cut off all dialogue immediately. this is currently never called. might be for testing? delete if unused. 
    public void ClearDialogue()
    {
        dialogueAvailable = false;
        currentSprite = null;
        HidePanels();
        HideBackground();
        textOutput.Clear();
        dialogueText.text = "";
        StopAllCoroutines();
    }

    //takes in the string name of a CSV file (from the resources folder), and converts it into a dictionary that is readable and stored in the queue line by line. 
    private Dictionary<int, Dialogue> BuildDialogueDictionary(TextAsset textFile) //converts CSV file to dictionary
    {
        Dictionary <int, Dialogue> GameDialogue = new Dictionary<int, Dialogue>();

        string[] data = textFile.text.Split(new char[] { '\n' });

        for (int i = 2; i <= data.Length - 1; i += 2) //even lines due to CSV sheet issues (prime lines are ,,)
        {
            string[] parsedData = data[i].Split(new char[] { ',' });
            //Debug.Log(i);
            Dialogue dialogueObj = new Dialogue(parsedData[0], parsedData[1].Replace("XYZ", ","), parsedData[2], parsedData[3], parsedData[4]); 

            GameDialogue.Add(i / 2, dialogueObj);
        }

        return GameDialogue;
    }

    //this loads the dictionary into the queue. Filename should be Dialogue, speakername is the one speaking
    //and speakerDialogue is the specific script that the character is saying. 
    public void LoadDialogue(string fileName, string speakerName, string speakerDialogue)
    {
        /*
        if (stagePanel != null)
            stagePanel.SetActive(false);
        */
        int currentLine = 1;
        fileName = fileName + "/" + speakerName + "/" + speakerDialogue;

        Dictionary<int, Dialogue> dialogueDict = BuildDialogueDictionary(Resources.Load<TextAsset>(fileName));

        while (currentLine <= dialogueDict.Count)
            textOutput.Enqueue(dialogueDict[currentLine++]);
        maxLines = textOutput.Count;
        count = maxLines;
        ShouldBePlayingDialogue = true;
        Manager.instance.NewState(Manager.GameState.DIALOGUE);
    }

    private void DisplayNextSentence()
    {
        nextLine = textOutput.Dequeue();
        spaceDelay = true;
        UpdateBackground(nextLine);
        lastRoutine = StartCoroutine(UpdateText(nextLine));
    }

    private void DisplayFullSentence()
    {
        StopCoroutine(lastRoutine);
        dialogueText.text = nextLine.text;
        spaceDelay = false;
    }

    private IEnumerator UpdateText(Dialogue DialogueObj)
    {
        dialogueText.text = "";
        dialogueText.fontStyle = FontStyles.Normal;
        string filePath = "Art/Portraits/";
        string spriteName = DialogueObj.sprite.ToString().Trim();

        //if(AudioManager.instance.currentSong.name != DialogueObj.audio && DialogueObj.audio != null)
            //FindObjectOfType<AudioManager>().DialogueTransitionSong(DialogueObj.audio);    //music changes during dialogue
        
        if (DialogueObj.speaker.Length == 1) //monologue: set namePanel to invisible and text to italic. Monologue denoted by speaker: M
        {
            namePanel.SetActive(false);
            if(currentSprite == null)
                spritePanel.SetActive(false);
            dialogueText.fontStyle = FontStyles.Italic;
        }
        else if (DialogueObj.speaker.Length == 2) //for MC talking: name and sprite panel are both visible, showing MC name and girl he talks to
        {
            namePanel.SetActive(true);
            nameText.text = DialogueObj.speaker;
            if(currentSprite == null)
                spritePanel.SetActive(false);


        }
        else if (DialogueObj.speaker == "Haruka" || DialogueObj.speaker == "Touka" || DialogueObj.speaker == "Akiko" || DialogueObj.speaker == "Natsuki") //aka only loads avail sprite 
        {
            namePanel.SetActive(true);
            nameText.text = DialogueObj.speaker;
            //Debug.Log("here");
            filePath = string.Format("{0}{1}/{2}", filePath, DialogueObj.speaker.Trim(), spriteName);
            /*
            if (DialogueObj.speaker == "Haruka" && !introTransition)
            {
                StartCoroutine(TransitionManager.instance.screenFadeIn);
                introTransition = true;
            }*/
            //Debug.Log(filePath);
            currentSprite = Resources.Load<Sprite>(filePath) as Sprite;
            spritePanel.GetComponent<Image>().sprite = currentSprite;
            if(currentSprite != null) 
                spritePanel.SetActive(true);
        }
        else //any other character: set namePanel to visible and text to normal
        {
            namePanel.SetActive(true);
            nameText.text = DialogueObj.speaker;
            filePath = string.Format("{0}{1}/{2}", filePath, DialogueObj.speaker.Trim(), spriteName);
            if (currentSprite == null)
                spritePanel.SetActive(false);
        }

        foreach (char letter in DialogueObj.text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;  
        }
        spaceDelay = false;
    }

    public string BGStringBuilder(string filePath, string backgroundName, string speaker, Dialogue DO)
    {
        return string.Format("{0}/{1}/{2}", filePath, "BG", backgroundName);

        /*
        string[] splitString = backgroundName.Split(' ');
        if(splitString[0] == "BG")
        {
            if(DO.sprite != "NONE")
                spritePanel.SetActive(true);    //shows the sprite only if the background is BG and if the sprite name is not equal to NONE
            else
                spritePanel.SetActive(false);    //otherwise it hides the spritePanel
            return string.Format("{0}{1}/{2}", filePath, "BG", splitString[1]);
        }
        else{
            spritePanel.SetActive(false);
            if(speaker == "")
                speaker = DO.speaker;
            return string.Format("{0}{1}/{2}/{3}", filePath, "CG", speaker, splitString[1]);
        }
        */
    }
    private void UpdateBackground(Dialogue DialogueObj)
    {
        string backgroundName = DialogueObj.background.ToString().Trim();
        string bgFilepath = "Art";
        string testSpeaker = DialogueObj.speaker.ToString().Trim();
        Sprite tempBG;
        if(testSpeaker != "MC" && testSpeaker != "M")
        {
            currentSpeaker = testSpeaker;
        }
        bgFilepath = BGStringBuilder(bgFilepath, backgroundName, currentSpeaker, DialogueObj);
        tempBG = Resources.Load<Sprite>(bgFilepath) as Sprite;
        /*if(tempBG != null)    //if it is null then that means that the MC or someone else is talking and we cant access a new background
        {
            currentBG = tempBG;
            if(sceneManagement.instance.sceneFlag == true)
            {
                sceneManagement.instance.sceneFlag = false;
                TransitionManager.instance.BGFadeFirst();
            }
            else
            {
                if(bgPanel2.GetComponent<Image>().sprite == null)
                {
                    TransitionManager.instance.BGFadeZero();
                }
                else
                {   
                    TransitionManager.instance.BGFadeSecond();
                }
            }
        }*/
    }
}