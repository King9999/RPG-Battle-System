using UnityEngine;
using TMPro;

//This is used for any UI to follow a game object.
public class ClampText : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    //public float xOffset, yOffset;

    // Start is called before the first frame update
    void Start()
    {       
        Vector3 textPos = Camera.main.WorldToScreenPoint(transform.position);   //attach to an avatar object
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //textUI.transform.position = new Vector3(textPos.x + xOffset, textPos.y + yOffset, 0);
        //place UI underneath the sprite
        textUI.transform.position = new Vector3(textPos.x, textPos.y - (sr.bounds.extents.y * 65), 0);
    }
}
