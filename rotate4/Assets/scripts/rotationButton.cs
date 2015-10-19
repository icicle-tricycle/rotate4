using UnityEngine;
using System.Collections;

public class rotationButton : MonoBehaviour {

    //0 = ccw, 1 = cw
    public bool rotDir;
    /// <summary>
    /// The board manager game object
    /// </summary>
    public GameObject bm;

    void Start()
    {
        //bm = GameObject.Find("boardManager");
    }

    public void onClick()
    {
        Debug.Log("clicked");
        bm.GetComponent<boardManager>().Rotate(rotDir);
        bm.GetComponent<boardManager>().switchPlayers();
    }
}
