namespace ChessLogic
{
    public class Result
    {
        public Player Winner { get; }
        public EndReason Reason { get; }
        public Result(Player winner, EndReason reason) // Определение результата
        {
            Winner = winner;
            Reason = reason;
        }

        public static Result Win(Player winner) // Описание победы
        {
            return new Result(winner, EndReason.Checkmate);
        }

        public static Result Draw(EndReason reason) // Описание ничьей
        {
            return new Result(Player.None, reason);
        }
    }
}
