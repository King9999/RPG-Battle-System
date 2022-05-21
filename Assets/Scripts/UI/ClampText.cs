using UnityEngine;
using TMPro;
using UnityEngine.UI;

//This is used for any UI to follow a game object.
public class ClampText : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public Image avatarImg;             //allows mouse actions to occur when mouse hovers over avatar image.
    //public float xOffset, yOffset;

    // Start is called before the first frame update
    void Start()
    {       
        //Vector3 textPos = Camera.main.WorldToScreenPoint(transform.position);   //attach to an avatar object
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //place UI underneath the sprite
        //textUI.transform.position = new Vector3(textPos.x, textPos.y - (sr.bounds.extents.y * 65), 0);

        //image must be enabled for mouse hover to work but alpha is reduced to 0
        //avatarImg.transform.position = textPos;
        avatarImg.color = new Color(1, 1, 1, 0);
        UpdateUIPosition();
    }

    public void UpdateUIPosition()
    {
        Vector3 textPos = Camera.main.WorldToScreenPoint(transform.position);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //place UI underneath the sprite
        textUI.transform.position = new Vector3(textPos.x, textPos.y - (sr.bounds.extents.y * 65), 0);
        avatarImg.transform.position = textPos;
    }
}
