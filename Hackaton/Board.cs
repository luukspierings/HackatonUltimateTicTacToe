using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackaton
{
	class Board
	{
		public Cell[,] Grid { get; set; }

		public void CalculatePositions()
		{
			int xLength = Grid.GetLength(0);
			int yLength = Grid.GetLength(1);

			var board = new List<CellState>();


			for (int x = 0; x < Grid.GetLength(0); x++)
			{
				for (int y = 0; y < Grid.GetLength(1); y++)
				{
					board.Add(Grid[x, y].State);
				}
			}

			for (int x = 0; x < xLength; x++)
			{
				for (int y = 0; y < yLength; y++)
				{
					CellState prev = board[x * xLength + y];
					var cell = Grid[x, y];

					if (prev != CellState.Playable) continue;

					// calc me
					board[x * xLength + y] = CellState.Me;
					cell.MeWinning = WinningMove(board, CellState.Me);

					//calc opponent
					board[x * xLength + y] = CellState.Opponent;
					cell.OpponentWinning = WinningMove(board, CellState.Opponent);

					board[x * xLength + y] = prev;
				}
			}
		}

		private bool WinningMove(List<CellState> board, CellState player)
		{
			return (
				(board[0] == player && board[1] == player && board[2] == player) ||
				(board[3] == player && board[4] == player && board[5] == player) ||
				(board[6] == player && board[7] == player && board[8] == player) ||
				(board[0] == player && board[3] == player && board[6] == player) ||
				(board[1] == player && board[4] == player && board[7] == player) ||
				(board[2] == player && board[5] == player && board[8] == player) ||
				(board[0] == player && board[4] == player && board[8] == player) ||
				(board[2] == player && board[4] == player && board[6] == player)
			);
		}
	}

	public class Cell
	{
		public CellState State { get; set; }

		public bool MeWinning { get; set; }
		public bool OpponentWinning { get; set; }

	}

	public enum WinState
	{

	}

	public enum CellState
	{
		Playable,
		Undefined,
		Me,
		Opponent,
		Draw
	}
}
