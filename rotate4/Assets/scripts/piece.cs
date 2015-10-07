using UnityEngine;
using System.Collections;

public class piece : MonoBehaviour {

    /// <summary>
    /// Values of pieces:
    /// 0 = empty, 1 = white, 2 = black
    /// </summary>
    public int value;

    public Material whiteMat;
    public Material blackMat;

	// Use this for initialization
	void Start () {
        value = 0;
	}
	
	// Update is called once per frame
	void Update () {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (value == 0)
        {
            renderer.enabled = false;
        }
        else
        {
            renderer.enabled = true;
            if(value == 1)
            {
                renderer.material = whiteMat;
            }
            else if (value == 2)
            {
                renderer.material = blackMat;
            }
        }
	}
}
