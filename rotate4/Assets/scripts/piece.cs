using UnityEngine;
using System.Collections;

public class piece : MonoBehaviour {

    /// <summary>
    /// Values of pieces:
    /// 0 = empty, 1 = white, 2 = black
    /// </summary>
    public int value;
    public bool visited;

    public Material whiteMat;
    public Material blackMat;

	// Use this for initialization
	void Start () {
        value = 0;
	}

    public Vector2 targetPos;
    public Vector2 animationPos;
	
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

    public void moveAnimation()
    {
        return;
        if(true)
        {
            Vector2 dPos = new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.z);
            dPos *= .5f;
            transform.position += new Vector3(dPos.x, 0, dPos.y);
        }

    }
}
