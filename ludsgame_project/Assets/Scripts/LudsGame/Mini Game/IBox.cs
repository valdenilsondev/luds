using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public interface IBox
{
    string name { get; set; }

    Sprite picture { get; set; }

    //BoxType type { get; set; }

    //CategoryType category { get; set; }

    void IncreaseBox();

    void DecreaseBox();
}
