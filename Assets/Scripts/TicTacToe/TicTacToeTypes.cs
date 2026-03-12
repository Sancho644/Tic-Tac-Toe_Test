namespace TicTacToe
{
    public enum TicTacToeResult
    {
        PlayerWin,
        PlayerLose,
        Draw
    }

    public enum TicTacToeCell
    {
        Empty,
        X,
        O
    }

    public static class TicTacToeConstants
    {
        public const int Size = 3;
        public const int CellCount = Size * Size;
    }
}
