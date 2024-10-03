using CST201MiniMaxDemo;
using Point = CST201MiniMaxDemo.Board.Point;
using CellState = CST201MiniMaxDemo.Board.CellState;
using WinnerStatus = CST201MiniMaxDemo.Board.WinnerStatus;

//---------------------------

Board board = new Board();

ControlLoop(board);


//---------------------------

static void ControlLoop(Board board)
{
    int row = -1, col = -1;
    CellState turn = CellState.Player;
    Point nextMove;
    WinnerStatus state;

    DisplayClass display = new DisplayClass();
    MiniMax.AddObserver(display);



    display.PrintBoard(board);

    while (true)
    {

        if (turn  == CellState.Player) 
        {
            while (row < 0 || row >= board.Size)
            {
                row = display.TakeIntegerInput("Enter Row");
            }
            while (col < 0 || col >= board.Size)
            {
                col = display.TakeIntegerInput("Enter Col");
            }

            nextMove = new Point(row, col);

            board.MakeMove(nextMove, turn);
            display.PrintBoard(board);

            row = -1;
            col = -1;
            turn = CellState.Computer;
        }
        else
        {
            nextMove = board.CalculateComputerMove();
            board.MakeMove(nextMove, turn);
            display.PrintBoard(board);
            turn = CellState.Player;
        }


        state = board.GetWinnerStatus();
        if (state != WinnerStatus.InProgress)
        {
            break;
        }
    }

    display.DisplayMessage($"{state} has won");
}

public class DisplayClass : Observer
{
    public void PrintBoard(Board board)
    {
        DisplayTopLines(board);
        NewLine();

        for (int row = 0; row < board.Size; row++)
        {
            for (int col = 0; col < board.Size; col++)
            {
                DisplayCell(board, new Point(row, col));
            }

            DisplayMessage("|");
            NewLine();
            DisplayTopLines(board);
            NewLine();
        }
    }

    public void DisplayCell(Board board, Point point)
    {
        DisplayMessage($"| {board.GetCharacter(point)} ");

    }

    public void DisplayTopLines(Board board)
    {
        string display = "";

        for (int col = 0; col < board.Size; col++)
        {
            display += "+---";
        }

        display += "+";

        DisplayMessage(display);
    }


    public void DisplayMessage(string message)
    {
        Console.Write(message);
    }

    public void NewLine()
    {
        Console.WriteLine();
    }

    public void DisplayInputString()
    {
        Console.Write(">>> ");
    }

    // Integer input helper method
    public int TakeIntegerInput(string message)
    {
        // Declare and Initialize
        string? input = null;
        int output = 0;
        bool isValid = false;

        while (!isValid)
        {
            DisplayMessage(message);
            NewLine();
            DisplayInputString();
            input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                DisplayMessage("ERROR: Input is Null or Empty");
                NewLine();
            }
            else
            {
                try
                {
                    output = int.Parse(input);
                    isValid = true;
                }
                catch (FormatException)
                {
                    DisplayMessage("ERROR: Enter a valid integer");
                    NewLine();
                }
                catch (OverflowException)
                {
                    DisplayMessage("ERROR: Overflow, please enter a 32-bit integer");
                    NewLine();
                }
                catch (Exception e)
                {
                    DisplayMessage($"There was an exception {e.Message} please try again");
                    NewLine();
                }
            }

        }

        return output;
    }

    public void Observe(Board board)
    {
        PrintBoard(board);
        ClearScreen();
    }

    private void ClearScreen()
    {
        Console.Clear();
    }
}

