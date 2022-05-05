using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClampMapImage : MonoBehaviour
{
    public Image mapImg;             //allows mouse actions to occur when mouse hovers over avatar image.

    void Start()
    {       
        Vector3 textPos = Camera.main.WorldToScreenPoint(transform.position);   //attach to an avatar object
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //textUI.transform.position = new Vector3(textPos.x + xOffset, textPos.y + yOffset, 0);
        //place UI underneath the sprite

        //image must be enabled for mouse hover to work but alpha is reduced to 0
        mapImg.transform.position = textPos;
        mapImg.transform.localScale = new Vector3(0.5f, 0.5f, 0);
        mapImg.color = new Color(1, 1, 1, 0);
    }
}
