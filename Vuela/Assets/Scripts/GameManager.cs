using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Image textoBack;
    public GameObject textoInfo;
    public Text siNo;
    public Text arribaAbajo;

    private float secBtwFade = 0.1f;

    private void Awake()
    {
        Instance = this;

        Color tmp;
        tmp = textoBack.color;
        tmp.a = 0;
        textoBack.color = tmp;
        foreach (Transform child in textoInfo.transform)
        {
            tmp = child.gameObject.GetComponent<Text>().color;
            tmp.a = 0;
            child.gameObject.GetComponent<Text>().color = tmp;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        arribaAbajo.text = "ABAJO";
        siNo.text = "NO";

        Invoke("fadeInText", secBtwFade);
    }


    public void SiguientePlantaMaleta()
    {
        arribaAbajo.text = "ARRIBA";
        siNo.text = "SI";

        Invoke("fadeInText", secBtwFade);
    }

    void fadeInText()
    {
        Color tmp;
        float alpha = 0;

        tmp = textoBack.color;
        tmp.a += 0.1f;
        textoBack.color = tmp;

        foreach (Transform child in textoInfo.transform)
        {
            tmp = child.gameObject.GetComponent<Text>().color;
            tmp.a += 0.1f;
            alpha = tmp.a;
            child.gameObject.GetComponent<Text>().color = tmp;
        }

        if (alpha < 1)
            Invoke("fadeInText", secBtwFade);
        else
            Invoke("fadeOutText", 2.5f);
    }

    void fadeOutText()
    {
        Color tmp;
        float alpha = 0;

        tmp = textoBack.color;
        tmp.a -= 0.1f;
        textoBack.color = tmp;

        foreach (Transform child in textoInfo.transform)
        {
            tmp = child.gameObject.GetComponent<Text>().color;
            tmp.a -= 0.1f;
            alpha = tmp.a;
            child.gameObject.GetComponent<Text>().color = tmp;
        }

        if (alpha > 0)
            Invoke("fadeOutText", secBtwFade);
    }
}
