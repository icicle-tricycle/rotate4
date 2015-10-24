﻿using UnityEngine;
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
	/// <summary>
	/// Var for if it is player one's turn
	/// </summary>
	public int playerOne;

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
    
	// Use this for initialization
	void Start () {

        whiteCanvas = GameObject.Find("CanvasWhite");
        blackCanvas = GameObject.Find("CanvasBlack");
        boardQuad = GameObject.Find("Board");
        gameState = GameState.playerInput;

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
		switch (gameState) {
		case GameState.playerInput:
		{
			if (Input.GetButtonDown("Fire1"))// && IsOnBoard())
			{
				RaycastHit hit;
				Ray ray = camera.ScreenPointToRay(Input.mousePosition);
				
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.transform.gameObject.tag == "RowTrigger")
					{
						//Debug.Log("I have triggered " + hit.transform.name);
						if(board[hit.transform.gameObject.GetComponent<rowNumber>().rowNum, 0].value == 0){
							//Debug.Log ("Made a piece");
							AddPiece(hit.transform.gameObject.GetComponent<rowNumber>().rowNum, playerOne);
							switchPlayers();
						}
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
			break;
		}
		case GameState.animation:
		{
            if (boardQuad.GetComponent<boardRotate>().rotationInterval == 0)
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
		}
		case GameState.end:
		{

			break;
		}
		default:
			break;
		}
        
	}
    //Receive column and the player value
	//Go to column and look at 
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
        board[column, i - 1].value = player;
        board[column, i - 1].animationPos = new Vector3(
                    origin.x + spacing.x * i,
                    origin.z + spacing.z * column);
        board[column, i - 1].targetPos = new Vector2(
                board[column, i - 1].GetComponent<Transform>().position.x,
                board[column, i - 1].GetComponent<Transform>().position.z
            );
        //Debug.Log(board[column, i - 1].animationPos);

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
            //Debug.Log(player + " wins");
        }
	}
	//Rotate the board clockwise or counter-clockwise
	public void Rotate(bool clockwise){
        gameState = GameState.animation;

        piece[,] temp;
        temp = copyBoard();
        resetBoard();
        
		//Start at lower right corner
		//take piece and add it to corresponding row
		//move up the column, and distribute them across
		if (clockwise) {
            boardQuad.GetComponent<boardRotate>().rotationInterval = -.4f;
			for (int i = temp.GetLength(0) - 1; i >=0; i--) {
				for (int j = temp.GetLength(1) - 1; j >= 0; j--) {
					//Debug.Log("row " + i);
					//Debug.Log("column " + j);
					//Debug.Log ("player " + temp [i, j].value);
					AddPiece (temp.GetLength (1) - j - 1, temp [i, j].value);
				}
			}
			//grid.transform.rotation = Quaternion.Slerp(new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), new Quaternion(90.0f, 0.0f, 0.0f, 0.0f), 5.0f);
		}
		//Start at lower left corner
		//take piece and add it to corresponding row
		//move up the column, and distribute them across
		else {
            boardQuad.GetComponent<boardRotate>().rotationInterval = .4f;
			for (int i = 0; i < temp.GetLength(0); i++) {
				for (int j = temp.GetLength(1) - 1; j >= 0; j--) {
					//Debug.Log("row " + i);
					//Debug.Log("column " + j);
					//Debug.Log ("player " + temp [i, j].value);
					AddPiece (j, temp [i, j].value);
				}
			}
		}

        
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
