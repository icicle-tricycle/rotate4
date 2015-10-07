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
	public int player;
    

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

<<<<<<< HEAD
	void AddPiece(int column, int player){
		int spot;

		for (int i = 0; i < board.length; i++) {
			if(board[column, i].value == 0){
				spot = i;
			}
		}

		board[column, spot].value = player;
	}

	void Rotate(){
		GameObject[,] temp = board;

		for (int i = temp.Length; i >=0; i--) {
			for(int j = temp[i].length; j >= 0; j--){

			for(int j = temp[i-1].length; j >= 0; j--){
				AddPiece(temp.Length - i, temp[i,j].value);
			}
		}
	}
=======
    void AddPiece()
    {

    }

    void IsOnBoard()
    {
        //if()
    }
>>>>>>> f6a98282173cf62e0ee0918f32ea1bd3eafc5bd1
}
