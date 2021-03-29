using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public void Sort()
    {
        this.list = this.list
                        .OrderBy(managerCard => managerCard.card.name)
                        .ToList();
    }
}

[Serializable]
public class ManagerCard
{
    public Card card;

    public string iconPath;
    public bool inShop;
    public int cardsInDeck = 0;
    //public string bindingIcon;

    public ManagerCard(Card card, string iconPath, bool inShop)
    {
        this.card = card;
        this.iconPath = iconPath;
        this.inShop = inShop;
        
        Load();
    }

    public void Load()
    {
        // loads icons in containing card
        Debug.Log("Loading Asset at path: " + this.iconPath);
        card.icon = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(this.iconPath, typeof(Sprite));
        Debug.Log("Card Icon: " + card.icon);
    }

    public void UpdateIconPath()
    {
        if (this.card.icon != null) {
            iconPath = UnityEditor.AssetDatabase.GetAssetPath(this.card.icon);
            Debug.Log("Asset Path: " + iconPath);
        }
    }
}
