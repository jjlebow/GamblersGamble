using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public static class CardFunctions 
{
    private static string jsonSavePath = "Assets/Resources/JSON/savedCards.json";

/// <summary>
/// Return unique ManagerCards added and saved from the ManagerWindow
/// </summary>
    public static ManagerCardList ReadSavedList(bool sorted = true)
    {
        string json = "";
        using (StreamReader r = new StreamReader(jsonSavePath))
        {
            json = r.ReadToEnd();
        }
        ManagerCardList managerCardList = JsonUtility.FromJson<ManagerCardList>(json);
        managerCardList.Load();

        if (sorted == true) {
            managerCardList.Sort();
        }

        return managerCardList;
    }

/// <summary>
/// Retrieves cards marked to be in shop from JSON file
/// </summary>
    public static List<Card> GetShopCards()
    {
        ManagerCardList managerCardList = ReadSavedList();
        List<Card> cards = managerCardList.list
                            .Where(managerCard => managerCard.inShop == true)
                            .Select(managerCard => managerCard.card)
                            .ToList();
        return cards;
    }

/// <summary>
/// Retrieves card deck whose count is specified in the JSON file
/// </summary>
    public static List<Card> GetDefaultCardDeck()
    {
        List<Card> defaultDeck = new List<Card>();
        foreach (ManagerCard managerCard in ReadSavedList().list) {
            // Debug.Log(String.Format("{0} Cards In Deck: {1}", managerCard.card.name, managerCard.cardsInDeck));
            for (int i = 0; i < managerCard.cardsInDeck; ++i) {
                defaultDeck.Add(new Card(managerCard.card));
            }
        }
        // Debug.Log("Default Deck Count: " + defaultDeck.Count);
        return defaultDeck;
    }
}