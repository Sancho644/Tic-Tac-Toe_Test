using System.Collections.Generic;

namespace TicTacToe
{
    public class TicTacToeBoard
    {
        private readonly TicTacToeCell[] _cells = new TicTacToeCell[TicTacToeConstants.CellCount];

        public void Reset()
        {
            for (var i = 0; i < _cells.Length; i++)
            {
                _cells[i] = TicTacToeCell.Empty;
            }
        }

        public bool IsEmpty(int index)
        {
            return _cells[index] == TicTacToeCell.Empty;
        }

        public void Place(int index, TicTacToeCell mark)
        {
            _cells[index] = mark;
        }

        public bool IsFull()
        {
            for (var i = 0; i < _cells.Length; i++)
            {
                if (_cells[i] == TicTacToeCell.Empty)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckWin(TicTacToeCell mark)
        {
            return CheckLine(0, 1, 2, mark)
                || CheckLine(3, 4, 5, mark)
                || CheckLine(6, 7, 8, mark)
                || CheckLine(0, 3, 6, mark)
                || CheckLine(1, 4, 7, mark)
                || CheckLine(2, 5, 8, mark)
                || CheckLine(0, 4, 8, mark)
                || CheckLine(2, 4, 6, mark);
        }

        public int FindWinningMove(TicTacToeCell mark)
        {
            for (var i = 0; i < _cells.Length; i++)
            {
                if (_cells[i] != TicTacToeCell.Empty)
                {
                    continue;
                }

                _cells[i] = mark;
                var win = CheckWin(mark);
                _cells[i] = TicTacToeCell.Empty;

                if (win)
                {
                    return i;
                }
            }

            return -1;
        }

        public List<int> GetEmptyIndices()
        {
            var result = new List<int>();
            for (var i = 0; i < _cells.Length; i++)
            {
                if (_cells[i] == TicTacToeCell.Empty)
                {
                    result.Add(i);
                }
            }

            return result;
        }

        private bool CheckLine(int a, int b, int c, TicTacToeCell mark)
        {
            return _cells[a] == mark && _cells[b] == mark && _cells[c] == mark;
        }
    }
}
