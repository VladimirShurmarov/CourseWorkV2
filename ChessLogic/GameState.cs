using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }
        public Result Result { get; private set; } = null;

        private int noCaptureOrPawnMoves = 0; // Переменная для подсчета ходов для правила 50 ходов

        public GameState(Player player, Board board)
        {
            CurrentPlayer = player;
            Board = board;
        }

        public IEnumerable<Move> LegalMovesForPiece(Position pos)
        {
            if (Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[pos];
            IEnumerable<Move> moveCandidates = piece.GetMoves(pos, Board);
            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        public void MakeMove(Move move)
        {
            Board.SetPawnSkipPosition(CurrentPlayer, null); // Запрет на возможность взятия на проходе, если это не следующий ход за ходом 2 клетками
            bool captureOrPawn = move.Execute(Board); // Проверка, была ли срублена фигура или сделали ли пешка ход для правило 50 ходов
            if (captureOrPawn)
            {
                noCaptureOrPawnMoves = 0;
            }
            else
            {
                noCaptureOrPawnMoves++;
            }

            CurrentPlayer = CurrentPlayer.Opponent();
            CheckForGameOver();
        }

        public IEnumerable<Move> AllLegalMovesFor(Player player) // Определение всех возможных ходов
        {
            IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player).SelectMany(pos =>
            {
                Piece piece = Board[pos];
                return piece.GetMoves(pos, Board);
            });

            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        private void CheckForGameOver() // Определение типа результата при отсутствии возможных ходов у игрока
        {
            if (!AllLegalMovesFor(CurrentPlayer).Any())
            {
                if (Board.IsInCheck(CurrentPlayer))
                {
                    Result = Result.Win(CurrentPlayer.Opponent());
                }
                else
                {
                    Result = Result.Draw(EndReason.Stalemate);
                }
            }
            else if (Board.InsufficientMaterial())
            {
                Result = Result.Draw(EndReason.InsufficientMaterial);
            }
            else if (FiftyMoveRule())
            {
                Result = Result.Draw(EndReason.FiftyMoveRule); 
            }
        }

        public bool IsGameOver()
        {
            return Result != null;
        }

        private bool FiftyMoveRule() // Правило 50 ходов 
        {
            int fullMoves = noCaptureOrPawnMoves / 2;
            return fullMoves == 50;
        }
    }
}
