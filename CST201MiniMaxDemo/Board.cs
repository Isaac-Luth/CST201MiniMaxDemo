using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST201MiniMaxDemo
{
    public class Board
    {
        public readonly int Size = 3;
        public CellState[,] Grid { get; private set; }
        public int MovesMade { get; private set; }
        public long MaxMoves { get; init; }

        public enum CellState
        {
            Player,
            Computer,
            Empty
        }

        public enum WinnerStatus
        {
            Player,
            Computer,
            Tie,
            InProgress
        }

        public struct Point
        {
            public int Row { get; init; }
            public int Col { get; init; }

            public Point(int row, int col)
            {
                Row = row;
                Col = col;
            }

            internal Point(Point point, int rowChange, int colChange)
            {
                Row = point.Row + rowChange;
                Col = point.Col + colChange;
            }
        }



        public Board()
        {
            Grid = new CellState[Size, Size];
            MovesMade = 0;
            MaxMoves = Size * Size;

            InitializeGrid();
        }

        public void MakeMove(Point point, CellState state)
        {
            if (!CheckInRange(point))
            {
                throw new IndexOutOfRangeException();
            }
            if (GetCell(point) != CellState.Empty)
            {
                throw new InvalidOperationException();
            }

            ChangeState(point, state);
            MovesMade++;
        }

        public WinnerStatus GetWinnerStatus()
        {
            return WinnerStatus.InProgress;

        }





        private WinnerStatus CellStateToWinnerStatus(CellState state)
        {
            switch(state)
            {
                case CellState.Player:
                    return WinnerStatus.Player;
                case CellState.Computer:
                    return WinnerStatus.Computer;
                default:
                    return WinnerStatus.InProgress;
            }
        }

        public char GetCharacter(Point point)
        {
            CellState cellState = GetCell(point);
            switch (cellState)
            {
                case CellState.Player:
                    return 'X';
                case CellState.Computer:
                    return 'O';
                default:
                    return ' ';
            }
        }


        public Point CalculateComputerMove()
        {
            Point point;

            point = MiniMax.GetBestMove(this).Item1;


            return point;
        }

        private void ChangeState(Point point, CellState state)
        {
            Grid[point.Row, point.Col] = state;
        }

        private CellState GetCell(Point point)
        {
            return Grid[point.Row, point.Col];
        }

        private void InitializeGrid()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    Grid[row, col] = CellState.Empty;
                }
            }
        }

        private bool CheckInRange(Point point)
        {
            if (point.Row < 0 ||
                point.Row >= Size)
            {
                return false;
            }
            if (point.Col < 0 ||
                point.Col >= Size)
            {
                return false;
            }

            return true;
        }

        internal void UndoMove(Point point)
        {
            if (!CheckInRange(point))
            {
                throw new IndexOutOfRangeException();
            }
            if (GetCell(point) == CellState.Empty)
            {
                throw new InvalidOperationException();
            }

            ChangeState(point, CellState.Empty);
            MovesMade--;
        }

        

    }
}
