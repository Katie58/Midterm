using System;
using System.Diagnostics;

namespace Midterm
{
    public enum State { hidden, flag, qmark, clicked }

    class Board
    {
        public bool isMine;
        private int rows;
        private int columns;
        private int numMines;
        private double minesPercent;
        private int[,] hiddenBoard;
        private State[,] displayBoard;
        public static int winCounter = 0;
        public static int loseCounter = 0;
        public static Stopwatch stopwatch = new Stopwatch();
        public int Rows
        {
            get
            {
                return rows;
            }
        }
        public int Columns
        {
            get
            {
                return columns;
            }
        }

        public Board()
        {
            rows = 10;
            columns = 10;
            minesPercent = .15;
            InitializeBoard();
        }

        public Board(int row, int column)
        {
            rows = row;
            columns = column;
            minesPercent = .15;
            InitializeBoard();
        }

        public Board(int row, int column, double minesPercent)
        {
            rows = row;
            columns = column;
            this.minesPercent = minesPercent;
            InitializeBoard();
        }

        //sending dimensions from UserInput class
        //////////////////////////////////sending dimensions from Menu class
        public static void BoardDimensions(int xy)/////////////////STEP 1 - MAKE BOARD
        {
            int row = xy;
            int column = xy;
            double minesPercent = .15;
            Board gameBoard = new Board(row, column, minesPercent);

            Console.Clear();
            UserInput.Playstate(gameBoard);
        }

        public static void BoardDimensions(int x, int y, int mines)
        {
            int row = x;
            int column = y;
            int minesTotal = mines;
            double minesPercent = mines / 100.0;
            Board gameBoard = new Board(row, column, minesPercent);
            Console.Clear();
            UserInput.Playstate(gameBoard);
        }

        private void InitializeBoard()////////////////////////////////STEP 2 - INITIALIZE BOARD ARRAYS
        {
            hiddenBoard = new int[rows, columns];
            displayBoard = new State[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    hiddenBoard[i, j] = 0;//initialize default to 0
                    displayBoard[i, j] = State.hidden;//initialize state to hidden
                }
            }
            MakeAllMines();
        }

        public void DisplayBoard()
        {
            int yAxisCounter = 0;
            int xAxisCounter = 0;
            char displayChar = ' ';

            Console.Write("    ");

            for (; yAxisCounter < columns; yAxisCounter++)//displays column numbers
            {
                Console.Write("{0, -3}", yAxisCounter + 1);//display each number with spacing of 3 to the left
            }
            Console.WriteLine();
            for (int i = 0; i < rows; i++)//display row numbers and tiles
            {
                Console.Write("{0, -4}", xAxisCounter + 1);//display each number with spacing of 4 to the left
                for (int j = 0; j < columns; j++)
                {
                    //yAxisCounter++;
                    switch (displayBoard[i, j])//display entire row
                    {
                        case State.clicked:
                            displayChar = (char)(hiddenBoard[i, j] + '0');
                            setColor(hiddenBoard[i, j]);
                            if (displayChar == '9')
                            {
                                Console.BackgroundColor = ConsoleColor.Magenta;
                                displayChar = '*'; //'*'
                            }
                            if (displayChar == '0')
                            {
                                displayChar = '-';
                            }
                            break;
                        case State.flag:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            displayChar = 'F'; //''
                            break;
                        case State.hidden:
                            //Console.ForegroundColor = ConsoleColor.Cyan;
                            displayChar = '#'; //''
                            break;
                        case State.qmark:
                            displayChar = '?';
                            break;
                    }
                    Console.Write(displayChar + "  ");
                    Console.ResetColor();
                }
                xAxisCounter++;
                Console.WriteLine();
            }
        }

        public void setColor(int colorCode)
        {
            switch (colorCode)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case 3:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case 4:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;

                case 5:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case 6:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;

                case 7:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;

                case 8:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
            }
        }

        // this method would work to reveal if the tile is a flag, qmark, number, bomb
        public bool RevealTile(int row, int column)//if click is selected
        {
            if (displayBoard[row, column] == State.hidden)
            {
                displayBoard[row, column] = State.clicked;
                if (hiddenBoard[row, column] == 0) // If there are no mines next to this one, reveal all the tiles next to it. 
                {
                    for (int i = row - 1; i <= row + 1; i++)
                    {
                        for (int j = column - 1; j <= column + 1; j++)
                        {
                            try
                            {
                                RevealTile(i, j);
                            }
                            catch (IndexOutOfRangeException)
                            {

                            }
                        }
                    }
                }
                else if (hiddenBoard[row, column] == 9)
                {
                    isMine = true;
                }
                return true;
            }
            return false;
        }

        private void MakeAllMines()////////////////////////////STEP 3 - MAKE MINES
        {
            Random r = new Random();
            // trys to create a mine and adds one to i each time a mine is made.
            int i;
            for (i = 0; i < rows * columns * minesPercent; i += MakesMine(r.Next() % rows, r.Next() % columns) ? 1 : 0) ;
            numMines = i;
        }

        public bool MakesMine(int row, int column)//validate mines
        {
            int mine = 9;
            if (hiddenBoard[row, column] != mine)
            {
                hiddenBoard[row, column] = mine;

                for (int i = row - 1; i <= row + 1; i++)//if mine, set 
                {
                    for (int j = column - 1; j <= column + 1; j++)
                    {
                        try
                        {
                            if (hiddenBoard[i, j] != mine)
                            {
                                hiddenBoard[i, j] += 1;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {

                        }
                    }
                }
                return true;
            }
            return false;
        }

        public bool WinsOrLoses()//return true if game over
        {
            if (isMine == true)//if inputCord[i] = 9 : true
            {
                Console.WriteLine("BOOOOOOOOM!");
                Console.WriteLine("Oh no, you hit a bomb!\n");
                DisplayHiddenBoard();  // <-- display hidden board
                Console.WriteLine("Try Again! :)");
                Console.ReadLine();
                loseCounter++;
                stopwatch.Reset();
                return true;
            }

            // number of clicked tiles should be equal to all tiles - mines.
            int numberClicked = 0;
            foreach (State tile in displayBoard)
            {
                if (tile == State.clicked)
                {
                    numberClicked++;
                }
            }

            if (displayBoard.Length - (numMines) == numberClicked)
            {
                DisplayHiddenBoard();  // <-- display hidden board

                UserInput.RecentScores(stopwatch.Elapsed.ToString(@"mm\:ss\.ff"), rows + "x" + columns);
                UserInput.RecentScoreReader();
                winCounter++;
                return true;
            }
            return false;
        }

        public void IsFlagged(int row, int column, ConsoleKey inputKey)//inputs from PlayState(), sets flags & Qmarks
        {
            if (displayBoard[row, column] == State.clicked)//if already set, display so
            {
                Console.WriteLine("This space is already clicked!");
            }
            else if (inputKey == ConsoleKey.F)//////////////////////////checks if input is flag
            {
                if (displayBoard[row, column] == State.flag)//if already flagged
                {
                    displayBoard[row, column] = State.hidden;//hide
                }
                else
                {
                    displayBoard[row, column] = State.flag;//if not yet flagged, flag
                }
            }
            else if (inputKey == ConsoleKey.Q)///////////////////////checks if input is question
            {
                if (displayBoard[row, column] == State.qmark)//if already marked
                {
                    displayBoard[row, column] = State.hidden;//hide
                }
                else
                {
                    displayBoard[row, column] = State.qmark;//if not yet marked, mark
                }
            }
        }

        public void DisplayHiddenBoard()
        {
            int yAxisCounter = 0;
            int xAxisCounter = 0;
            char temp = ' ';
            Console.Write("    ");
            for (; yAxisCounter < columns; yAxisCounter++)
            {
                Console.Write("{0, -3}", yAxisCounter + 1);
            }
            Console.WriteLine();
            for (int i = 0; i < rows; i++)
            {
                Console.Write("{0, -4}", xAxisCounter + 1);
                for (int j = 0; j < columns; j++)
                {

                    switch (displayBoard[i, j])
                    {
                        case State.clicked:
                            temp = (char)(hiddenBoard[i, j] + '0');
                            setColor(hiddenBoard[i, j]);
                            if (temp == '9')
                            {
                                Console.BackgroundColor = ConsoleColor.Magenta;
                                temp = '*'; //'*'
                            }
                            if (temp == '0')
                            {
                                temp = '-';
                            }
                            break;
                        case State.flag:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            temp = 'F'; //''
                            if (hiddenBoard[i, j] != 9)
                            {
                                Console.BackgroundColor = ConsoleColor.Cyan;
                            }
                            break;
                        case State.hidden:
                            temp = '#'; //''
                            if (hiddenBoard[i, j] == 9)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                temp = '*';
                            }
                            break;
                        case State.qmark:
                            temp = '?';
                            break;
                    }
                    Console.Write(temp + "  ");
                    Console.ResetColor();
                }
                xAxisCounter++;
                Console.WriteLine();
            }
            Console.WriteLine("\n\n");
            stopwatch.Stop();
        }
    }
}
