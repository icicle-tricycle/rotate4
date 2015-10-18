using UnityEngine;
using System.Collections;

public class boardManager : MonoBehaviour {

    /// <summary>
    /// Values of pieces:
    /// 0 = empty, 1 = white, 2 = black
    /// </summary>
    piece[,] board;
    //piece[,] tempBoard;

    //public int width = 6;
    //public int height = 6;
    Vector3 origin = new Vector3(-7.95f, 0.25f, 4.25f);
    Vector3 spacing = new Vector3(1.70f, 0f, -1.70f);
    Vector3 boardOrigin;
    Vector3 screenSpacing;
    public Camera camera;

    public piece boardPiece;
	public int playerOne;
    

	// Use this for initialization
	void Start () {
        board = new piece[6, 6];
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                piece temp = Instantiate(boardPiece);
                temp.transform.position = new Vector3(
                    origin.x + spacing.x*i,
                    origin.y + spacing.y*0,
                    origin.z + spacing.z*j);
                board[i, j] = temp;
                //tempBoard[i, j] = temp;
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
                if (hit.transform.gameObject.tag == "RowTrigger")
                {
                    Debug.Log("I have triggered " + hit.transform.name);
                    AddPiece(hit.transform.gameObject.GetComponent<rowNumber>().rowNum, playerOne);
                    switchPlayers();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
			Rotate (true);
        }
		else if (Input.GetKeyDown(KeyCode.T))
		{
			Rotate (false);
		}
	}

	void AddPiece(int column, int player){
		int i;

		for (i = 0; i < board.GetLength(0); i++) {
			if(board[column, i].value != 0){
                break;
			}
		}

		if (i == 0) 
        {
			return;
		}
		board[column, i-1].value = player;

        int numWins = 0;

        numWins += checkWin(player, 0, column, i - 1, new Vector2(1, -1));
        numWins += checkWin(player, 0, column, i - 1, new Vector2(1, 0));
        numWins += checkWin(player, 0, column, i - 1, new Vector2(1, 1));
        numWins += checkWin(player, 0, column, i - 1, new Vector2(0, -1));
        numWins += checkWin(player, 0, column, i - 1, new Vector2(0, 1));
        numWins += checkWin(player, 0, column, i - 1, new Vector2(-1, -1));
        numWins += checkWin(player, 0, column, i - 1, new Vector2(-1, 0));
        numWins += checkWin(player, 0, column, i - 1, new Vector2(-1, 1));

        if (numWins > 0)
        {
            Debug.Log(player + " wins");
        }
	}

	void Rotate(bool clockwise){
        piece[,] temp;
        temp = copyBoard();
        resetBoard();
        
		if (clockwise) {
			for (int i = temp.GetLength(0) - 1; i >=0; i--) {
				for (int j = temp.GetLength(1) - 1; j >= 0; j--) {
					//Debug.Log("row " + i);
					//Debug.Log("column " + j);
					Debug.Log ("player " + temp [i, j].value);
					AddPiece (temp.GetLength (1) - j - 1, temp [i, j].value);
				}
			}
		}
		else {
			for (int i = 0; i < temp.GetLength(0); i++) {
				for (int j = temp.GetLength(1) - 1; j >= 0; j--) {
					//Debug.Log("row " + i);
					//Debug.Log("column " + j);
					Debug.Log ("player " + temp [i, j].value);
					AddPiece (j, temp [i, j].value);
				}
			}
		}
	}

    void switchPlayers()
    {
        if (playerOne == 1)
        {
            playerOne++;
            return;
        }
        playerOne = 1;
    }

    void resetBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j].value = 0;
            }
        }
    }

    piece[,] copyBoard()
    {
        piece[,] temp = new piece[6, 6];
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                piece tempPiece = new piece();
                tempPiece.value = board[i,j].value;
                temp[i, j] = tempPiece;
            }
        }
        return temp;
    }

    int checkWin(int winValue, int winStreak, int row, int col, Vector2 direction)
    {
        //Debug.Log(winStreak);
        if (winStreak == 4)
        {
            return 1;
        }
        else if (row < 0 || row >= board.GetLength(0) || col < 0 || col >= board.GetLength(1))
        {
            return 0;
        }

        if (!board[row,col].visited && winValue == board[row, col].value)
        {
            board[row, col].visited = true;
            int returnValue = checkWin(winValue, winStreak+1, row + (int)direction.x, col + (int)direction.y, direction);
            board[row, col].visited = false;

            return returnValue;
        }

        return 0;
    }
}
