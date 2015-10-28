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
	//private float timer = 0;

    public piece boardPiece;
	/// <summary>
	/// Var for if it is player one's turn
	/// </summary>
	public int playerOne;
	public int boardState;
    public int framesSinceRotate = 7;
	public int winner = 0;

	public enum GameState
	{
		playerInput, animation, end
	}
	public GameState gameState;

    //public GameObject whiteCanvas;
    //public GameObject blackCanvas; 
    private GameObject whiteCanvas;
    private GameObject blackCanvas;
	private GameObject boardQuad;
	private GameObject blackWinsCanvas;
	private GameObject whiteWinsCanvas;
	private GameObject tieCanvas;
    
	// Use this for initialization
	void Start () {

        whiteCanvas = GameObject.Find("CanvasWhite");
        blackCanvas = GameObject.Find("CanvasBlack");
        boardQuad = GameObject.Find("Board");
		blackWinsCanvas = GameObject.Find ("BlackWins");
		whiteWinsCanvas = GameObject.Find ("WhiteWins");
		tieCanvas = GameObject.Find ("Tie");
		blackWinsCanvas.SetActive (false);
		whiteWinsCanvas.SetActive (false);
		tieCanvas.SetActive (false);
        gameState = GameState.playerInput;
		boardState = 0;

        board = new piece[6, 6];
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                piece temp = Instantiate(boardPiece);
                temp.transform.parent = boardQuad.transform;
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
		UpdatePieceLinks();
	}
	
	// Update is called once per frame
	void Update () {
		switch (gameState) 
        {
		    case GameState.playerInput:
			    UpdatePieceLinks();
			/*timer += Time.deltaTime;
			//Debug.Log (timer);
			if(timer > .3){
				framesSinceRotate++;*/
				Fall ();
				/*timer = 0;
			}*/
                //printBoard();
			    if (Input.GetButtonDown("Fire1"))// && IsOnBoard())
			    {
				    RaycastHit hit;
				    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
				
				    if (Physics.Raycast(ray, out hit))
				    {
					    if (hit.transform.gameObject.tag == "RowTrigger")
					    {
                            //Debug.Log("I have triggered " + hit.transform.gameObject.GetComponent<rowNumber>().rowNum);
						    //if(board[hit.transform.gameObject.GetComponent<rowNumber>().rowNum, 0].value == 0){
							    //Debug.Log ("Made a piece");
							    AddPiece(hit.transform.gameObject.GetComponent<rowNumber>().rowNum, playerOne, board);
							    //switchPlayers();
						    //}
					    }
				    }
			    }
			    else if (Input.GetKeyDown(KeyCode.R))
			    {
				    Rotate (true);
				    switchPlayers();
			    }
			    else if (Input.GetKeyDown(KeyCode.T))
			    {
				    Rotate (false);
				    switchPlayers();
			    }

				int numWins = 0;
				int player1Wins = 0;
				int player2Wins = 0;
				
				if (/*(timer > .3) &&*/ (framesSinceRotate == board.GetLength(0)+2))
				{
					for (int i = 0; i < board.GetLength(0); i++)
					{
						for (int j = 0; j < board.GetLength(1); j++)
						{
							numWins = 0;
							if (board[i, j].value != 0)
							{
								numWins += checkWin(board[i, j].value, 0, i, j, new Vector2(1, -1), board);
								numWins += checkWin(board[i, j].value, 0, i, j, new Vector2(1, 0), board);
								numWins += checkWin(board[i, j].value, 0, i, j, new Vector2(1, 1), board);
								numWins += checkWin(board[i, j].value, 0, i, j, new Vector2(0, -1), board);
								numWins += checkWin(board[i, j].value, 0, i, j, new Vector2(0, 1), board);
								numWins += checkWin(board[i, j].value, 0, i, j, new Vector2(-1, -1), board);
								numWins += checkWin(board[i, j].value, 0, i, j, new Vector2(-1, 0), board);
								numWins += checkWin(board[i, j].value, 0, i, j, new Vector2(-1, 1), board);
								
								if (board[i, j].value == 1)
								{
									player1Wins += numWins;
								}
								else
								{
									player2Wins += numWins;
								}
							}
						}
					}
				}
			
				if (player1Wins != 0 || player2Wins != 0) 
				{
					gameEnd (player1Wins/2, player2Wins/2);
				}
				//comment this out during timer implementation
                framesSinceRotate++;
			    break;
		    case GameState.animation:
                if (boardQuad.GetComponent<boardRotate>().RotationInterval == 0)
                {
                    gameState = GameState.playerInput;
                }
                //boardQuad.transform.Rotate(new Vector3(0f, 0f, 0.4f));
                boardQuad.GetComponent<boardRotate>().rotate();
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        board[i, j].GetComponent<piece>().moveAnimation();
                    }
                }
			    break;
		    case GameState.end:
				if(winner == 1){
				whiteWinsCanvas.SetActive(true);
			}else if(winner == 2){
				blackWinsCanvas.SetActive(true);
			}else if(winner == 3){
				tieCanvas.SetActive(true);
			}
			    break;
		    default:
			    break;
		}

        
        
	}
    //Receive column and the player value
	//Go to column and look at 
	void AddPiece(int column, int player, piece[,] tempBoard)
    {
		int i;

        if (boardState == 0)
        {
            for (i = 0; i < tempBoard.GetLength(0); i++)
            {
                if (tempBoard[column, i].value != 0)
                {
                    break;
                }
            }

            if (i == 0)
            {
                return;
            }
            i--;
        }
        else if (boardState == 1)
        {
            column = tempBoard.GetLength(1) - column - 1;
            for (i = 0; i < tempBoard.GetLength(0); i++)
            {
                if (tempBoard[i, column].value != 0)
                {
                    break;
                }
            }

            if (i == 0)
            {
                return;
            }

            int temp = i;
            i = column;
            column = temp;
            column--;
        }
        else if (boardState == 2)
        {
            column = tempBoard.GetLength(0) - column - 1;
            for (i = tempBoard.GetLength(0) - 1; i >= 0; i--)
            {
                if (tempBoard[column, i].value != 0)
                {
                    break;
                }
            }

            if (i == tempBoard.GetLength(0) - 1)
            {
                return;
            }

            i++;
        }
        else
        {
            for (i = tempBoard.GetLength(0) - 1; i >= 0; i--)
            {
                if (tempBoard[i, column].value != 0)
                {
                    break;
                }
            }

            if (i == tempBoard.GetLength(0) - 1)
            {
                return;
            }

            int temp = i;
            i = column;
            column = temp;
            column++;
        }

        tempBoard[column, i].value = player;

        int numWins = 0;

        numWins += checkWin(player, 0, column, i, new Vector2(1, -1), tempBoard);
        numWins += checkWin(player, 0, column, i, new Vector2(1, 0), tempBoard);
        numWins += checkWin(player, 0, column, i, new Vector2(1, 1), tempBoard);
        numWins += checkWin(player, 0, column, i, new Vector2(0, -1), tempBoard);
        numWins += checkWin(player, 0, column, i, new Vector2(0, 1), tempBoard);
        numWins += checkWin(player, 0, column, i, new Vector2(-1, -1), tempBoard);
        numWins += checkWin(player, 0, column, i, new Vector2(-1, 0), tempBoard);
        numWins += checkWin(player, 0, column, i, new Vector2(-1, 1), tempBoard);

        //Debug.Log("Check for wins");
        if (numWins > 0)
        {
            Debug.Log(player + " wins");
			if(player == 1)
			{
				gameEnd(1, 0);
			}
			else{
				gameEnd(0, 1);
			}
        }
        switchPlayers();
	}
	//Rotate the board clockwise or counter-clockwise
	public void Rotate(bool clockwise){
        gameState = GameState.animation;

        piece[,] temp;
        temp = copyBoard();
        
		if (clockwise) {
            boardQuad.GetComponent<boardRotate>().RotationInterval = -3.0f;
			boardState++;
			if(boardState > 3)
				boardState = 0;
		}
		else {
            boardQuad.GetComponent<boardRotate>().RotationInterval = 3.0f;
			boardState--;
			if(boardState < 0)
				boardState = 3;
		}
        framesSinceRotate = 0;
    }
    /// <summary>
    /// Changes player variable and alpha of turn indicator canvases
    /// </summary>
    public void switchPlayers()
    {
        if (playerOne == 1)
        {
            whiteCanvas.GetComponent<CanvasGroup>().alpha = 0.0f;
            blackCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
            playerOne++;
            return;
        }
        whiteCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        blackCanvas.GetComponent<CanvasGroup>().alpha = 0.0f;
        playerOne = 1;
    }
    /// <summary>
    /// Makes the board empty
    /// </summary>
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
    /// <summary>
    /// Makes a copy of the current board
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// checks if a winning line of pieces has been made
    /// </summary>
    /// <param name="winValue">Which value of piece to use for the check</param>
    /// <param name="winStreak">The amount of pieces we have in a line already, should be 0 when this isn't called by itself</param>
    /// <param name="row">the horizontal line to check</param>
    /// <param name="col">the vertical line to check</param>
    /// <param name="direction">a vec2 of where to check the next piece in the winning row</param>
    /// <returns></returns>
    int checkWin(int winValue, int winStreak, int row, int col, Vector2 direction, piece[,] tempBoard)
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

        if (!board[row, col].visited && winValue == board[row, col].value)
        {
            board[row, col].visited = true;
            int returnValue = checkWin(winValue, winStreak + 1, row + (int)direction.x, col + (int)direction.y, direction, board);
            board[row, col].visited = false;

            return returnValue;
        }

        return 0;
    }
	//Resets the the piece that is below each piece
	void UpdatePieceLinks()
	{
		for (int i = 0; i < board.GetLength(0); i++) {
			for (int j = 0; j < board.GetLength(1); j++){
                if (j < board.GetLength(1) - 1)
                {
                    board[i, j].SetBelow(board[i, j + 1]);
                }
                else
                {
                    board[i, j].SetBelow(null);
                }

                if (i < board.GetLength(0) - 1)
                {
                    board[i, j].SetRight(board[i + 1, j]);
                }
                else
                {
                    board[i, j].SetRight(null);
                }

                if (i > 0)
                {
                    board[i, j].SetLeft(board[i - 1, j]);
                }
                else
                {
                    board[i, j].SetLeft(null);
                }

                if (j > 0)
                {
                    board[i, j].SetTop(board[i, j - 1]);
                }
                else
                {
                    board[i, j].SetTop(null);
                }
			}
		}
	}
	//Causes pieces to fall, by setting the piece below to value and setting original to 0
	void Fall()
	{
		for (int i = board.GetLength(0) - 1; i >=0; i--) {
			for (int j = board.GetLength(1) - 1; j >= 0; j--) {
                if (boardState == 0 && board[i, j].GetBelow != null && board[i, j].GetBelow.value == 0)
                {
                    board[i, j].GetBelow.value = board[i, j].value;
                    board[i, j].value = 0;
                }
                else if (boardState == 1 && board[i, j].GetRight != null && board[i, j].GetRight.value == 0)
                {
                    board[i, j].GetRight.value = board[i, j].value;
                    board[i, j].value = 0;
                }
                else if (boardState == 2 && board[i, j].GetTop != null && board[i, j].GetTop.value == 0)
                {
                    board[i, j].GetTop.value = board[i, j].value;
                    board[i, j].value = 0;
                }
                else if (boardState == 3 && board[i, j].GetLeft != null && board[i, j].GetLeft.value == 0)
                {
                    board[i, j].GetLeft.value = board[i, j].value;
                    board[i, j].value = 0;
                }
			}
		}
	}

    void printBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j].value != 0)
                {
                    Debug.Log("Player " +board[i,j].value+" location["+i+"+"+j+"]");
                }
            }
        }
    }
	void gameEnd(int whiteWins, int blackWins)
	{
		Debug.Log ("White wins: " + whiteWins + ", Black wins: " + blackWins);
		if (whiteWins > blackWins) {
			winner = 1;
			gameState = GameState.end;
		} else if (blackWins > whiteWins) {
			winner = 2;
			gameState = GameState.end;
		} else {
			winner = 3;
			gameState = GameState.end;
		}
	}
}
