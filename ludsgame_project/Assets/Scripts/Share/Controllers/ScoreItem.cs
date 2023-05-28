using Assets.Scripts.Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Share.Controllers
{
    public class ScoreItem : MonoBehaviour
    {
        public ScoreItemsType type;
        public int multiplyingFactor;
        private float value;

        public float GetValue()
        {
            return value;
        }

        public void SetValue(float value)
        {
            this.value = value;
        }

        public void SetValueText(float value)
        {
			print(this.name);
            //this.GetComponentInChildren<Text>().text = value.ToString();
            this.transform.GetChild(0).GetComponentInChildren<Text>().text = value.ToString();
        }

        public void SetImage(Sprite image)
        {
            this.transform.GetChild(1).GetComponentInChildren<Image>().sprite = image;
        }
    }
}
