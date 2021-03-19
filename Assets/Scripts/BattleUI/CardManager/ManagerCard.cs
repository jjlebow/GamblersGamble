using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ManagerCard
{
    public Card card;

    public string iconPath;
    public bool inShop;
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
        card.icon = Resources.Load<Sprite>(this.iconPath);
    }
}
