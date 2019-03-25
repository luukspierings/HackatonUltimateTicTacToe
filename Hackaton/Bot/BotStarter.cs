using System;
using System.Collections.Generic;
using System.Linq;
using Hackaton;

namespace UltimateTicTacToeBot.Bot
{
    class BotStarter
    {
        private Random rand = new Random();

		private Board macroBoard = null;
		private Board fullBoard = null;
		private BotState currentState;
		private Board[,] Microgrids = null; 


		public void CalcStarterPoints(Move move)
		{
			if (move.X == 4 && move.Y == 4) move.BestFieldPoints += 10;
		}


		public void CalcCanWinMicro(Move move)
		{
			var xPadding = (move.MacroX * 3);
			var yPadding = (move.MacroY * 3);

			var microGrid = Microgrids[move.MacroX, move.MacroY];

			if(microGrid == null)
			{
				microGrid = currentState.Field.MicroBoard(move.MacroX, move.MacroY);
				microGrid.CalculatePositions();
				Microgrids[move.MacroX, move.MacroY] = microGrid;
			}

			bool isWinnableMove = microGrid.Grid[move.X - xPadding, move.Y - yPadding].MeWinning;
			bool opponentWinnableMove = microGrid.Grid[move.X - xPadding, move.Y - yPadding].OpponentWinning;

			if (isWinnableMove)
			{
				move.BestFieldPoints += 20;
				Console.Error.WriteLine("--------------- OUR Winning move detected! ---------------");
			}

			if (opponentWinnableMove)
			{
				move.BestFieldPoints += 15;
				Console.Error.WriteLine("--------------- OPPONENT Winning move detected! ---------------");
			}

		}
		public void CalcOpponentCanWinMicro (Move move)
		{

			var mX = move.X % 3;
			var mY = move.Y % 3;

			var microGrid = Microgrids[mX, mY];

			if (microGrid == null)
			{
				microGrid = currentState.Field.MicroBoard(mX, mY);
				microGrid.CalculatePositions();
				Microgrids[mX, mY] = microGrid;
			}

			if(macroBoard.Grid[mX,mY].State != CellState.Opponent && macroBoard.Grid[mX, mY].State != CellState.Me)
			{
				move.BestFieldPoints += 5;
			}
			else
			{
				return;
			}

			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					if(microGrid.Grid[x, y].OpponentWinning)
					{
						move.BestFieldPoints -= 10;
						Console.Error.WriteLine("--------------- OPPONENT Future winning move detected! ---------------");
						return;
					}
				}
			}

		}
		public void CalcCanWinMacro(Move move)
		{
			var mX = move.MacroX;
			var mY = move.MacroY;

			if(macroBoard.Grid[mX, mY].MeWinning)
			{
				move.BestFieldPoints += 20;
				Console.Error.WriteLine("--------------- ME Can win macro row! ---------------");
				return;
			}
		}

		public void CalcOpponentCanWinMacro(Move move)
		{
			var mX = move.MacroX;
			var mY = move.MacroY;

			if (macroBoard.Grid[mX, mY].OpponentWinning)
			{
				move.BestFieldPoints += 15;
				Console.Error.WriteLine("--------------- Opponent Can win macro row! ---------------");
				return;
			}
		}

		public Move GetMove(BotState state)
        {
            var moves = state.Field.GetAvailableMoves();

			macroBoard = state.Field.MacroBoard();
			macroBoard.CalculatePositions();

			fullBoard = state.Field.FullBoard();
			currentState = state;

			Microgrids = new Board[3, 3];


			foreach (var item in moves)
			{
				CalcStarterPoints(item);

				CalcCanWinMicro(item);
				CalcOpponentCanWinMicro(item);
				CalcCanWinMacro(item);
				CalcOpponentCanWinMacro(item);
			}
				
			if (moves.Count > 0)
            {
				moves.ElementAt(rand.Next(moves.Count)).BestFieldPoints += 1;

				var bestMove = moves.OrderByDescending(m => m.BestFieldPoints).FirstOrDefault();

				return bestMove; 
            }

			return null;
        }

        static void Main(string[] args)
        {
            BotParser parser = new BotParser(new BotStarter());
            parser.Run();
        }
    }
}
