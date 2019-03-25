using System;

namespace UltimateTicTacToeBot
{
    /// <summary>
    /// Stores a move.
    /// </summary>
    class Move
    {
        public int X { get; set; }
		public int Y { get; set; }

		public int MacroX { get; set; }
		public int MacroY { get; set; }
		public int BestFieldPoints { get; set; }

        override public String ToString()
        {
            return String.Format("place_move {0} {1}", X, Y);
        }
    }
}
