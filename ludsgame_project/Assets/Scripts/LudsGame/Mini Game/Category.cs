using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Category
{
    public string name;

    public Sprite picture;

    public BoxType type;

    public CategoryType category;

    public Category(string name, Sprite picture, BoxType type, CategoryType category)
    {
        this.name = name;
        this.picture = picture;
        this.type = type;
        this.category = category;

    }    
}