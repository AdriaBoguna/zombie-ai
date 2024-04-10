using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerController thePlayer;

    public Image lifebarFill;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float healthPercentage = thePlayer.currentHealth / thePlayer.maxHealth;

        rectTransform.localScale = new Vector3(healthPercentage, 1, 1);

        Color32 greenColor = new Color32(6, 144, 21, 255);
        lifebarFill.color = Color.Lerp(Color.red, greenColor, healthPercentage);
    }
}
