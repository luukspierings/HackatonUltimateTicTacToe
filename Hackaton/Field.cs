using System;
using System.Collections.Generic;
using System.Text;
using Hackaton;

namespace UltimateTicTacToeBot
{
    /// <summary>
    /// Handles everything that has to do with the field, such as storing 
    /// the current state and performing calculations on the field.
    /// </summary>
    class Field
    {
        public const String EmptyField = ".";
        public const String AvailableField = "-1";
        public const int COLS = 9;
        public const int ROWS = 9;

        public int MyId { get; set; }
        public int OpponentId { get; set; }      
        
        private String[,] board;
        private String[,] macroboard;

        public Field()
        {
            board = new String[COLS, ROWS];
            macroboard = new String[COLS / 3, ROWS / 3];
            ClearBoard();
        }

        /// <summary>
        /// Initialise field from comma separated String
        /// </summary>
        /// <param name="s"></param>
        public void ParseFromString(String s)
        {
            s = s.Replace(";", ",");
            String[] r = s.Split(',');
            int counter = 0;
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    board[x, y] = r[counter];
                    counter++;
                }
            }
        }

        /// <summary>
        /// Initialise macroboard from comma separated String
        /// </summary>
        /// <param name="s"></param>
        public void ParseMacroboardFromString(String s)
        {
            String[] r = s.Split(',');
            int counter = 0;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    macroboard[x, y] = r[counter];
                    counter++;
                }
            }
        }

		public Board FullBoard()
		{
			var b = new Board {
				Grid = new Cell[9, 9]
			};

			var myString = MyId.ToString();
			var opponentString = OpponentId.ToString();

			for (int y = 0; y < 9; y++)
			{
				for (int x = 0; x < 9; x++)
				{
					var test = board[x, y];
					CellState state;

					if (test == myString)
					{
						state = CellState.Me;
					}
					else if (test == opponentString)
					{
						state = CellState.Opponent;
					}
					else
					{
						state = CellState.Playable;
					}

					b.Grid[x,y] = new Cell()
					{
						State = state
					};
				}
			}
			return b;

		}

		public Board MicroBoard(int macroX, int macroY)
		{
			var b = new Board();
			b.Grid = new Cell[3, 3];

			var myString = MyId.ToString();
			var opponentString = OpponentId.ToString();

			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					var test = board[x + macroX * 3, y + macroY * 3];
					CellState state;


					if (test == myString)
					{
						state = CellState.Me;
					}
					else if (test == opponentString)
					{
						state = CellState.Opponent;
					}
					else
					{
						state = CellState.Playable;
					}

					b.Grid[x, y] = new Cell()
					{
						State = state
					};
				}
			}

			return b;
		}


		public Board MacroBoard()
		{
			var board = new Board();
			board.Grid = new Cell[3,3];

			var myString = MyId.ToString();
			var opponentString = OpponentId.ToString();

			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					var test = macroboard[x, y];
					CellState state;

					if (test == myString) {
						state = CellState.Me;
					}
					else if ( test == opponentString)
					{
						state = CellState.Opponent;
					}
					else if (test == AvailableField)
					{
						state = CellState.Playable;
					}
					else
					{
						state = CellState.Undefined;
					}

					board.Grid[x,y] = new Cell()
					{
						State = state
					};
				}
			}

			return board;
		}




        public void ClearBoard()
        {
            for (int x = 0; x < COLS; x++)
            {
                for (int y = 0; y < ROWS; y++)
                {
                    board[x, y] = EmptyField;
                }
            }
        }

        public List<Move> GetAvailableMoves()
        {
            var moves = new List<Move>();

            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    if (IsInActiveMicroboard(x, y) && board[x, y] == EmptyField)
                    {
                        moves.Add(new Move
						{
							X = x,
							Y = y,
							MacroX = x/3,
							MacroY = y/3,
							BestFieldPoints = 0
						});
                    }
                }
            }

            return moves;
        }

        public bool IsInActiveMicroboard(int x, int y)
        {
            return macroboard[x / 3, y / 3] == AvailableField;
        }
        

        /// <summary>
        /// Creates comma separated String with player ids for the microboards.
        /// </summary>
        /// <returns>String with player names for every cell, or 'empty' when cell is empty.</returns>
        override public String ToString()
        {
            var r = new StringBuilder("");
            int counter = 0;
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                {
                    if (counter > 0)
                    {
                        r.Append(",");
                    }
                    r.Append(board[x, y]);
                    counter++;
                }
            }
            return r.ToString();
        }


        /// <summary>
        /// Checks whether the field is full
        /// </summary>
        /// <returns>Returns true when field is full, otherwise returns false</returns>
        public bool IsFull()
        {
            for (int x = 0; x < COLS; x++)
                for (int y = 0; y < ROWS; y++)
                    if (board[x, y] == EmptyField)
                        return false; // At least one cell is not filled

            // All cells are filled
            return true;
        }
        
        public bool IsEmpty()
        {
            for (int x = 0; x < COLS; x++)
            {
                for (int y = 0; y < ROWS; y++)
                {
                    if (board[x, y] != EmptyField)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


		public bool IsEnemy(int x, int y)
		{
			return board[x, y] == OpponentId.ToString();
		}

		public bool IsMe(int x, int y)
		{
			return board[x, y] == MyId.ToString();
		}

        /// <summary>
        /// Returns the player id for the given column and row
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public String GetPlayerId(int column, int row)
        {
            return board[column, row];
        }
    }
}
