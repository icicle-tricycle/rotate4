using UnityEngine;
using System.Collections;

public class boardManager : MonoBehaviour {

    /// <summary>
    /// Values of pieces:
    /// 0 = empty, 1 = white, 2 = black
    /// </summary>
    GameObject[,] board;
    Vector3 origin = new Vector3(-4.75f, 0.25f, 3.15f);
    Vector3 spacing = new Vector3(1.9f, 0f, -1.26f);
    Vector3 boardOrigin;
    Vector3 screenSpacing;
    public Camera camera;

    public GameObject boardPiece;
    

	// Use this for initialization
	void Start () {
        board = new GameObject[6, 6];
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                GameObject temp = Instantiate(boardPiece);
                temp.transform.position = new Vector3(
                    origin.x + spacing.x*i,
                    origin.y + spacing.y*0,
                    origin.z + spacing.z*j);
                board[i, j] = temp;
            }
        }
        //camera = GetComponent<Camera>();
        boardOrigin = camera.WorldToScreenPoint(origin);
        screenSpacing = camera.WorldToScreenPoint(spacing);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))// && IsOnBoard())
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycasting");
                if (hit.transform.gameObject.tag == "RowTrigger")
                {
                    Debug.Log("I have triggered " + hit.transform.name);
                }
            }
            Debug.Log("Mouse 1 " + Input.mousePosition);
        }
	}

    void AddPiece()
    {

    }

    void IsOnBoard()
    {
        //if()
    }
}
