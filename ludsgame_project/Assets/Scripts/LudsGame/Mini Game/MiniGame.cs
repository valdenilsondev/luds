using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class MiniGame
{
    public string name;
    public List<CategoryType> categoryList;
    public Sprite picture;
    public BoxType type;
    public bool active;

    public MiniGame(string name, List<CategoryType> category)
    {
        this.name = name;
        this.categoryList = category;
    }

    public string GetMiniGameName() { return name; }

    public List<CategoryType> GetMiniGameCategories() { return categoryList; }

    public void IncreaseBox()
    {
        throw new NotImplementedException();
    }

    public void DecreaseBox()
    {
        throw new NotImplementedException();
    }


}
