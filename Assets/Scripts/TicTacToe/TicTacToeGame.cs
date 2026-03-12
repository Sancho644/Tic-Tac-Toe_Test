using System;

namespace TicTacToe
{
    public class TicTacToeGame
    {
        private readonly TicTacToeBoard _board;
        private readonly TicTacToeAi _ai;
        private readonly ITicTacToeUi _ui;

        private Action<TicTacToeResult> _onFinished;
        private bool _playerTurn;

        public bool IsRunning { get; private set; }
        public event Action<bool> GameRunningChanged;

        public TicTacToeGame(TicTacToeBoard board, TicTacToeAi ai, ITicTacToeUi ui)
        {
            _board = board;
            _ai = ai;
            _ui = ui;
            _ui.BindCellClicked(OnCellClicked);
        }

        public void StartGame(Action<TicTacToeResult> onFinished, bool playerStarts)
        {
            _onFinished = onFinished;
            _playerTurn = playerStarts;
            IsRunning = true;
            _board.Reset();
            _ui.ResetCells();
            _ui.Show(true);
            UpdateStatus();
            GameRunningChanged?.Invoke(true);

            if (!_playerTurn)
            {
                MakeAiMove();
            }
        }

        public void StopGame()
        {
            IsRunning = false;
            _onFinished = null;
            _ui.Show(false);
            GameRunningChanged?.Invoke(false);
        }

        private void OnCellClicked(int index)
        {
            if (!IsRunning || !_playerTurn || !_board.IsEmpty(index))
            {
                return;
            }

            PlaceMark(index, TicTacToeCell.X);
            if (CheckGameEnd())
            {
                return;
            }

            _playerTurn = false;
            UpdateStatus();
            MakeAiMove();
        }

        private void MakeAiMove()
        {
            if (!IsRunning)
            {
                return;
            }

            var move = _ai.ChooseMove();
            if (move >= 0)
            {
                PlaceMark(move, TicTacToeCell.O);
            }

            if (CheckGameEnd())
            {
                return;
            }

            _playerTurn = true;
            UpdateStatus();
        }

        private void PlaceMark(int index, TicTacToeCell mark)
        {
            _board.Place(index, mark);
            _ui.SetCell(index, mark == TicTacToeCell.X ? "X" : "O", false);
        }

        private bool CheckGameEnd()
        {
            if (_board.CheckWin(TicTacToeCell.X))
            {
                EndGame(TicTacToeResult.PlayerWin, "Победа");
                return true;
            }

            if (_board.CheckWin(TicTacToeCell.O))
            {
                EndGame(TicTacToeResult.PlayerLose, "Поражение");
                return true;
            }

            if (_board.IsFull())
            {
                EndGame(TicTacToeResult.Draw, "Ничья");
                return true;
            }

            return false;
        }

        private void EndGame(TicTacToeResult result, string status)
        {
            IsRunning = false;
            _ui.SetStatus(status);
            _ui.SetAllInteractable(false);
            _onFinished?.Invoke(result);
            GameRunningChanged?.Invoke(false);
        }

        private void UpdateStatus()
        {
            _ui.SetStatus(_playerTurn ? "Ваш ход" : "Ход соперника");
        }
    }
}
