using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using FinchAPI;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace Project_FinchControl
{

    /// <summary>
    /// User Commands
    /// </summary>

    public enum Command
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        GETTEMPERATURE,
        GETLIGHTVALUE,
        LIGHTS,
        SING,
        DANCEPARTY,
        DONE
    }

    // **************************************************
    //
    // Title: Finch Control - Menu Starter
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu
    // Application Type: Console
    // Author: Phinizy, Robin
    // Dated Created: 10/1/2020
    // Last Modified: 11/7/2020
    //
    // **************************************************


    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();

        }

        /// <summary>
        /// ***************************************
        /// *              Set Theme              *
        /// ***************************************
        /// </summary>
        static void SetTheme()
        {

            (int windowWidth, int windowHeight) windowSizeTuple;

            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeSettings;

            bool themeSet = false;
            bool windowSet = false;
            bool windowWidth = false;
            bool windowHeight = false;

            string userResponse;
            string correct;

            //
            // get saved data from text file to set the theme
            //
            themeSettings = ReadThemeTxtFile();
            windowSizeTuple = ReadWindowInfoData();

            Console.SetWindowSize(windowSizeTuple.windowWidth, windowSizeTuple.windowHeight);
            Console.ForegroundColor = themeSettings.foregroundColor;
            Console.BackgroundColor = themeSettings.backgroundColor;
            Console.Clear();


            DisplayScreenHeader("Set Application Theme");

            Console.WriteLine("\t Welcome to the Finch Control Application. Please Verify Settings");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"\t Current foreground color: {Console.ForegroundColor}");
            Console.WriteLine($"\t Current background color: {Console.BackgroundColor}");
            Console.WriteLine();
            Console.WriteLine($"\t Current window width: {Console.WindowWidth}");
            Console.WriteLine($"\t Current background color: {Console.WindowHeight}");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t Would you like to change the current theme settings? Please type [yes] or [no]");
            userResponse = Console.ReadLine().ToLower();

            if (userResponse == "yes")
            {
                do
                {
                    themeSettings.foregroundColor = GetConsoleSettingsFromUser("foreground");
                    themeSettings.backgroundColor = GetConsoleSettingsFromUser("background");

                    //
                    //set new theme
                    //
                    Console.ForegroundColor = themeSettings.foregroundColor;
                    Console.BackgroundColor = themeSettings.backgroundColor;
                    Console.Clear();
                    DisplayScreenHeader("Set Application Theme");
                    Console.WriteLine($"\t New forground color: {Console.ForegroundColor}");
                    Console.WriteLine($"\t New background color: {Console.BackgroundColor}");

                    Console.WriteLine();
                    Console.WriteLine("\t Are these settings correct?");

              
                    correct = Console.ReadLine().ToLower();

                    if (correct == "yes")
                    {
                        themeSet = true;
                    }

                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("\tplease try again");
                        themeSet = false;
                    }

                } while (!themeSet);
            }
            if (userResponse != "yes" && userResponse != "no")
            {
                Console.WriteLine("Please enter a valid response [yes] or [no]");
                Console.WriteLine();
                Console.WriteLine("press any key to continue");
                Console.ReadKey();
                SetTheme();
            }

            Console.WriteLine("\t Would you like to change the current window size? Please type [yes] or [no]");
            userResponse = Console.ReadLine().ToLower();

            if (userResponse == "yes")

            {
                do
                {
                    windowSizeTuple.windowWidth = GetWindowSettingsFromUser("width");
                    windowSizeTuple.windowHeight = GetWindowSettingsFromUser("height");

                    if (windowSizeTuple.windowWidth < 201 && windowSizeTuple.windowWidth > 51)
                    {
                        windowWidth = true;
                    }

                    if (windowSizeTuple.windowHeight < 61 && windowSizeTuple.windowHeight > 11)
                    {
                        windowHeight = true;
                    }
                    
                    if (windowWidth == true && windowHeight == true)
                    {
                        //
                        // set window size
                        //
                        Console.SetWindowSize(windowSizeTuple.windowWidth, windowSizeTuple.windowHeight);
                        Console.Clear();
                        DisplayScreenHeader("Set Application Window Size");
                        Console.WriteLine($"\t New Window Size: {Console.WindowWidth},{Console.WindowHeight}");

                        Console.WriteLine();
                        Console.WriteLine("\t Are these settings correct?");
                        correct = Console.ReadLine().ToLower();

                        if (correct == "yes")
                        {
                            windowSet = true;
                        }

                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("\tplease try again");
                            windowSet = false;
                        }
                    }
                    
                    else
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t Please check your window sizes are within limits. [min/max width: 50/200] , [min/max height: 10/60]");
                        Console.WriteLine();
                        Console.WriteLine($"\t You have enetered width: {windowSizeTuple.windowWidth} height: {windowSizeTuple.windowHeight}");
                        Console.ForegroundColor = themeSettings.foregroundColor;
                        windowSet = false;
                        Console.WriteLine();
                    }
                    
                } while (!windowSet);
            }

            if (userResponse != "yes" && userResponse != "no")
            {
                Console.WriteLine("Please enter a valid response [yes] or [no]");
                Console.WriteLine();
                Console.WriteLine("press any key to continue");
                Console.ReadKey();
                SetTheme();
            }

            WriteThemeTxtFile(
                themeSettings.foregroundColor, 
                themeSettings.backgroundColor, 
                windowSizeTuple.windowWidth, 
                windowSizeTuple.windowHeight);
         
            DisplayContinuePrompt();

        }
        /// <summary>
        /// ***************************************
        /// *         Read from TXTfile           *
        /// ***************************************
        /// </summary>
        static (ConsoleColor foregroundColor, ConsoleColor backgroundColor) ReadThemeTxtFile()
        {
            string dataPath = @"Data/theme.txt";
            string[] themeSettings;

            ConsoleColor foregroundColor;
            ConsoleColor backgroundColor;

            themeSettings = File.ReadAllLines(dataPath);

            Enum.TryParse(themeSettings[0], true, out foregroundColor);
            Enum.TryParse(themeSettings[1], true, out backgroundColor);

            return (foregroundColor, backgroundColor);

        }
        /// <summary>
        /// ***************************************
        /// *         Read from TXTfile           *
        /// ***************************************
        /// </summary>
        static (int windowWidth, int windowHeight) ReadWindowInfoData()
        {
            string dataPath = @"Data\theme.txt";

            string windowSizeText;
            string[] windowSizeArray;

            (int windowWidth, int windowHeight) windowSizeTuple;

            windowSizeText = File.ReadLines(dataPath).Skip(2).Take(1).First();

            windowSizeArray = windowSizeText.Split(',');

            int.TryParse(windowSizeArray[0], out windowSizeTuple.windowWidth);
            int.TryParse(windowSizeArray[1], out windowSizeTuple.windowHeight);
      

            return windowSizeTuple;
        }
        /// <summary>
        /// ***************************************
        /// *         Get Color Settings          *
        /// ***************************************
        /// </summary>
        static ConsoleColor GetConsoleSettingsFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;

            do
            {
                Console.Write($"\t Enter a value for the {property}: ");
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\t***** It appears your entry was not valid. Please try again. *****\n");
                }
                else
                {
                    validConsoleColor = true;
                }
            } while (!validConsoleColor);

            return consoleColor;

        }
        /// <summary>
        /// ***************************************
        /// *         Get Window Settings         *
        /// ***************************************
        /// </summary>
        static int GetWindowSettingsFromUser(string property)
        {
            bool validResponse = false;
            int userResponse;

            do
            {
                Console.Write($"\t Enter a value for the {property}: ");


                if (!int.TryParse(Console.ReadLine(), out userResponse))
                {
                    Console.WriteLine("\n\t***** It appears your entry was not valid. Please try again. *****\n");
                }
                else
                {
                    validResponse = true;
                }
            } while (!validResponse);

            return userResponse;
        }
        /// <summary>
        /// ***************************************
        /// *          Write to TXTfile           *
        /// ***************************************
        /// </summary>
        static void WriteThemeTxtFile(ConsoleColor foreground, ConsoleColor background, int windowWidth, int windowHeight)
        {
            string dataPath = @"Data/theme.txt";
            string windowInfoText;

            windowInfoText = windowWidth + "," + windowHeight;

            File.WriteAllText(dataPath, foreground.ToString() + "\n");
            File.AppendAllText(dataPath, background.ToString() + "\n");
            File.AppendAllText(dataPath, windowInfoText.ToString());
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.WriteLine("\ts) Settings");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":
                        DataRecorderDisplayMenuScreen(finchRobot);

                        break;

                    case "d":
                        AlarmSystemDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        UserProgrammingDisplayMenuScreen(finchRobot);
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    case "s":
                        SetTheme();
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region TALENT SHOW
        /// <summary>
        /// *****************************************************************
        /// *                     TALENT SHOW MENU                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch myFinch)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                Console.WriteLine("This module is still under development.");
                Console.WriteLine();
                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Dance");
                Console.WriteLine("\tc) Mixing it Up");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLightAndSound(myFinch);
                        break;

                    case "b":
                        DisplayDance(myFinch);
                        break;

                    case "c":
                        DisplayMixingItUp(myFinch);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }
        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayLightAndSound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will not show off its glowing talent!");
            DisplayContinuePrompt();

            for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
            {
                finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
                finchRobot.noteOn(lightSoundLevel * 100);
            }

            DisplayMenuPrompt("Talent Show Menu");
        }
        /// <summary>
        /// ************************************************
        /// *                DISPLAY DANCE                 *
        /// ************************************************ 
        /// </summary>
        static void DisplayDance(Finch finchRobot)
        {
            DisplayScreenHeader("Dance");

            Console.WriteLine("The Finch robot will now dance!");
            DisplayContinuePrompt();

            /// Where's My Key Dance
            Console.WriteLine("\tDance: Where's my Keys");
            for (int i = 0; i < 3; i++)
            {
                finchRobot.setMotors(200, 200);
                finchRobot.wait(2000);
                finchRobot.setMotors(0, 0);

                finchRobot.setMotors(-200, -200);
                finchRobot.wait(2000);
                finchRobot.setMotors(0, 0);

                finchRobot.setMotors(-200, 200);
                finchRobot.wait(1500);
                finchRobot.setMotors(0, 0);

                finchRobot.setMotors(200, -200);
                finchRobot.wait(1500);
                finchRobot.setMotors(0, 0);
            }
            /// The Junior High Dance
            Console.WriteLine("\tDance: The Junior High");
            for (int i = 0; i < 10; i++)
            {
                finchRobot.setMotors(50, 0);
                finchRobot.wait(1000);
                finchRobot.setMotors(0, 0);

                finchRobot.setMotors(0, 50);
                finchRobot.wait(1000);
                finchRobot.setMotors(0, 0);
            }

            DisplayContinuePrompt();
            DisplayMenuPrompt("Talent Show Menu");
        }
        /// <summary>
        /// ************************************************
        /// *                MIXING IT UP                  *
        /// ************************************************ 
        /// </summary>
        static void DisplayMixingItUp(Finch finchRobot)
        {
            DisplayScreenHeader("Mixing It Up");

            Console.WriteLine("The Finch robot will now Mix Things!");
            DisplayContinuePrompt();

            BabySharkDanceParty(finchRobot);

            DisplayContinuePrompt();
            DisplayMenuPrompt("Talent Show Menu");
        }
        /// <summary>
        /// ************************************************
        /// *                DANCE PARTY                   *
        /// ************************************************ 
        /// </summary>
        static void BabySharkDanceParty(Finch finchRobot)
        {
            for (int i = 0; i < 3; i++)
            {
                finchRobot.setMotors(50, 50);
                finchRobot.setLED(255, 0, 255);
                finchRobot.noteOn(587);  // BA
                finchRobot.wait(500);
                finchRobot.setMotors(0, 0);
                finchRobot.noteOff();

                finchRobot.setMotors(-50, -50);
                finchRobot.setLED(0, 0, 255);
                finchRobot.noteOn(659);  //BY
                finchRobot.wait(500);
                finchRobot.setMotors(0, 0);
                finchRobot.noteOff();

                finchRobot.setMotors(50, 50);
                finchRobot.setLED(255, 0, 255);
                finchRobot.noteOn(784);  //SHARK
                finchRobot.wait(350);
                finchRobot.noteOff();

                finchRobot.wait(10);

                finchRobot.setMotors(50, -50);
                finchRobot.setLED(0, 0, 255);
                finchRobot.noteOn(784);  // DOO
                finchRobot.wait(150);
                finchRobot.setMotors(0, 0);
                finchRobot.noteOff();

                finchRobot.setMotors(-50, 50);
                finchRobot.setLED(255, 0, 255);
                finchRobot.noteOn(784);  // DOO
                finchRobot.wait(150);
                finchRobot.setMotors(0, 0);
                finchRobot.noteOff();

                finchRobot.wait(50);

                finchRobot.setMotors(50, -50);
                finchRobot.setLED(0, 0, 255);
                finchRobot.noteOn(784);  // DOO
                finchRobot.wait(150);
                finchRobot.setMotors(0, 0);
                finchRobot.noteOff();

                finchRobot.setMotors(-50, 50);
                finchRobot.setLED(255, 0, 255);
                finchRobot.noteOn(784);  // DOO
                finchRobot.wait(150);
                finchRobot.setMotors(0, 0);
                finchRobot.noteOff();

                finchRobot.wait(50);

                finchRobot.setMotors(50, -50);
                finchRobot.setLED(0, 0, 255);
                finchRobot.noteOn(784);  // DOO
                finchRobot.wait(150);
                finchRobot.setMotors(0, 0);
                finchRobot.noteOff();

                finchRobot.setMotors(-50, 50);
                finchRobot.setLED(255, 0, 255);
                finchRobot.noteOn(784);  // DOO
                finchRobot.wait(150);
                finchRobot.setMotors(0, 0);
                finchRobot.noteOff();

                finchRobot.setLED(0, 0, 0);
                finchRobot.wait(50);
            }

            finchRobot.setMotors(-50, -50);
            finchRobot.setLED(0, 0, 255);
            finchRobot.noteOn(784);  //BA
            finchRobot.wait(300);
            finchRobot.noteOff();

            finchRobot.setMotors(50, 50);
            finchRobot.setLED(255, 0, 255);
            finchRobot.noteOn(784);  //BY
            finchRobot.wait(300);
            finchRobot.noteOff();

            finchRobot.setMotors(-50, -50);
            finchRobot.setLED(0, 0, 255);
            finchRobot.noteOn(740);  //SHARK
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.setMotors(0, 0);
        }


        #endregion

        #region DATA RECORDER

        /// <summary>
        /// *****************************************************************
        /// *                    DATA RECORDER MENU                         *
        /// *****************************************************************
        /// </summary>
        static void DataRecorderDisplayMenuScreen(Finch myFinch)
        {
            int numberOfDataPoints = 0;
            double dataPointFrequency = 0;
            double[] temperatures = null;

            Console.CursorVisible = true;

            bool quitDataRecorder = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Data Recorder Menu");
                Console.WriteLine();

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\te) Manual Data Entry");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        temperatures = DataRecorderDisplayGetData(numberOfDataPoints, dataPointFrequency, myFinch);
                        break;

                    case "d":
                        DataRecorderDisplayData(temperatures);
                        break;

                    case "e":
                        temperatures = DataRecorderManualGetData(numberOfDataPoints);
                        break;

                    case "q":
                        quitDataRecorder = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitDataRecorder);
        }


        /// <summary>
        /// display table of collected temperature date to the user
        /// </summary>
        /// <param name="temperatures"></param>
        static void DataRecorderDisplayData(double[] temperatures)
        {
            //
            //validate is array contains values
            if (temperatures != null)
            {
                DisplayScreenHeader("Show Data");

                Console.WriteLine("\t Data in Celsius");
                DataRecorderDisplayTable(temperatures);

                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("\t Data in Fahrenheit");
                DataRecorderDisplayDataFarenheightConversionTable(temperatures);

                DisplayContinuePrompt();
            }
            //
            //if array contains no values: display error message
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tERROR");
                Console.WriteLine("\tPlease return to the Data Recorder Menu and collect data before continuing.");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                DisplayContinuePrompt();
            }
        }

        /// <summary>
        /// create table from collected temperature data
        /// </summary>
        /// <param name="temperatures"></param>
        static void DataRecorderDisplayTable(double[] temperatures)

        {
            double totalTempC = 0;

            //
            //display table headers
            //
            Console.WriteLine(
                "Recording #".PadLeft(15) +
                "Temp".PadLeft(15)
                );
            Console.WriteLine(
                 "------------".PadLeft(15) +
                 "------------".PadLeft(15)
                 );

            //
            // display table data
            //
            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                (index + 1).ToString().PadLeft(15) +
                temperatures[index].ToString("n2").PadLeft(15)
                );

                totalTempC += temperatures[index];
            }
            Console.WriteLine("------------".PadLeft(30));
            Console.WriteLine("Average TEMP".PadLeft(30));
            Console.WriteLine("" + (totalTempC / (temperatures.Length)).ToString("n4").PadLeft(30));
        }

        /// <summary>
        /// Convert sensor data to Fahrenheit and display in a table
        /// </summary>
        /// <param name="temperatures"></param>
        static void DataRecorderDisplayDataFarenheightConversionTable(double[] temperatures)
        {
            double tempFahrenheit;
            double totalTempF = 0;
            //
            //display table headers
            //
            Console.WriteLine(
                "Recording #".PadLeft(15) +
                "Temp".PadLeft(15)
                );
            Console.WriteLine(
                 "------------".PadLeft(15) +
                 "------------".PadLeft(15)
                 );
            //
            // display table data
            //
            for (int index = 0; index < temperatures.Length; index++)
            {
                tempFahrenheit = ((9 * temperatures[index]) + (32 * 5)) / 5;
                Console.WriteLine(
                (index + 1).ToString().PadLeft(15) +
                tempFahrenheit.ToString("n2").PadLeft(15)
                );
                totalTempF += tempFahrenheit;
            }
            Console.WriteLine("------------".PadLeft(30));
            Console.WriteLine("Average TEMP".PadLeft(30));
            Console.WriteLine("" + (totalTempF / (temperatures.Length)).ToString("n4").PadLeft(30));

        }

        ///// <summary>
        /// collect data from finch temperature sensors
        /// </summary>
        /// <param name="numberOfDataPoints"></param>
        /// <param name="dataPointFrequency"></param>
        /// <param name="myFinch"></param>
        /// <returns></returns>
        static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency, Finch myFinch)
        {
            double[] temperatures = new double[numberOfDataPoints];
            double totalTemp = 0;
            DisplayScreenHeader("Get Data");

            Console.WriteLine($"\tNumber of Data Points: {numberOfDataPoints}");
            Console.WriteLine($"\tData Point Frequency: {dataPointFrequency}");
            Console.WriteLine();
            Console.WriteLine("\tThe Finch Robot is ready to begin recording the temperature data");
            DisplayContinuePrompt();

            //
            //check to see if user entered data points and frequency
            if (numberOfDataPoints > 0 && dataPointFrequency > 0)
            {
                for (int index = 0; index < numberOfDataPoints; index++)
                {
                    temperatures[index] = myFinch.getTemperature();
                    Console.WriteLine($"Reading {index + 1}: {temperatures[index].ToString("n2")}");
                    int waitInSeconds = (int)dataPointFrequency * 1000;
                    myFinch.wait(waitInSeconds);
                    totalTemp += temperatures[index];
                }

                Array.Sort(temperatures);

                //
                //ask user to verify finch connection for temp reading 0
                if (totalTemp == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("\tERROR");
                    Console.WriteLine("\tPlease return to the Menu and make sure Finch is connected.");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    DisplayContinuePrompt();
                    return temperatures;
                }

                else
                {
                    DisplayContinuePrompt();
                    return temperatures;
                }
            }
            //
            // display error if no values for date poionts and frequency given.
            else
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tERROR");
                Console.WriteLine();
                Console.WriteLine("\tPlease return to the Menu and enter a valid number for Data Points and or Data Point Frequency");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                DisplayContinuePrompt();
                return temperatures;
            }
        }

        /// <summary>
        /// allows user to manually enter temperature data
        /// </summary>
        /// <param name="numberOfDataPoints"></param>
        /// <returns></returns>
        static double[] DataRecorderManualGetData(int numberOfDataPoints)
        {
            double[] temperatures = new double[numberOfDataPoints];

            DisplayScreenHeader("Manual Data Entry");
            Console.WriteLine();
            Console.WriteLine($"\tNumber of Data Points required: {numberOfDataPoints}");
            Console.WriteLine();

            //
            // validate user response for data points > than 0
            if (numberOfDataPoints > 0)
            {
                Console.WriteLine();
                Console.WriteLine("\tBegin recording manual temperature data");
                DisplayContinuePrompt();

                for (int index = 0; index < numberOfDataPoints; index++)
                {
                    Console.WriteLine($"Enter Data Point Number{index + 1}");
                    double.TryParse(Console.ReadLine(), out temperatures[index]);
                }
                Array.Sort(temperatures);
                DisplayContinuePrompt();
                return temperatures;
            }
            //
            // display error message if 0 data points
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tERROR");
                Console.WriteLine("\tPlease return to the main menu and enter a valid number for Data Points");
                DisplayContinuePrompt();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                return temperatures;
            }

        }

        /// <summary>
        /// get data point frequency from the user
        /// </summary>
        /// <returns> data point frequency </returns>
        static double DataRecorderDisplayGetDataPointFrequency()
        {
            double dataPointFrequency;
            string userResponse;

            DisplayScreenHeader("Data Point Frequency");

        askUserFrequency:
            Console.WriteLine("\tPlease enter the required number of frequency of data points");

            //
            // Validate User Input
            //
            if (!double.TryParse(Console.ReadLine(), out dataPointFrequency))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tYour response is not valid.");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine();
                goto askUserFrequency;
            }

            else
            {
                Console.WriteLine();
                Console.WriteLine($"\tYou have entered the number {dataPointFrequency} for your frequency of data points. Is this correct?");
                userResponse = Console.ReadLine().ToLower();

                if (userResponse == "yes" || userResponse == "y")
                {
                    DisplayContinuePrompt();
                    return dataPointFrequency;
                }
                else
                {
                    goto askUserFrequency;
                }
            }
        }

        /// <summary>
        /// get number of data points from the user
        /// </summary>
        /// <returns> number of data points </returns>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;
            string userResponse;

            DisplayScreenHeader("Number Of Data Points");

        askUserNumDataPoints:
            Console.WriteLine("\tPlease enter the number of data points required");

            //
            // Validate User Input
            //
            if (!int.TryParse(Console.ReadLine(), out numberOfDataPoints))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tYour response is not valid.");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine();
                goto askUserNumDataPoints;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"\tYou have entered the number {numberOfDataPoints} for your required data points. Is this correct?");
                userResponse = Console.ReadLine().ToLower();

                if (userResponse == "yes" || userResponse == "y")
                {
                    DisplayContinuePrompt();
                    return numberOfDataPoints;
                }
                else
                {
                    goto askUserNumDataPoints;
                }
            }
        }

        #endregion

        #region ALARM SYSTEM

        /// <summary>
        /// *****************************************************************
        /// *                    ALARM SYSTEM MENU                          *
        /// *****************************************************************
        /// </summary>
        static void AlarmSystemDisplayMenuScreen(Finch myFinch)
        {
            Console.CursorVisible = true;

            bool quitAlarmSystem = false;
            string menuChoice;

            string sensorsToMonitor = "";
            string rangeType = "";
            int minMaxThresholdValue = 0;
            int timeToMonitor = 0;
            int minMaxThresholdValueTemp = 0;

            do
            {
                DisplayScreenHeader("Alarm System");
                Console.WriteLine();

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors to Monitor");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Minimum/Maximim Threshold Value");
                Console.WriteLine("\td) Set Time to Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sensorsToMonitor = LightAlarmDisplaySetSensorsToMonitor();
                        break;

                    case "b":
                        rangeType = LightAlarmDisplaySetRangeType();
                        break;

                    case "c":
                        //
                        //if user selected a light monitoring mode: get light threshold value
                        if (sensorsToMonitor == "left" || sensorsToMonitor == "right" || sensorsToMonitor == "both" || sensorsToMonitor == "all")
                        {
                            minMaxThresholdValue = LightAlarmDisplaySetThresholdValue(rangeType, myFinch, sensorsToMonitor, minMaxThresholdValueTemp);
                        }
                        //
                        //if user selected a temperatue monitoring mode: get temp threshold value
                        if (sensorsToMonitor == "temp" || sensorsToMonitor == "all")
                        {
                            minMaxThresholdValueTemp = TempAlarmDisplaySetThresholdValue(rangeType, myFinch, minMaxThresholdValueTemp);
                        }
                        //
                        //send user back to menu to select sensors to monitor
                        if (sensorsToMonitor == "")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine();
                            Console.WriteLine("\tPlease Select a sensor to monitor before continuing");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            DisplayMenuPrompt("Alarm System");
                        }
                        break;

                    case "d":
                        timeToMonitor = LightAlarmDisplaySetTimetoMonitor();
                        break;

                    case "e":
                        LightAlarmSetAlarm(myFinch, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor, minMaxThresholdValueTemp);
                        break;

                    case "q":
                        quitAlarmSystem = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitAlarmSystem);
        }
        /// <summary>
        /// ************************************************
        /// *                  SET SENSORS                 *
        /// ************************************************  
        /// </summary>
        /// <returns></returns>
        static string LightAlarmDisplaySetSensorsToMonitor()
        {
            string sensorsToMonitor;

            DisplayScreenHeader("\tSensors to Monitor");

            Console.WriteLine("\tSensors to Monitor [left, right, both, temp, all]:");
            sensorsToMonitor = Console.ReadLine().ToLower();

            //
            //validate user input for correct value
            if (sensorsToMonitor != "left" && sensorsToMonitor != "right" && sensorsToMonitor != "both" && sensorsToMonitor != "temp" && sensorsToMonitor != "all")
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\tYour response");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write(" " + sensorsToMonitor + " ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("is not valid");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("\n");
                Console.WriteLine("\tPress any key to continue");
                Console.ReadKey();
                LightAlarmDisplaySetSensorsToMonitor();
            }
            //
            //echo user input value
            else
            {
                Console.WriteLine("\tSensors to Monitor: " + sensorsToMonitor);
                DisplayMenuPrompt("Alarm System");
            }

            return sensorsToMonitor;

        }
        /// <summary>
        /// ************************************************
        /// *                 SET RANGE TYPE               *
        /// ************************************************  
        /// </summary>
        /// <returns></returns>
        static string LightAlarmDisplaySetRangeType()
        {
            string rangeType;
            bool firstLoop = true;

            DisplayScreenHeader("\tRange Type");
            Console.WriteLine();

            //
            //
            //validate user input for correct value
            do
            {
                //
                //mesage for first loop only. 
                if (firstLoop == true)
                {
                    Console.WriteLine();
                    Console.WriteLine("\t Enter Range Type [minimum, maximum]:");
                    rangeType = Console.ReadLine().ToLower();
                    firstLoop = false;
                }

                //
                // message for invalid user response
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\tPlease Enter a Valid Response");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    rangeType = Console.ReadLine().ToLower();
                }

            } while (rangeType != "minimum" && rangeType != "maximum");

            //
            //echo user input value
            Console.WriteLine("\tRange Type: " + rangeType);
            DisplayMenuPrompt("Alarm System");

            return rangeType;
        }
        /// <summary>
        /// ************************************************
        /// *                SET THRESHOLD TEMP           *
        /// ************************************************ 
        /// </summary>
        /// <returns></returns>
        static int TempAlarmDisplaySetThresholdValue(string rangeType, Finch myFinch, int minMaxThresholdValueTemp)
        {

            bool firstLoop = true;

            DisplayScreenHeader("Minimum/Maximum Temp Threshold Value");
            Console.WriteLine();
            Console.WriteLine($"\tTemperature sensor ambient value: {myFinch.getTemperature()}");

            //
            //
            //validate user input for correct value
            do
            {
                //
                //mesage for first loop only. 
                if (firstLoop == true)
                {
                    Console.WriteLine($"\tEnter the {rangeType} temperature threshold value:");
                    firstLoop = false;
                }

                //
                // message for invalid user response
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\tPlease Enter a Valid Response");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }

            } while (!int.TryParse(Console.ReadLine(), out minMaxThresholdValueTemp));

            //
            //echo user input value
            Console.WriteLine("\tTemperature threshold value: " + minMaxThresholdValueTemp);
            DisplayMenuPrompt("Alarm System");

            return minMaxThresholdValueTemp;
        }
        /// <summary>
        /// ************************************************
        /// *              SET THRESHOLD LIGHT             *
        /// ************************************************ 
        /// </summary>
        /// <returns></returns>
        static int LightAlarmDisplaySetThresholdValue(string rangeType, Finch myFinch, string sensorsToMonitor, int minMaxThresholdValueTemp)
        {
            int minMaxThresholdValue = 0;
            bool firstLoop = true;

            DisplayScreenHeader("Minimum/Maximum Threshold Value");


            if (sensorsToMonitor == "left" || sensorsToMonitor == "right" || sensorsToMonitor == "both" || sensorsToMonitor == "all")
            {
                Console.WriteLine($"\tLeft light sensor ambient value: {myFinch.getLeftLightSensor()}");
                Console.WriteLine($"\tRight light sensor ambient value: {myFinch.getRightLightSensor()}");
                Console.WriteLine();
                //
                //
                //validate user input for correct value
                do
                {
                    //
                    //mesage for first loop only. 
                    if (firstLoop == true)
                    {
                        Console.WriteLine($"Enter the {rangeType} light sensor threshold value:");
                        firstLoop = false;
                    }

                    //
                    // message for invalid user response
                    else
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\tPlease Enter a Valid Response");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }

                } while (!int.TryParse(Console.ReadLine(), out minMaxThresholdValue));

                //
                //echo user input value
                Console.WriteLine("\t" + rangeType + " Threshold Value: " + minMaxThresholdValue);

            }
            //
            // prevent menu prompt from running twice if user chooses the temperature options
            if (sensorsToMonitor == "left" || sensorsToMonitor == "right" || sensorsToMonitor == "both")
            {
                DisplayMenuPrompt("Alarm System");
            }

            return minMaxThresholdValue;
        }
        /// <summary>
        /// ************************************************
        /// *             SET TIME TO MONITOR              *
        /// ************************************************  
        /// </summary>
        /// <returns></returns>
        static int LightAlarmDisplaySetTimetoMonitor()
        {
            int timeToMonitor;

            DisplayScreenHeader("Time to Monitor");

            Console.WriteLine($"\tEnter the time to monitor (in seconds):");
            //
            //validate user input for correct value
            if (!int.TryParse(Console.ReadLine(), out timeToMonitor))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\tYour response is not valid");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("\n");
                Console.WriteLine("\tPress any key to continue");
                Console.ReadKey();
                LightAlarmDisplaySetTimetoMonitor();
            }
            //
            //echo user input value
            else
            {
                Console.WriteLine("\tTime to Monitor: " + timeToMonitor);
                DisplayMenuPrompt("Alarm System");
            }

            return timeToMonitor;
        }
        /// <summary>
        /// ************************************************
        /// *                RUN ALARM                     *
        /// ************************************************ 
        /// </summary>
        static void LightAlarmRunAlarm(Finch myFinch,
            string sensorsToMonitor,
            string rangeType,
            int minMaxThresholdValue,
            int timeToMonitor,
            int minMaxThresholdValueTemp)
        {
            //
            //run alarm program with user parameters

            bool thresholdExceeded = false;
            bool tempThresholdExceeded = false;
            int secondsElasped = 0;
            int currentLightSensorValue = 0;
            double currentTempSensorValue = 0;

            while (secondsElasped < timeToMonitor && !thresholdExceeded || !tempThresholdExceeded)
            {
                switch (sensorsToMonitor)
                {
                    case "left":
                        currentLightSensorValue = myFinch.getLeftLightSensor();
                        break;

                    case "right":
                        currentLightSensorValue = myFinch.getRightLightSensor();
                        break;

                    case "both":
                        currentLightSensorValue = (myFinch.getRightLightSensor() + myFinch.getLeftLightSensor()) / 2;
                        break;

                    case "temp":
                        currentTempSensorValue = myFinch.getTemperature();
                        break;

                    case "all":
                        currentLightSensorValue = (myFinch.getRightLightSensor() + myFinch.getLeftLightSensor()) / 2;
                        currentTempSensorValue = myFinch.getTemperature();

                        break;
                }
                switch (rangeType)
                {
                    case "minimum":
                        if (currentLightSensorValue < minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        if (currentTempSensorValue < minMaxThresholdValueTemp)
                        {
                            tempThresholdExceeded = true;
                        }
                        break;

                    case "maximim":
                        if (currentLightSensorValue > minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        if (currentTempSensorValue > minMaxThresholdValueTemp)
                        {
                            tempThresholdExceeded = true;
                        }
                        break;

                }

                if (sensorsToMonitor == "left" || sensorsToMonitor == "right" || sensorsToMonitor == "both")
                {
                    Console.SetCursorPosition(10, 10);
                    Console.WriteLine("Time Elasped: " + secondsElasped);
                    Console.SetCursorPosition(10, 11);
                    Console.WriteLine("Light Value: " + currentLightSensorValue);
                    myFinch.wait(1000);
                    secondsElasped++;
                }

                if (sensorsToMonitor == "all")
                {
                    Console.SetCursorPosition(10, 10);
                    Console.WriteLine("Time Elasped: " + secondsElasped);
                    Console.SetCursorPosition(10, 11);
                    Console.WriteLine("Light Value: " + currentLightSensorValue);
                    Console.SetCursorPosition(10, 12);
                    Console.WriteLine("Temp Value: " + currentTempSensorValue);
                    myFinch.wait(1000);
                    secondsElasped++;
                }

                if (sensorsToMonitor == "temp")
                {
                    Console.SetCursorPosition(10, 10);
                    Console.WriteLine("Time Elasped: " + secondsElasped);
                    Console.SetCursorPosition(10, 11);
                    Console.WriteLine("Temp Value: " + currentTempSensorValue);
                    myFinch.wait(1000);
                    secondsElasped++;
                }
            }

            if (thresholdExceeded == true || tempThresholdExceeded == true)
            {
                //display sensor reading that triggered alarm.

                if (thresholdExceeded == true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(10, 14);
                    Console.WriteLine("\tTripped Light Value: " + currentLightSensorValue);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                if (tempThresholdExceeded == true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(10, 15);
                    Console.WriteLine("\tTripped Temp Value: " + currentTempSensorValue);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }

                for (int i = 0; i < 5; i++)
                {

                    Console.SetCursorPosition(0, 17);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\t<*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*>");
                    Console.WriteLine("\t\t*                                                          *");
                    Console.WriteLine("\t\t*                - -                   - -                 *");
                    Console.WriteLine("\t\t*              -  !  -     ALARM     -  !  -               *");
                    Console.WriteLine("\t\t*                - -                   - -                 *");
                    Console.WriteLine("\t\t*                                                          *");
                    Console.WriteLine("\t\t<*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*>");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine();
                    Console.WriteLine();

                    //
                    // display which sensor exceeded threshold to trip alarm.
                    if (thresholdExceeded == true)
                    {
                        Console.WriteLine($"\tThe {rangeType}  light threshold value of {minMaxThresholdValue} was exceeded");
                        Console.WriteLine();
                    }
                    if (tempThresholdExceeded == true)
                    {
                        Console.WriteLine($"\tThe {rangeType}  temperature threshold value of {minMaxThresholdValueTemp} was exceeded");
                        Console.WriteLine();
                    }

                    Console.Beep(900, 700);
                    Console.SetCursorPosition(0, 17);
                    Console.WriteLine("\t\t<*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*>");
                    Console.WriteLine("\t\t*                                                          *");
                    Console.WriteLine("\t\t*                - -                   - -                 *");
                    Console.WriteLine("\t\t*              -  !  -    WARNING    -  !  -               *");
                    Console.WriteLine("\t\t*                - -                   - -                 *");
                    Console.WriteLine("\t\t*                                                          *");
                    Console.WriteLine("\t\t<*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*><*>");
                    Console.Beep(300, 700);

                }
            }
            else
            {
                if (sensorsToMonitor == "left" || sensorsToMonitor == "right" || sensorsToMonitor == "both" || sensorsToMonitor == "all")
                {
                    Console.WriteLine();
                    Console.WriteLine($"\tThe {rangeType} light threshold value of {minMaxThresholdValue} was not exceeded");
                    Console.WriteLine();
                }

                if (sensorsToMonitor == "temp" || sensorsToMonitor == "all")
                {
                    Console.WriteLine();
                    Console.WriteLine($"\tThe {rangeType} temperature threshold value of {minMaxThresholdValueTemp} was not exceeded");
                    Console.WriteLine();
                }

                Console.WriteLine("\n");
                Console.WriteLine("\n");
                DisplayMenuPrompt("Alarm System");
            }
        }
        /// <summary>
        /// ************************************************
        /// *                SET ALARM                     *
        /// ************************************************ 
        /// </summary>
        static void LightAlarmSetAlarm(Finch myFinch,
            string sensorsToMonitor,
            string rangeType,
            int minMaxThresholdValue,
            int timeToMonitor,
            int minMaxThresholdValueTemp
           )
        {
            bool allValuesTemp = false;
            bool allValuesAll = false;
            bool allValuesLight = false;


            DisplayScreenHeader("Set Alarm");

            Console.WriteLine($"Sensor to monitor: {sensorsToMonitor}");
            Console.WriteLine("Range Type: {0}", rangeType);
            Console.WriteLine("Min/Max threshold values: Light(" + minMaxThresholdValue + "), Temperature(" + minMaxThresholdValueTemp + ")");
            Console.WriteLine($"Time to monitor: {timeToMonitor}");
            Console.WriteLine();

            Console.WriteLine("Press any key to begin monitoring.");
            Console.ReadKey();
            Console.WriteLine();


            // **********************************************************
            //           USER SENSORS TO MONITOR LOGIC BLOCK            *
            // **********************************************************
            //validate the user has entered all required settings for the alarm.
            //
            //
            //
            //Check for all variables required for "TEMP"
            if (sensorsToMonitor == "temp" && rangeType != "" && timeToMonitor != default && minMaxThresholdValueTemp != default)
            {
                allValuesTemp = true;
                LightAlarmRunAlarm(myFinch, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor, minMaxThresholdValueTemp);
            }
            //
            //
            //Check for all variables required for "ALL"
            if (sensorsToMonitor == "all" && rangeType != "" && timeToMonitor != default && minMaxThresholdValueTemp != default && minMaxThresholdValue != default)
            {
                allValuesAll = true;
                LightAlarmRunAlarm(myFinch, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor, minMaxThresholdValueTemp);
            }
            //
            //
            //Check for all variables required for "LEFT" , "RIGHT", or "BOTH"
            if (sensorsToMonitor == "left" || sensorsToMonitor == "right" || sensorsToMonitor == "both")
            {
                if (rangeType != "" && timeToMonitor != default && minMaxThresholdValue != default)
                {
                    allValuesLight = true;
                    LightAlarmRunAlarm(myFinch, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor, minMaxThresholdValueTemp);
                }
            }

            //
            //
            // If all user input values missing
            if (sensorsToMonitor == "" && rangeType == "" && minMaxThresholdValue == default && timeToMonitor == default && minMaxThresholdValueTemp == default)
            {
                Console.WriteLine();
                Console.WriteLine("\tPlease enter ALL required values to set your alarm");
                Console.WriteLine();
                Console.SetCursorPosition(0, 13);
                Console.WriteLine("\t\tSelected Sensors: " + sensorsToMonitor);
                Console.WriteLine("\t\tRange Type: " + rangeType);
                Console.WriteLine("\t\tSensor Threshold: Light(" + minMaxThresholdValue + "), Temperature(" + minMaxThresholdValueTemp + ")");
                Console.WriteLine("\t\tTime to Monitor: " + timeToMonitor);
                Console.WriteLine();

                //
                //change color of text to red for missing values
                if (sensorsToMonitor == "")
                {
                    Console.SetCursorPosition(0, 13);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\tSelected Sensors: " + sensorsToMonitor);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }

                if (rangeType == "")
                {
                    Console.SetCursorPosition(0, 14);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\tRange Type: " + rangeType);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }

                if (minMaxThresholdValue == default && minMaxThresholdValueTemp == default)
                {
                    Console.SetCursorPosition(0, 15);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\tSensor Threshold: Light(" + minMaxThresholdValue + "), Temperature(" + minMaxThresholdValueTemp + ")");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                if (timeToMonitor == 0)
                {
                    Console.SetCursorPosition(0, 16);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\tTime to Monitor: " + timeToMonitor);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }

                Console.WriteLine();
                DisplayMenuPrompt("Alarm System");

            }

            //
            //
            //if user seleced "TEMP" and is missing input values
            if (sensorsToMonitor == "temp" && allValuesTemp == false)
            {

                if (rangeType == "" || timeToMonitor == default || minMaxThresholdValueTemp == default)
                {

                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter ALL required values to set your alarm");
                    Console.WriteLine();
                    Console.SetCursorPosition(0, 13);
                    Console.WriteLine("\t\tSelected Sensors: " + sensorsToMonitor);
                    Console.WriteLine("\t\tRange Type: " + rangeType);
                    Console.WriteLine("\t\tSensor Threshold: Temperature(" + minMaxThresholdValueTemp + ")");
                    Console.WriteLine("\t\tTime to Monitor: " + timeToMonitor);
                    Console.WriteLine();

                    //
                    //change color of text to red for missing values
                    if (sensorsToMonitor == "")
                    {
                        Console.SetCursorPosition(0, 13);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\tSelected Sensors: " + sensorsToMonitor);
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }

                    if (rangeType == "")
                    {
                        Console.SetCursorPosition(0, 14);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\tRange Type: " + rangeType);
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }

                    if (minMaxThresholdValueTemp == default)
                    {
                        Console.SetCursorPosition(0, 15);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\tSensor Threshold: Temperature(" + minMaxThresholdValueTemp + ")");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    if (timeToMonitor == 0)
                    {
                        Console.SetCursorPosition(0, 16);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\tTime to Monitor: " + timeToMonitor);
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }

                    Console.WriteLine();
                    DisplayMenuPrompt("Alarm System");

                }

            }

            //
            //
            //if user seleced "ALL" and is missing input values
            if (sensorsToMonitor == "all" && allValuesAll == false)
            {

                if (rangeType == "" || timeToMonitor == default || minMaxThresholdValueTemp == default || minMaxThresholdValue == default)
                {

                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter ALL required values to set your alarm");
                    Console.WriteLine();
                    Console.SetCursorPosition(0, 13);
                    Console.WriteLine("\t\tSelected Sensors: " + sensorsToMonitor);
                    Console.WriteLine("\t\tRange Type: " + rangeType);
                    Console.WriteLine("\t\tSensor Threshold: Light(" + minMaxThresholdValue + "), Temperature(" + minMaxThresholdValueTemp + ")");
                    Console.WriteLine("\t\tTime to Monitor: " + timeToMonitor);
                    Console.WriteLine();

                    //
                    //change color of text to red for missing values
                    if (sensorsToMonitor == "")
                    {
                        Console.SetCursorPosition(0, 13);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\tSelected Sensors: " + sensorsToMonitor);
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }

                    if (rangeType == "")
                    {
                        Console.SetCursorPosition(0, 14);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\tRange Type: " + rangeType);
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }

                    if (minMaxThresholdValue == default || minMaxThresholdValueTemp == default)
                    {
                        Console.SetCursorPosition(0, 15);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\tSensor Threshold: Light(" + minMaxThresholdValue + "), Temperature(" + minMaxThresholdValueTemp + ")");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    if (timeToMonitor == 0)
                    {
                        Console.SetCursorPosition(0, 16);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\tTime to Monitor: " + timeToMonitor);
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }

                    Console.WriteLine();
                    DisplayMenuPrompt("Alarm System");

                }

                if (sensorsToMonitor == "left" || sensorsToMonitor == "right" || sensorsToMonitor == "both" && allValuesLight == false)
                {

                    if (rangeType == "" || timeToMonitor == default || minMaxThresholdValue == default)
                    {

                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter ALL required values to set your alarm");
                        Console.WriteLine();
                        Console.SetCursorPosition(0, 13);
                        Console.WriteLine("\t\tSelected Sensors: " + sensorsToMonitor);
                        Console.WriteLine("\t\tRange Type: " + rangeType);
                        Console.WriteLine("\t\tSensor Threshold: Light(" + minMaxThresholdValue + ")");
                        Console.WriteLine("\t\tTime to Monitor: " + timeToMonitor);
                        Console.WriteLine();

                        //
                        //change color of text to red for missing values
                        if (sensorsToMonitor == "")
                        {
                            Console.SetCursorPosition(0, 13);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\tSelected Sensors: " + sensorsToMonitor);
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                        }

                        if (rangeType == "")
                        {
                            Console.SetCursorPosition(0, 14);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\tRange Type: " + rangeType);
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                        }

                        if (minMaxThresholdValue == default)
                        {
                            Console.SetCursorPosition(0, 15);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\tSensor Threshold: Light(" + minMaxThresholdValue + ")");
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                        }
                        if (timeToMonitor == 0)
                        {
                            Console.SetCursorPosition(0, 16);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\tTime to Monitor: " + timeToMonitor);
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                        }

                        Console.WriteLine();
                        DisplayMenuPrompt("Alarm System");

                    }
                }

                Console.WriteLine("\n");
                Console.WriteLine("\n");
                DisplayMenuPrompt("Alarm System");

            }
        }
        #endregion

        #region USER PROGRAMMING

        /// <summary>
        /// ***************************************************************
        /// *                 USER PROGRAMMING MENU                       *
        /// ***************************************************************
        /// </summary>
        static void UserProgrammingDisplayMenuScreen(Finch myFinch)
        {
            Console.CursorVisible = true;

            bool quitUserProgramming = false;
            string menuChoice;

            //
            // tuple to store all three command parameters
            //

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            List<Command> commands = new List<Command>();

            do
            {
                DisplayScreenHeader("User Programming");

                Console.WriteLine();
                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandParameters();
                        break;

                    case "b":
                        UserProgrammingDisplayGetFinchCommands(commands);
                        break;

                    case "c":
                        UserProgammingDisplayFinchCommands(commands);
                        break;

                    case "d":
                        UserProgrammingDisplayExecuteFinchCommands(myFinch, commands, commandParameters);
                        break;

                    case "q":
                        quitUserProgramming = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitUserProgramming);
        }
        /// <summary>
        /// ************************************************
        /// *                EXECUTE COMMANDS              *
        /// ************************************************ 
        /// </summary>
        /// <param name="myFinch"></param>
        /// <param name="commands"></param>
        /// <param name="commandParameters"></param>
        static void UserProgrammingDisplayExecuteFinchCommands(Finch finchRobot, List<Command> commands, (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters)
        {
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            int waitMilliSeconds = (int)(commandParameters.waitSeconds * 1000);
            string commandFeedback = "";
            const int TURNING_MOTOR_SPEED = 100;

            DisplayScreenHeader("Execute Finch Commands");

            Console.WriteLine("\t The Finch robot is ready to execute the list of commands.");
            DisplayContinuePrompt();


            foreach (Command command in commands)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 10);

                switch (command)
                {
                    case Command.NONE:
                        break;

                    case Command.MOVEFORWARD:
                        commandFeedback = Command.MOVEFORWARD.ToString();
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        finchRobot.wait(1000);

                        break;

                    case Command.MOVEBACKWARD:
                        commandFeedback = Command.MOVEBACKWARD.ToString();
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);
                        finchRobot.wait(1000);

                        break;

                    case Command.STOPMOTORS:
                        commandFeedback = Command.STOPMOTORS.ToString();
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        finchRobot.setMotors(0, 0);
                        break;

                    case Command.WAIT:
                        commandFeedback = Command.WAIT.ToString();
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        finchRobot.wait(waitMilliSeconds);
                        break;

                    case Command.TURNRIGHT:
                        commandFeedback = Command.TURNRIGHT.ToString();
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        finchRobot.setMotors(TURNING_MOTOR_SPEED, -TURNING_MOTOR_SPEED);
                        finchRobot.wait(1500);
                        break;

                    case Command.TURNLEFT:
                        commandFeedback = Command.TURNRIGHT.ToString();
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        finchRobot.setMotors(-TURNING_MOTOR_SPEED, TURNING_MOTOR_SPEED);
                        finchRobot.wait(1500);
                        break;

                    case Command.LEDON:
                        commandFeedback = Command.LEDON.ToString();
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        finchRobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        finchRobot.wait(1500);
                        break;

                    case Command.LEDOFF:
                        commandFeedback = Command.LEDOFF.ToString();
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(1500);
                        break;

                    case Command.GETTEMPERATURE:
                        commandFeedback = $"Temperature: {finchRobot.getTemperature():n2}";
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        finchRobot.wait(2500);
                        break;

                    case Command.GETLIGHTVALUE:
                        commandFeedback = $"Light Value: {(finchRobot.getRightLightSensor() + finchRobot.getLeftLightSensor()) / 2:n2}";
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        finchRobot.wait(2500);
                        break;

                    case Command.LIGHTS:
                        commandFeedback = Command.LIGHTS.ToString();
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        for (int i = 0; i < 2; i++)
                        {
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(100);
                            finchRobot.setLED(255, 150, 0);
                            finchRobot.wait(100);
                            finchRobot.setLED(0, 255, 0);
                            finchRobot.wait(100);
                        }
                        break;

                    case Command.SING:
                        commandFeedback = Command.SING.ToString();
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        PlayRickRollHello(finchRobot);
                        break;

                    case Command.DANCEPARTY:
                        commandFeedback = Command.DANCEPARTY.ToString();
                        Console.Write($"\tExecuting finch command:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{commandFeedback}                ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        BabySharkDanceParty(finchRobot);
                        break;

                    case Command.DONE:
                        commandFeedback = Command.DONE.ToString();
                        break;

                    default:
                        break;

                }

            }
            //
            //adding this to prevent "runnaway finch syndrome"
            finchRobot.setMotors(0, 0);
            Console.CursorVisible = true;
            DisplayMenuPrompt("UserProgramming");

        }
        /// <summary>
        /// ************************************************
        /// *                DISPLAY COMMANDS              *
        /// ************************************************
        /// </summary>
        /// <param name="commands"> list of commands</param>
        static void UserProgammingDisplayFinchCommands(List<Command> commands)
        {
            DisplayScreenHeader("Finch Robot Commands");

            UserProgammingEchoFinchCommands(commands);

            DisplayMenuPrompt("User Programming");
        }

        /// <summary>
        /// ************************************************
        /// *               ECHO COMMANDS                  *
        /// ************************************************
        /// </summary>
        /// <param name="commands"></param>
        static void UserProgammingEchoFinchCommands(List<Command> commands)
        {
            int itemNumber = 1;
            //
            //method will display stored user commands

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t You have entered the following command sequence:");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\t*****************");
            Console.WriteLine();
            foreach (Command command in commands)
            {
                Console.WriteLine($"\t{itemNumber}: {command}");
                itemNumber++;
            }
            Console.WriteLine();
            Console.WriteLine("\t*****************");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
        }
        /// <summary>
        /// ************************************************
        /// *                 GET COMMANDS                 *
        /// ************************************************
        /// </summary>
        /// <param name="commands"></param>
        static void UserProgrammingDisplayGetFinchCommands(List<Command> commands)
        {
            Command command = Command.NONE;

            int userInput;
            string input;

            DisplayScreenHeader("Finch Robot Commands");

            //
            // display a list of available commands
            int commandCount = 1;
            Console.WriteLine("\t List of Available Commands");
            Console.WriteLine();
            Console.WriteLine("\t********************************************************************");
            Console.WriteLine("\t-                                                                  -");
            Console.Write("\t-");
            foreach (string commandName in Enum.GetNames(typeof(Command)))
            {
                Console.Write($"- {commandName.ToLower()}  -");
                if (commandCount % 5 == 0) Console.Write("-\n\t");
                commandCount++;
            }
            Console.WriteLine("\n\t-                                                                  -");
            Console.WriteLine("\t********************************************************************");
            Console.WriteLine();

            while (command != Command.DONE)
            {
                Console.WriteLine("\t Enter Command:");

                //store user uput for validations
                input = Console.ReadLine();
                //
                //check to see if input is part of the ENUM list
                if (Enum.TryParse(input.ToUpper(), out command))
                {
                    //
                    //check to see if user entered a number
                    if (!int.TryParse(input, out userInput))
                    {
                        commands.Add(command);
                    }
                    //
                    //display error message if user entered a number. No mumbers allowed!
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\t*******************************************");
                        Console.WriteLine("\t\tPlease enter a command from the list above.");
                        Console.WriteLine("\t\t*******************************************");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine();
                    }

                }
                //
                //
                //display error message if user entry is not a valid command
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\t*******************************************");
                    Console.WriteLine("\t\tPlease enter a command from the list above.");
                    Console.WriteLine("\t\t*******************************************");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine();
                }
            }
            //
            //echo commands
            UserProgammingEchoFinchCommands(commands);

            DisplayMenuPrompt("User Programming");
        }
        /// <summary>
        /// ************************************************
        /// *                GET PARAMETERS               *
        /// ************************************************ 
        /// </summary>
        /// <returns></returns>
        static (int motorSpeed, int ledBrightness, double waitSeconds) UserProgrammingDisplayGetCommandParameters()
        {
            DisplayScreenHeader("Command Parameters");

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;


            bool needMotorSpeed = true;
            bool needLedBrightness = true;
            bool needWaitSeconds = true;

            //
            //
            //ask user for MOTOR SPEED and validate response
            while (commandParameters.motorSpeed > 255 || commandParameters.motorSpeed <= 0)
            {
                do
                {
                    //
                    //mesage for first loop only. 
                    if (needMotorSpeed == true)
                    {
                        Console.WriteLine("\t Enter MotorSpeed [1 - 255]:");
                        needMotorSpeed = false;
                    }

                    //
                    // message for invalid user response
                    else
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\tPlease Enter a value between[1] and [255]");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }

                } while (!int.TryParse(Console.ReadLine(), out commandParameters.motorSpeed));
            }
            //
            //
            //ask user for LED BRIGHTNESS and validate response
            while (commandParameters.ledBrightness > 255 || commandParameters.ledBrightness <= 0)
            {
                do
                {
                    //
                    //mesage for first loop only. 
                    if (needLedBrightness == true)
                    {
                        Console.WriteLine("\t Enter LED Brigtness [1 - 255]:");
                        needLedBrightness = false;
                    }

                    //
                    // message for invalid user response
                    else
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\tPlease Enter a value between[1] and [255]");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }

                } while (!int.TryParse(Console.ReadLine(), out commandParameters.ledBrightness) && commandParameters.ledBrightness < 256 && commandParameters.ledBrightness >= 0);
            }
            //
            //
            // ask user for WAIT in seconds and validate response
            while (commandParameters.waitSeconds > 10 || commandParameters.waitSeconds <= 0)
            {
                do
                {
                    //
                    //mesage for first loop only. 
                    if (needWaitSeconds == true)
                    {
                        Console.WriteLine("\t Enter Wait time in Seconds [1 - 10]:");
                        needWaitSeconds = false;
                    }

                    //
                    // message for invalid user response
                    else
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\tPlease Enter a value between[1] and [10]");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }

                } while (!double.TryParse(Console.ReadLine(), out commandParameters.waitSeconds) && commandParameters.waitSeconds < 11 && commandParameters.waitSeconds >= 0);
            }
            //
            //echo user input value
            Console.WriteLine();
            Console.WriteLine($"\t Motor speed: {commandParameters.motorSpeed}");
            Console.WriteLine($"\t LED Brightness: { commandParameters.ledBrightness}");
            Console.WriteLine($"\t Wait command duration:{commandParameters.waitSeconds}");

            DisplayMenuPrompt("User Programming");

            return commandParameters;
        }
        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");


            DisplayContinuePrompt();

            PlayRickRollGoodBye(finchRobot);

            Console.WriteLine("\tThe Finch robot is now disconnected.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            // test connection and provide user feedback - text, lights, sounds
            while (robotConnected == true)
            {
                //LIGHT FLASHING WHILE CONNECTING
                for (int i = 0; i < 2; i++)
                {
                    finchRobot.setLED(255, 0, 0);
                    finchRobot.wait(100);
                    finchRobot.setLED(255, 150, 0);
                    finchRobot.wait(100);
                    finchRobot.setLED(0, 255, 0);
                    finchRobot.wait(100);
                }
                finchRobot.setLED(0, 255, 255);
                finchRobot.wait(1000);
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("\tCongratulations! Your Finch Robot is now Connected");

                PlayRickRollHello(finchRobot);

                DisplayContinuePrompt();
                return robotConnected;

            }
            Console.Clear();
            Console.WriteLine("\tThere appears to be a problem with your finch. Please Try again. You may have to restart the program");
            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *************************************************
        /// *                CLOSING SCREEN                 *
        /// *************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// ************************************************
        /// *                CONTINUE PROMPT               *
        /// ************************************************ 
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// ************************************************
        /// *                 MENU PROMPT                  *
        /// ************************************************ 
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// ************************************************
        /// *                SCREEN HEADER                 *
        /// ************************************************ 
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        /// <summary>
        /// ************************************************
        /// *              RICK ROLL GOODBYE               *
        /// ************************************************ 
        /// <param name="finchRobot"></param>
        static void PlayRickRollGoodBye(Finch finchRobot)
        {
            finchRobot.setLED(255, 0, 0);

            finchRobot.noteOn(217);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.setLED(0, 255, 0);

            finchRobot.noteOn(243);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.setLED(0, 0, 255);

            finchRobot.noteOn(289);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.setLED(255, 0, 0);

            finchRobot.noteOn(243);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.setLED(0, 255, 0);

            finchRobot.noteOn(289);
            finchRobot.wait(300);
            finchRobot.noteOff();

            finchRobot.setLED(0, 0, 255);

            finchRobot.noteOn(325);
            finchRobot.wait(300);
            finchRobot.noteOff();

            finchRobot.setLED(255, 0, 0);

            finchRobot.noteOn(273);
            finchRobot.wait(300);
            finchRobot.noteOff();

            finchRobot.setLED(0, 255, 0);

            finchRobot.noteOn(217);
            finchRobot.wait(300);
            finchRobot.noteOff();

            finchRobot.setLED(0, 0, 255);

            finchRobot.noteOn(217);
            finchRobot.wait(300);
            finchRobot.noteOff();

            finchRobot.setLED(255, 0, 0);

            finchRobot.noteOn(325);
            finchRobot.wait(400);
            finchRobot.noteOff();

            finchRobot.setLED(0, 255, 0);

            finchRobot.noteOn(289);
            finchRobot.wait(900);
            finchRobot.noteOff();

            finchRobot.setLED(0, 0, 255);

            finchRobot.setLED(0, 0, 0);
            finchRobot.disConnect();
        }

        /// <summary>
        /// ************************************************
        /// *                RICK ROLL HELLO               *
        /// ************************************************ 
        /// <param name="finchRobot"></param>
        static void PlayRickRollHello(Finch finchRobot)
        {
            finchRobot.setLED(255, 0, 0);
            finchRobot.noteOn(217);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.setLED(0, 155, 255);
            finchRobot.noteOn(243);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.setLED(255, 0, 255);
            finchRobot.noteOn(289);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.setLED(255, 0, 0);
            finchRobot.noteOn(243);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.setLED(255, 0, 0);
            finchRobot.noteOn(365);
            finchRobot.wait(400);
            finchRobot.noteOff();

            finchRobot.setLED(0, 155, 255);
            finchRobot.noteOn(365);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.setLED(255, 0, 255);
            finchRobot.noteOn(325);
            finchRobot.wait(900);
            finchRobot.noteOff();
        }


        #endregion
    }
}

