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


	}
	
	// Update is called once per frame
	void Update () {
	
	}

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

			}
		}
	}
}
