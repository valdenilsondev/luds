using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Share.Managers;
using System.Collections.Generic;

public class LifesManager : MonoBehaviour
{
    private int lifes = 3;
    private bool isDead = false;
    private GameObject heartPrefab;

    private Sprite emptyHeart;
    private Sprite fullHeart;

    private List<Image> hearts;

    //coracao principal
    private GameObject mainHeartReference;

    public static LifesManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //chamada pra criar coracoes
    }

    public void Reset()
    {
        Initialize(PlayerPrefsManager.GetNumberOfLifes());
    }

    public void Initialize(int amount)
    {
        InitializeHearts(amount);
    }

    #region [Public Methods]

    public void InitializeHearts(int numberOfHearts)
    {
		lifes = numberOfHearts;
		this.transform.GetComponentInChildren<Text> ().text = lifes + "X";
    }

    public void LifeLost()
    {
        lifes--;
        this.transform.GetComponentInChildren<Text> ().text = lifes + "X";
    }

	public int GetCurrentNumberOfHearts(){
		return lifes;
	}


    public bool IsDead()
    {
        if (lifes <= -1)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }
        return isDead;
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }

    #endregion

    #region [Private Methods]

    private void RemoveHeart()
    {
        if (!IsDead())
            StartCoroutine(Blink(hearts[lifes].gameObject, 5, true));
    }

    #endregion

    IEnumerator Blink(GameObject life, int times, bool disable)
    {
        var original = life.GetComponent<Image>().sprite;

        for (int i = 0; i < times; i++)
        {
            life.GetComponent<Image>().sprite = emptyHeart;
            yield return new WaitForSeconds(0.3f);
            life.GetComponent<Image>().sprite = original;
            yield return new WaitForSeconds(0.3f);
        }
        if (disable)
        {
            life.GetComponent<Image>().sprite = emptyHeart;
        }
    }  

}


