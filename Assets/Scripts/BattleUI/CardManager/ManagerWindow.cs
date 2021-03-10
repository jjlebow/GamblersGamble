
using UnityEngine;
using UnityEditor;
using System.IO;

public class ManagerWindow : EditorWindow
{
    // public string name;
    // public Object icon;
    // public int cost;
    // public int damage;
    // public int suit;

    public Card testCard = null;

    private static string jsonSavePath = "Assets/Resources/JSON/testCard.json";



    [MenuItem("Window/ManagerWindow")]
    public static void Init()
    {
        EditorWindow window = GetWindow<ManagerWindow>("ManagerWindow");
        window.Show();
    }

    public void Awake()
    {
        Debug.Log("Awake");
        // testCard = GenerateCard("Attack", "Art/sword_icon", 5, 7, 1);
        testCard = ReadCard();
    }

    public static Card ReadCard()
    {
        string json = "";
        using (StreamReader r = new StreamReader(jsonSavePath))
        {
            json = r.ReadToEnd();
        }
        Card newCard = JsonUtility.FromJson<Card>(json);
        Debug.Log("New Card Name: " + newCard.name);
        return newCard;
    }

    public static Card SaveCard()
    {
        Debug.Log("Saving Card JSON");
        string testCardJson = JsonUtility.ToJson(testCard);
        File.WriteAllText(jsonSavePath, testCardJson);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static Card GenerateCard(string newName, string newIconPath, int newCost, int newDamage, int newSuit)
    {
    	Card newCard = new Card();
    	newCard.cost = newCost;
    	newCard.name = newName;
    	newCard.icon = null;
        newCard.damage = newDamage;
        newCard.suit = newSuit;
        Debug.Log("Generating Card");
    	return newCard;
    }

    void OnGUI ()
    {
        GUILayout.Label("This is a label.", EditorStyles.boldLabel);

        //string newName, string newIconPath, int newCost, int newDamage, int newSuit
        //"Attack", "Art/sword_icon", 5, 7, 1

        if (GUILayout.Button("Load")) {
            Debug.Log("Loading Card JSON");
            testCard = ReadCard();
            Repaint();
        }
        
        if (testCard != null) {

            testCard.name = EditorGUILayout.TextField("Card Name: ", testCard.name);
            // testCard.icon = EditorGUILayout.ObjectField("Icon Prefab: ", testCard.icon, typeof(Object), false);
            testCard.cost = EditorGUILayout.IntField("Cost: ", testCard.cost);
            testCard.damage = EditorGUILayout.IntField("Damage: ", testCard.damage);
            testCard.suit = EditorGUILayout.IntField("Suit: ", testCard.suit);
        }

        // EditorGUILayout.ObjectField()

        if (GUILayout.Button("Save")) {
            SaveCard();
        }
    }
}
