using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Box : MonoBehaviour, IBox
{
    public string name { get; set; }

    public Sprite picture { get; set; }

    public BoxType type { get; set; }

    public CategoryType category { get; set; }

    public List<CategoryType> mg_category { get; set; }

    public void IncreaseBox()
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("scale", new Vector3(1f, 1f, 1f), "time", 1f, "easytype", iTween.EaseType.easeOutElastic));
    }

    public void DecreaseBox()
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("scale", new Vector3(0.8f,0.8f,0.8f), "time", 1f, "easytype", iTween.EaseType.easeOutElastic));
    }



}
