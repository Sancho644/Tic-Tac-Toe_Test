using System;

namespace TicTacToe
{
    public interface ITicTacToeUi
    {
        void BindCellClicked(Action<int> onCellClicked);
        void Show(bool show);
        void ResetCells();
        void SetCell(int index, string text, bool interactable);
        void SetAllInteractable(bool interactable);
        void SetStatus(string text);
    }
}
