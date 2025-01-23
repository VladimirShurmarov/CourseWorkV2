using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Castle : Move
    {
        public override MoveType Type { get; }

        public override Position FromPos { get; }
        public override Position ToPos { get; }

        private readonly Direction kingMoveDir;
        private readonly Position rookFromPos;
        private readonly Position rookToPos;

        public Castle(MoveType type, Position kingPos)
        {
            Type = type;
            FromPos = kingPos;

            if (type == MoveType.CastleKS) // Определение возможности рокировки королем со своей стороны
            {
                kingMoveDir = Direction.East;
                ToPos = new Position(kingPos.Row, 6);
                rookFromPos = new Position(kingPos.Row, 7);
                rookToPos = new Position(kingPos.Row, 5);
            }
            else if (type == MoveType.CastleQS) // Определение возможности рокировки королем со стороны ферзя
            {
                kingMoveDir = Direction.West;
                ToPos = new Position(kingPos.Row, 2);
                rookFromPos = new Position(kingPos.Row, 0);
                rookToPos = new Position(kingPos.Row, 3);
            }
        }

        public override bool Execute(Board board)
        {
            new NormalMove(FromPos, ToPos).Execute(board);
            new NormalMove(rookFromPos, rookToPos).Execute(board);

            return false; // Возврат нужен для счета в правиле 50 ходов
        }

        public override bool IsLegal(Board board) // Проверка будет ли король под угрозой в связи с рокировкой
        {
            Player player = board[FromPos].Color;

            if (board.IsInCheck(player)) // Рокировку нельзя делать под угрозой
            {
                return false;
            }

            Board copy = board.Copy();
            Position kingPosInCopy = FromPos;

            for (int i = 0; i < 2; i++) // Проверка, будет ли король под угрозой при его перемещении через клетки для рокировки
            {
                new NormalMove(kingPosInCopy, kingPosInCopy + kingMoveDir).Execute(copy);
                kingPosInCopy += kingMoveDir;

                if (copy.IsInCheck(player))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
