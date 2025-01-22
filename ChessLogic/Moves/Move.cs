namespace ChessLogic
{
    public abstract class Move
    {
        public abstract MoveType Type { get; } // Определение направления хода
        public abstract Position FromPos { get; } // Определение начальных координат фигуры
        public abstract Position ToPos { get; } //  Определение конечных координат фигуры

        public abstract void Execute(Board board);

        public virtual bool IsLegal(Board board) // Определение допустимости хода (не оставит короля под угрозой)
        {
            Player player = board[FromPos].Color;
            Board boardCopy = board.Copy();
            Execute(boardCopy);
            return !boardCopy.IsInCheck(player);
        }
    }
}
