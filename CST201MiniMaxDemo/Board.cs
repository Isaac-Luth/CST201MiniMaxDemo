
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
            Point point;

            for (int row =  0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    // There is a center cell of every victory
                    // this algorithm will return on the center cell
                    point = new Point(row, col);
                    if (GetWinnerStatusCell(point) != WinnerStatus.InProgress)
                    {
                        return CellStateToWinnerStatus(GetCell(point));
                    }
                }
            }

            if (MovesMade == MaxMoves)
            {
                return WinnerStatus.Tie;
            }

            return WinnerStatus.InProgress;
        }

        private WinnerStatus GetWinnerStatusCell(Point point)
        {
            CellState cellState = GetCell(point);

            if (cellState == CellState.Empty)
            {
                return WinnerStatus.InProgress;
            }

            // Check Diagonals
            if (point.Row - 1 >= 0 &&
                point.Row + 1 < Size &&
                point.Col - 1 >= 0 &&
                point.Col + 1 < Size)
            {
                if (cellState == GetCell(new Point(point, -1, -1)) &&
                    cellState == GetCell(new Point(point, 1, 1)))
                {
                    return CellStateToWinnerStatus(GetCell(point));
                }
                if (cellState == GetCell(new Point(point, -1, 1)) &&
                    cellState == GetCell(new Point(point, 1, -1)))
                {
                    return CellStateToWinnerStatus(GetCell(point));
                }
            }

            // Check Row
            if (point.Row - 1 >= 0 &&
                point.Row + 1 < Size)
            {
                if (cellState == GetCell(new Point(point, -1, 0)) &&
                    cellState == GetCell(new Point(point, 1, 0)))
                {
                    return CellStateToWinnerStatus(GetCell(point));
                }
            }

            // Check Column
            if (point.Col - 1 >= 0 &&
                point.Col + 1 < Size)
            {
                if (cellState == GetCell(new Point(point, 0, -1)) &&
                    cellState == GetCell(new Point(point, 0, 1)))
                {
                    return CellStateToWinnerStatus(GetCell(point));
                }
            }

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
