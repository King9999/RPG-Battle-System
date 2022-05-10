using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public enum PathState {Normal, Locked}
    public PathState pathState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPath(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    public bool PathVisible() {  return gameObject.activeSelf;}
}
