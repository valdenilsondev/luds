using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameOverScreen
{
    public class Button : MonoBehaviour
    {
        public string name;
        private Dictionary<String, Sprite> dict;
        Ray ray;
        RaycastHit hit;

        void Awake()
        {
            var buttons = Resources.LoadAll<Sprite>("Share/GameOver");
            dict = new Dictionary<String, Sprite>();

            foreach (var btn in buttons)
            {
                if(!btn.name.Contains("active"))
                    dict.Add("Normal", btn);
                else
                    dict.Add("Active", btn);    
            }
        }

        void Update()
        {
            /*ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                print(hit.collider.name);
            }*/
        }

        void OnTriggerEnter2D(Collider2D col)
        {
        }

		private int fontSizeNormal;

        public void Normal()
        {
			if(this.GetComponentInChildren<Text>() != null){
            	this.GetComponentInChildren<Text>().fontSize = fontSizeNormal;
            	this.GetComponent<Image>().rectTransform.localScale = new Vector3(1, 1, 0);
			}
        }

        public void Active()
        {
			if(this.GetComponentInChildren<Text>() != null){
				fontSizeNormal = this.GetComponentInChildren<Text>().fontSize;
				this.GetComponentInChildren<Text>().fontSize = (int) ( fontSizeNormal*1.3f );
         	   this.GetComponent<Image>().rectTransform.localScale = new Vector3(1.2f, 1.2f, 0);
			}
        }
    }
}