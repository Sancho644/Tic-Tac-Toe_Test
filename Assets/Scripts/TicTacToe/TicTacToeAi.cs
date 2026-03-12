using System;
using System.Collections.Generic;

namespace TicTacToe
{
    public class TicTacToeAi
    {
        private readonly TicTacToeBoard _board;
        private readonly System.Random _rng = new System.Random();

        public TicTacToeAi(TicTacToeBoard board)
        {
            _board = board;
        }

        public int ChooseMove()
        {
            var winMove = _board.FindWinningMove(TicTacToeCell.O);
            if (winMove >= 0)
            {
                return winMove;
            }

            var blockMove = _board.FindWinningMove(TicTacToeCell.X);
            if (blockMove >= 0)
            {
                return blockMove;
            }

            if (_board.IsEmpty(4))
            {
                return 4;
            }

            var corners = new[] { 0, 2, 6, 8 };
            var availableCorners = new List<int>();
            foreach (var corner in corners)
            {
                if (_board.IsEmpty(corner))
                {
                    availableCorners.Add(corner);
                }
            }

            if (availableCorners.Count > 0)
            {
                return availableCorners[_rng.Next(availableCorners.Count)];
            }

            var available = _board.GetEmptyIndices();
            if (available.Count == 0)
            {
                return -1;
            }

            return available[_rng.Next(available.Count)];
        }
    }
}
