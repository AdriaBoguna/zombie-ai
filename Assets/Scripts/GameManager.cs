using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public Text UItext;
    void Start()
    {
        if (gm != null && gm != this)
            Destroy(this.gameObject);
        gm = this;
        DontDestroyOnLoad(gameObject);
        
    }

    // Update is called once per frame
    public void UpdateText(string text)
    {
        UItext.text = text;        
    }
}
