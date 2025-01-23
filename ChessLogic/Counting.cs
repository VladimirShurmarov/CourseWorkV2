using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Counting // Класс для подсчета количества фигур у каждого игрока
                          // (Необходимо для окончания игры при недостаточном количестве фигур для мата)
    {
        private readonly Dictionary<PieceType, int> whiteCount = new();
        private readonly Dictionary<PieceType, int> blackCount = new();

        public int TotalCount { get; private set; }
        public Counting() 
        {
            foreach(PieceType type in Enum.GetValues(typeof(PieceType)))
            {
                whiteCount[type] = 0;
                blackCount[type] = 0;
            }    
        }
        public void Increment(Player color, PieceType type) // Подсчет количества фигур
        {
            if (color == Player.White)
                whiteCount[type]++; 
            else if (color == Player.Black)
                blackCount[type]++;

            TotalCount++;
        }

        public int Black(PieceType type)
        { return blackCount[type]; }
        public int White(PieceType type)
        { return whiteCount[type]; }
    }
}
