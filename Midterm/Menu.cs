using System;
using System.Collections.Generic;
namespace Midterm
{
    class Menu
    {
        public static void Header()//Header clears each page and displays title
        {
            Console.Clear();
            int width = Console.WindowWidth;
            string head = "Welcome to MINEFIELD!";
            string count = $"Games Won: {Board.winCounter}  -  Games Lost: {Board.loseCounter}";
            string timer = Board.stopwatch.Elapsed.ToString(@"mm\:ss\.ff");

            Console.WriteLine(new string(' ', (width - head.Length) / 2) + head);
            Console.WriteLine(new string(' ', (width - count.Length) / 2) + count);
            Console.WriteLine(new string(' ', (width - timer.Length) / 2) + timer);
            Console.WriteLine(Environment.NewLine);
        }

        public static void StartMenu()
        {
            bool retry = true;//bool loops entire method until user chooses to exit method
            while (retry)
            {
                Header();
                int width = Console.WindowWidth;
                string select = "What would you like to do? (enter number)  ";

                List<KeyValuePair<string, Action>> menu = new List<KeyValuePair<string, Action>>();//List with KeyValuePairs for menu: string = display; Action = method call; to add: just menu.Add with no additional code changes
                menu.Add(new KeyValuePair<string, Action>("Play", () => MainMenu()));
                menu.Add(new KeyValuePair<string, Action>("Instructions", () => Instructions()));
                menu.Add(new KeyValuePair<string, Action>("Scores", () => UserInput.RecentScoreReader()));
                menu.Add(new KeyValuePair<string, Action>("Exit", () => Blank()));

                int menuCount = 0;//globally declared to use in multiple nests, changes dynamically based on menu items
                foreach (KeyValuePair<string, Action> item in menu)
                {
                    menuCount += 1;//counter to display selection options
                    Console.WriteLine(new string(' ', (width - 12) / 2) + menuCount + " - " + item.Key);
                }

                Console.Write($"\n{new string(' ', (width - select.Length) / 2)}" + select);
                int entry = 0;
                if (menuCount < 10)//set condition to equal 1 key press
                {
                    ConsoleKey key = Console.ReadKey().Key;//reads the key
                    entry = KeyToNum(key);//sends key to method that converts to int
                }
                else
                {
                    int.TryParse(Console.ReadLine(), out entry);//reads the line (user must press enter)
                }

                if (entry > 0 && entry <= menu.Count)//checks for valid entry
                {
                    menu[entry - 1].Value.Invoke();//invokes method
                    if (entry == menu.Count)//if last entry (Exit) was chosen, loop is set to false
                    {
                        retry = false;
                    }
                }
            }
        }

        public static void MainMenu()
        {
            bool retry = true;
            while (retry)
            {
                Header();
                int width = Console.WindowWidth;
                string select = "What would you like to do? (enter number)  ";

                List<KeyValuePair<string, Action>> menu = new List<KeyValuePair<string, Action>>();
                menu.Add(new KeyValuePair<string, Action>("Easy", () => Board.BoardDimensions(8)));
                menu.Add(new KeyValuePair<string, Action>("Intermediate", () => Board.BoardDimensions(16)));
                menu.Add(new KeyValuePair<string, Action>("Hard", () => Board.BoardDimensions(32)));
                menu.Add(new KeyValuePair<string, Action>("Custom", () => CustomXY()));
                menu.Add(new KeyValuePair<string, Action>("Return to Start Menu", () => Blank()));

                int menuCount = 0;
                foreach (KeyValuePair<string, Action> item in menu)
                {
                    menuCount += 1;
                    Console.WriteLine(new string(' ', (width - 12) / 2) + menuCount + " - " + item.Key);
                }

                Console.Write($"\n{new string(' ', (width - select.Length) / 2)}" + select);
                int entry = 0;
                if (menuCount < 10)
                {
                    ConsoleKey key = Console.ReadKey().Key;
                    entry = KeyToNum(key);
                }
                else
                {
                    int.TryParse(Console.ReadLine(), out entry);//if menu count is larger than 10 it will require enter
                }

                if (entry > 0 && entry <= menu.Count)
                {
                    menu[entry - 1].Value.Invoke();
                    retry = false;
                }
            }
        }

        public static void Blank()
        {
            //to remove possibly defined 'indirect recursion'
        }

        public static void Instructions()
        {
            Header();
            int width = Console.WindowWidth;
            string title = "TO PLAY:\n";
            string line1 = "1. Select difficulty level\n";
            string line2 = "2. Enter coordinates of tile\n";
            string line3 = "3. Press c to select";
            string line4 = "   Press f to flag  ";
            string line5 = "   Press q for uncertain\n";
            string line6 = "4. Continue until all squares are selected.";
            string end = "Press any key to continue...";

            Console.WriteLine(new string(' ', (width - title.Length) / 2) + title);
            Console.WriteLine(new string(' ', (width - line1.Length) / 2) + line1);
            Console.WriteLine(new string(' ', (width - line2.Length) / 2) + line2);
            Console.WriteLine(new string(' ', (width - line3.Length) / 2) + line3);
            Console.WriteLine(new string(' ', (width - line4.Length) / 2) + line4);
            Console.WriteLine(new string(' ', (width - line5.Length) / 2) + line5);
            Console.WriteLine(new string(' ', (width - line6.Length) / 2) + line6);
            Console.WriteLine(Environment.NewLine);
            Console.Write(new string(' ', (width - end.Length) / 2) + end);
            Console.ReadKey();
        }

        public static void CreditsStatic()
        {
            Header();
            int width = Console.WindowWidth;

            Console.WriteLine($"\n{new string(' ', (width - 7) / 2)}CREDITS\n\n{new string(' ', (width - 15) / 2)}" +
                $"DEV TEAM: BOOM!\n\n{new string(' ', (width - 15) / 2)}NICHOLAS LANDAU\n{new string(' ', (width - 15) / 2)}" +
                $"JOHNATHAN LEECH\n{new string(' ', (width - 13) / 2)}KATIE HARRELL\n{new string(' ', (width - 9) / 2)}TY CARRON\n\n");
            Console.WriteLine(Environment.NewLine);
        }

        public static void CustomXY()//sets custom board settings
        {
            Header();
            int width = Console.WindowWidth;
            //default values
            int[] input = new int[5] { 2, 0, 0, 0, 0 };//0=set, 1=input, 2=rows, 3=columns, 4=mines
            bool set = true;
            ConsoleKey key = ConsoleKey.Enter;
            int maxWindowWidth = Console.LargestWindowWidth;
            int maxWindowHeight = Console.LargestWindowHeight;
            int maxRow = maxWindowHeight - (maxWindowHeight % 10) - 10;
            int maxCol = (maxWindowWidth / 3) - ((maxWindowWidth / 3) % 10) - 10;
            int maxMine = 50;
            //universal variables
            int max = 0;
            int select = 0;
            string display = "";

            while (set)//loops to set row, column, and mines
            {
                switch (input[0])//display which value will be set
                {
                    case 2:
                        display = "Rows";
                        max = maxRow;
                        select = 2;
                        break;
                    case 3:
                        display = "Columns";
                        max = maxCol;
                        select = 3;
                        break;
                    case 4:
                        display = "Mines %";
                        max = maxMine;
                        select = 4;
                        break;
                }
                Console.Write($"\n{new string(' ', (width - 25) / 2)}Enter {display} (10 - {max}):  ");

                int num = 0;
                int tens = 0;
                int single = 0;
                for (int i = 0; i < 2; i++)//loops per digit
                {
                    key = Console.ReadKey().Key;//get input
                    if ((key >= ConsoleKey.D0 && key <= ConsoleKey.D9) || (key >= ConsoleKey.NumPad0 && key <= ConsoleKey.NumPad9))//validate key press
                    {
                        num = KeyToNum(key);//convert key to int
                    }
                    else
                    {
                        i = -1;//if either number is invalid, reset
                        if (input[0] == select) { Console.Write($" INVALID!\n{new string(' ', (width - 25) / 2)}Enter {display} (10 - {max}):  "); }
                        else { Console.WriteLine("ERROR - WTF did you do?"); }
                        continue;
                    }
                    //validate input
                    if (input[0] == select)//validate input for each digit
                    {
                        if (i == 0 && num > 0 && num <= max / 10)//if tens digit
                        {
                            tens = num * 10;
                        }
                        else if (i == 1 && num >= 0 && num <= 9)//if single digit
                        {
                            single = num;
                        }
                    }
                    else
                    {
                        Console.Write(" INVALID!");
                    }
                    input[1] = tens + single;//combines input value
                }

                if (input[0] == select)//validate double digit value
                {
                    if (input[1] > 9 && input[1] <= max)
                    {
                        input[input[0]] = input[1];//save value to array
                        input[0] += 1;//set next index
                        if (input[0] == 5)//break out after all values set
                        {
                            set = false;
                        }
                    }
                    else
                    {
                        Console.Write(" INVALID!");
                    }
                }
            }
            Board.BoardDimensions(input[2], input[3], input[4]);
            Console.WriteLine("Press any key to continue...");
        }

        public static bool YesNo()
        {
            bool valid = false;
            bool yes = true;

            while (!valid)
            {
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.Y)
                {
                    valid = true;
                    yes = true;
                }
                else if (key == ConsoleKey.N)
                {
                    valid = true;
                    yes = false;
                }
                else
                {
                    valid = false;
                    yes = true;
                }
            }
            return yes;
        }

        public static void Exit()
        {
            CreditsStatic();
            int width = Console.WindowWidth;
            string exit = "Goodbye! Press ESCAPE to Exit...";
            Console.Write($"\n{new string(' ', (width - exit.Length) / 2)}" + exit);
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                continue;
            }
        }

        public static int KeyToNum(ConsoleKey key)
        {
            if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) { return 1; }
            else if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) { return 2; }
            else if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3) { return 3; }
            else if (key == ConsoleKey.D4 || key == ConsoleKey.NumPad4) { return 4; }
            else if (key == ConsoleKey.D5 || key == ConsoleKey.NumPad5) { return 5; }
            else if (key == ConsoleKey.D6 || key == ConsoleKey.NumPad6) { return 6; }
            else if (key == ConsoleKey.D7 || key == ConsoleKey.NumPad7) { return 7; }
            else if (key == ConsoleKey.D8 || key == ConsoleKey.NumPad8) { return 8; }
            else if (key == ConsoleKey.D9 || key == ConsoleKey.NumPad9) { return 9; }
            else { return 0; }
        }
    }
}