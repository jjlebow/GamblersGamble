
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ManagerCardList
{
    public List<ManagerCard> list;

    public ManagerCardList()
    {
        this.list = new List<ManagerCard>();
    }

    public void Load()
    {
        foreach (var card in list) 
        {
            card.Load();
        }
    }
}


public class ManagerWindow : EditorWindow
{
    // public string name;
    // public Object icon;
    // public int cost;
    // public int damage;
    // public int suit;

    public ManagerCardList cards = new ManagerCardList();

    private static string jsonSavePath = "Assets/Resources/JSON/savedCards.json";
    private Vector2 scrollPosition;
    private float cardWidth = 250;
    private float cardHeight = 160;
    private ManagerCard cardToDelete = null;

    [MenuItem("Window/ManagerWindow")]
    public static void Init()
    {
        EditorWindow window = GetWindow<ManagerWindow>("ManagerWindow");
        window.Show();
    }

    public void Awake()
    {
        cards = ReadSavedList();
    }

    public static ManagerCardList ReadSavedList()
    {
        string json = "";
        using (StreamReader r = new StreamReader(jsonSavePath))
        {
            json = r.ReadToEnd();
        }
        ManagerCardList list = JsonUtility.FromJson<ManagerCardList>(json);
        list.Load();
        return list;
    }

    public void SaveCard()
    {
        Debug.Log("Saving Card JSON");

        string cardJson = JsonUtility.ToJson(cards, true);
        File.WriteAllText(jsonSavePath, cardJson);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void AddCard()
    {
        cards.list.Add(new ManagerCard(new Card(), "", false));
    }

    public void GenerateManagerCardList()
    {
        cards.list.Add(GenerateManagerCard("Attack", "Art/sword_icon", 5, 7,1));
        cards.list.Add(GenerateManagerCard("HeavyAttack", "Art/heavyAttack", 5, 10,1));
        cards.list.Add(GenerateManagerCard("Dash", "Art/Dash", 5, 0,0));
        cards.list.Add(GenerateManagerCard("PrecisionShot", "Art/precision_shot", 5, 8,2));
        cards.list.Add(GenerateManagerCard("Shoot", "Art/Dart", 5, 5,2));
        cards.list.Add(GenerateManagerCard("HeavyShot", "Art/heavy_shot", 5, 7,2));
        cards.list.Add(GenerateManagerCard("PrecisionAttack", "Art/precision_attack", 5, 5,1));
    }

    public static ManagerCard GenerateManagerCard(string newName, string newIconPath, int newCost, int newDamage, int newSuit)
    {
    	Card newCard = new Card(newName, newIconPath, newCost, newDamage, newSuit);
        ManagerCard newManagerCard = new ManagerCard(newCard, newIconPath, false);
        Debug.Log("Manager Card: " + newManagerCard.card.name);
    	return newManagerCard;
    }

    void DisplayCard(ManagerCard managerCard, float width, float height)
    {
        if (managerCard != null)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(width), GUILayout.Height(height));
            managerCard.card.name = EditorGUILayout.TextField("Card Name: ", managerCard.card.name);
            managerCard.card.icon = EditorGUILayout.ObjectField("Icon Prefab: ", managerCard.card.icon, typeof(Sprite), false) as Sprite;
            managerCard.card.cost = EditorGUILayout.IntField("Cost: ", managerCard.card.cost);
            managerCard.card.damage = EditorGUILayout.IntField("Damage: ", managerCard.card.damage);
            managerCard.card.suit = EditorGUILayout.IntField("Suit: ", managerCard.card.suit); 
            if (GUILayout.Button("Delete", GUILayout.Height(20), GUILayout.Width(100))) {
                cardToDelete = managerCard;
            }
            GUILayout.EndVertical();
        }
       
    }

    void HandleDelayedEvents()
    {
        // deleting cards
        if (cardToDelete != null) {
            var itemToRemove = cards.list.SingleOrDefault(c => System.Object.ReferenceEquals(c, cardToDelete));

            if (itemToRemove != null) {
                Debug.Log(String.Format("Deleting {0}", cardToDelete.card.name));
                cards.list.Remove(itemToRemove);
            }

            cardToDelete = null;
        }
    }

    void OnGUI ()
    {
        GUILayout.Label("This is a label.", EditorStyles.boldLabel);

        //string newName, string newIconPath, int newCost, int newDamage, int newSuit
        //"Attack", "Art/sword_icon", 5, 7, 1

        if (GUILayout.Button("Load", GUILayout.Height(20), GUILayout.Width(100))) {
            Debug.Log("Loading Card JSON");
            cards = ReadSavedList();
            Repaint();
        }

        if (GUILayout.Button("New Card", GUILayout.Height(20), GUILayout.Width(100))) {
            AddCard();   
        }

        int boxHeight = 500;
        int cardsPerRow = 4;
        int rows = (int)Math.Ceiling(cards.list.Count / (float)cardsPerRow);

        scrollPosition = GUILayout.BeginScrollView(
            scrollPosition, GUILayout.Width(cardWidth * cardsPerRow * 1.10f), GUILayout.Height(cardHeight * rows * 1.20f));

        for (int i = 0; i < rows; ++i) {
            GUILayout.BeginHorizontal("Card Row " + i, GUILayout.Height(cardHeight));

            for (int j = 0; j < cardsPerRow; ++j) {         
                int cardIndex = (i * cardsPerRow) + j;
                if (cardIndex >= cards.list.Count) { break; }
                DisplayCard(cards.list[cardIndex], cardWidth, cardHeight);
            }

            GUILayout.EndHorizontal();
        }

        // GUILayout.BeginHorizontal("Group Outside");

        // foreach (ManagerCard card in cards.list) {
        //     DisplayCard(card);
        // }
        
        // GUILayout.EndHorizontal();

        GUILayout.EndScrollView();

        // EditorGUILayout.ObjectField()

        if (GUILayout.Button("Save")) {
            SaveCard();
        }

        HandleDelayedEvents();
    }
}
