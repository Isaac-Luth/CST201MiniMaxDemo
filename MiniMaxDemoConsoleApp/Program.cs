using CST201MiniMaxDemo;
using Point = CST201MiniMaxDemo.Board.Point;
using CellState = CST201MiniMaxDemo.Board.CellState;
using WinnerStatus = CST201MiniMaxDemo.Board.WinnerStatus;

//---------------------------

Board board = new Board();

ControlLoop(board);


//---------------------------

// This is the main control loop
static void ControlLoop(Board board)
{
    int row = -1, col = -1;
    CellState turn = CellState.Player;
    Point nextMove;
    WinnerStatus state;

    Utilities utilities = new Utilities();
    MiniMax.AddObserver(utilities);



    utilities.PrintBoard(board);

    while (true)
    {

        if (turn  == CellState.Player) 
        {
            while (row < 0 || row >= board.Size)
            {
                row = utilities.TakeIntegerInput("Enter Row");
            }
            while (col < 0 || col >= board.Size)
            {
                col = utilities.TakeIntegerInput("Enter Col");
            }

            nextMove = new Point(row, col);

            board.MakeMove(nextMove, turn);
            utilities.PrintBoard(board);

            row = -1;
            col = -1;
            turn = CellState.Computer;
        }
        else
        {
            var startTime = DateTime.Now.TimeOfDay;
            nextMove = board.CalculateComputerMove();
            var endTime = DateTime.Now.TimeOfDay;
            var timeElapsed = endTime - startTime;
            var timeString = timeElapsed.ToString();
            board.MakeMove(nextMove, turn);
            utilities.PrintBoard(board);
            utilities.DisplayMessage($"{timeString} has elapsed");
            utilities.NewLine();
            turn = CellState.Player;
        }


        state = board.GetWinnerStatus();
        if (state != WinnerStatus.InProgress)
        {
            break;
        }
    }

    utilities.DisplayMessage($"{state} has won");
}

public class Utilities : Observer
{

    /// <summary>
    /// Prints the board
    /// </summary>
    /// <param name="board"></param>
    public void PrintBoard(Board board)
    {
        string printString = "";

        printString += GetTopLines(board);
        printString += "\n";

        for (int row = 0; row < board.Size; row++)
        {
            for (int col = 0; col < board.Size; col++)
            {
                printString += GetCellDisplay(board, new Point(row, col));
            }

            printString += "|\n";
            printString += GetTopLines(board);
            printString += "\n";
        }

        DisplayMessage(printString);
    }

    /// <summary>
    /// returns the Cell character display
    /// </summary>
    /// <param name="board"></param>
    /// <param name="point"></param>
    /// <returns>string</returns>
    public string GetCellDisplay(Board board, Point point)
    {
        return $"| {board.GetCharacter(point)} ";

    }

    /// <summary>
    /// Returns the top lines
    /// </summary>
    /// <param name="board"></param>
    /// <returns>string</returns>
    public string GetTopLines(Board board)
    {
        string ret = "";

        for (int col = 0; col < board.Size; col++)
        {
            ret += "+---";
        }

        ret += "+";

        return ret;
    }


    /// <summary>
    /// Displays message
    /// </summary>
    /// <param name="message"></param>
    public void DisplayMessage(string message)
    {
        Console.Write(message);
    }

    /// <summary>
    /// Displays a newline
    /// </summary>
    public void NewLine()
    {
        Console.WriteLine();
    }

    /// <summary>
    /// Displays a unified input string
    /// </summary>
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


    /// <summary>
    /// This is the inherited method from Observer allowing this to observe minimax
    /// </summary>
    /// <param name="board"></param>
    public void Observe(Board board)
    {
        ClearScreen();
        PrintBoard(board);
        Thread.Sleep(1);

    }

    /// <summary>
    /// Clears the console
    /// </summary>
    private void ClearScreen()
    {
        Console.Clear();
    }
}

