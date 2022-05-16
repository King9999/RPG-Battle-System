using UnityEngine;
using TMPro;

/* Used to inform player of events while in a dungeon. */
public class Notification : MonoBehaviour
{
    public TextMeshProUGUI popup;               //the message to convey to player.
    float duration;                             //time in seconds to display duration for.
    float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        duration = 3;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf && Time.time > currentTime + duration)
        {
            gameObject.SetActive(false);
        }
    }

    public void DisplayMessage(string msg)
    {
        gameObject.SetActive(true);
        popup.text = msg;
        currentTime = Time.time;
    }
}
