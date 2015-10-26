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

	public Vector2 targetPos;
	public Vector2 animationPos;
	private piece below;
    private piece left;
    private piece right;
    private piece top;

	// Use this for initialization
	void Start () {
        value = 0;
		below = null;
        left = null;
        right = null;
        top = null;
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

	public piece GetBelow
	{
		get {return below;}
	}

	public void SetBelow(piece value) {
		below = value;
	}

    public piece GetLeft
    {
        get { return left; }
    }

    public void SetLeft(piece value)
    {
        left = value;
    }

    public piece GetRight
    {
        get { return right; }
    }

    public void SetRight(piece value)
    {
        right = value;
    }

    public piece GetTop
    {
        get { return top; }
    }

    public void SetTop(piece value)
    {
        top = value;
    }

    public void moveAnimation()
    {
        //not usable yet
        return;
        if(true)
        {
            Vector2 dPos = new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.z);
            dPos *= .05f;
            transform.position += new Vector3(dPos.x, 0, dPos.y);
        }

    }
	
}
