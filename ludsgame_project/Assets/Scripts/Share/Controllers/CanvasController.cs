using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    public static CanvasController instance;

    void Awake()
    {
        instance = this;
    }
    
    #region [Enables / Disable]

    public void Enable()
    {
        StartCoroutine(DeactivateBlackBackGround());
    }

    public void Disable()
    {
        StartCoroutine(ActivateBlackBackGround());
    }


    #endregion

    IEnumerator ActivateBlackBackGround()
    {
        while (this.GetComponent<Image>().color.a < 0.6f)
        {
            if (this.GetComponent<Image>().color.a == 0.6f)
            {
                print("cor 0.6");
            }
            this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g,
                                                         this.GetComponent<Image>().color.b, (this.GetComponent<Image>().color.a + 0.01f));
            yield return null;
        }
    }

    IEnumerator DeactivateBlackBackGround()
    {
        while (this.GetComponent<Image>().color.a > 0)
        {
            this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g,
                                                         this.GetComponent<Image>().color.b, (this.GetComponent<Image>().color.a - 0.01f));
            yield return null;
        }
    }
}
