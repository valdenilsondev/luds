using Assets.Scripts.Share.Enums;
using Assets.Scripts.Share.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Share.Managers;

namespace Assets.Scripts.Share.Controllers
{
    public class GUIController : MonoBehaviour
    {
        public static GUIController instance;
        //hud to left
		public List<GameObject> hudToMoveLeft = new List<GameObject>();
		public List<float> hudToMoveLeftPos = new List<float>();
		private List<float> hudToMoveLeftOriginalPos = new List<float>();

        //hud to right
		public List<GameObject> hudToMoveRight = new List<GameObject>();
		public List<float> hudToMoveRightPos = new List<float>();
		private List<float> hudToMoveRightOriginalPos = new List<float>();

        //hud dist
        private float hudToLeftDist;
        private float hudToRightDist;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
			if (GameManagerShare.instance.game == Game.Pig) {
				var numberOfLifes = PlayerPrefsManager.GetNumberOfLifes ();
				InitializeLifesManager (numberOfLifes);
			}

			foreach (var op in hudToMoveRight) {
				hudToMoveRightOriginalPos.Add(op.GetComponent<RectTransform>().anchoredPosition.x);
			}

			foreach (var op in hudToMoveLeft) {
				hudToMoveLeftOriginalPos.Add(op.GetComponent<RectTransform>().anchoredPosition.x);
			}

        }

        #region [TimerManager]

        #endregion

        #region [CountDownManager]

        #endregion

        #region [LifesManager]

        private void InitializeLifesManager(int amount)
        {
            LifesManager.instance.Initialize(amount);
        }

        public void LifeLost()
        {
            LifesManager.instance.LifeLost();
        }

        #endregion

        #region [ScoreManager]

        public void IncreaseScore(ScoreItemsType item, float amount, float amount2)
        {
            ScoreManager.instance.IncreaseScore(item, amount, amount2);
        }

        #endregion

        # region [PowerUpsManager]

        public void EnablePowerUp(PowerUps powerUp)
        {
            PowerUpManager.Instance.EnablePowerUp(powerUp);
        }

        public void DisablePowerUp()
        {
            PowerUpManager.Instance.DisablePowerUp();
        }

        #endregion

        public void ShowGameOverScreen()
        {
            //TODO - Implementar a tela de gameover generica
        }

        #region [Enables / Disable]

        public void Enable()
        {
            ShowHUDElements();
        }


        public void Disable()
        {
            HideHUDElements();
        }

        #endregion


        public void Reset()
        {
            LifesManager.instance.Reset();
            ScoreManager.instance.Reset();
        }

		int j = 0;
        private void ShowHUDElements()
        {
            foreach (var hud in hudToMoveLeft)
            {
				/*iTween.MoveTo(hud, iTween.Hash("x", hudToMoveLeftOriginalPos[j], "islocal", true));
				j++;*/

				//scale to
				iTween.ScaleTo(hud, new Vector3(1,1,1), 1);
				//
            }
			j = 0;
            foreach (var hud in hudToMoveRight)
            {              	
				/*iTween.MoveTo(hud, iTween.Hash("x", hudToMoveRightOriginalPos[j], "islocal", true));
				j++;*/

				//scale to
				iTween.ScaleTo(hud, new Vector3(1,1,1), 1);
				//
            }
			j = 0;
        }
		int i = 0;

        private void HideHUDElements()
        {
            foreach (var hud in hudToMoveLeft)
            {
                /*iTween.MoveTo(hud, iTween.Hash("x", hudToMoveLeftPos[i], "islocal", true));
				i++;*/

				//scale to
				iTween.ScaleTo(hud, Vector3.zero, 1);
				//
            }
			i = 0;
            foreach (var hud in hudToMoveRight)
            {
                /*iTween.MoveTo(hud, iTween.Hash("x", hudToMoveRightPos[i], "islocal", true));
				i++;*/

				//scale to
				iTween.ScaleTo(hud, Vector3.zero, 1);
				//
            }
			i = 0;
        }



        IEnumerator MoveHudToLeft(GameObject hud)
        {
            //shortcut
            var pos = ScoreManager.instance.GetComponent<RectTransform>().anchoredPosition;

            //salvando pos original
            var posOriginal = pos.x;
            hudToMoveLeftPos.Add(posOriginal);

            while (Math.Round(pos.x) > posOriginal - hudToLeftDist)
            {
                hud.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.Lerp(pos.x, posOriginal - hudToLeftDist, 2 * Time.deltaTime), pos.y);
                pos = hud.GetComponent<RectTransform>().anchoredPosition;
                yield return null;
            }

            hud.GetComponent<RectTransform>().anchoredPosition = new Vector2(posOriginal - hudToLeftDist, pos.y);
        }

        IEnumerator MoveHudToRight(GameObject hud)
        {
            //shortcut
            var pos = hud.GetComponent<RectTransform>().anchoredPosition;

            //salvando pos original
            var posOriginal = pos.x;

            hudToMoveRightPos.Add(posOriginal);

            while (Mathf.Round(pos.x) < posOriginal + hudToRightDist)
            {
                hud.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.Lerp(pos.x, posOriginal + hudToRightDist, 2 * Time.deltaTime), pos.y);
                pos = hud.GetComponent<RectTransform>().anchoredPosition;
                yield return null;
            }

            hud.GetComponent<RectTransform>().anchoredPosition = new Vector2(posOriginal + hudToRightDist, pos.y);
        }

        //fix
        IEnumerator MoveHudLeftBackToOriginalPos(GameObject hud)
        {
            
            //shortcut
            var pos = ScoreManager.instance.GetComponent<RectTransform>().anchoredPosition;

            int i = hudToMoveLeft.IndexOf(hud);

            while (Mathf.Round(pos.x) < hudToMoveLeftPos[i])
            {
                hud.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.Lerp(pos.x, hudToMoveLeftPos[i], 2 * Time.deltaTime), pos.y);
                pos = hud.GetComponent<RectTransform>().anchoredPosition;
                yield return null;
            }

            //hud.GetComponent<RectTransform>().anchoredPosition = new Vector2(hudToMoveLeftPos[i], pos.y);
            hudToMoveLeftPos.Clear();

        }

        IEnumerator MoveHudRightBackToOriginalPos(GameObject hud)
        {
            var pos = hud.GetComponent<RectTransform>().anchoredPosition;

            int i = hudToMoveRight.IndexOf(hud);

            while (Mathf.Round(pos.x) > hudToMoveRightPos[i])
            {
                hud.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.Lerp(pos.x, hudToMoveRightPos[i], 2 * Time.deltaTime), pos.y);
                pos = hud.GetComponent<RectTransform>().anchoredPosition;
                yield return null;
            }

            //hud.GetComponent<RectTransform>().anchoredPosition = new Vector2(hudToMoveRightPos[i], pos.y);
            hudToMoveRightPos.Clear();
        }


    }
}
