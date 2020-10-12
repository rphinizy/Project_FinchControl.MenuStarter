using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control - Menu Starter
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu
    // Application Type: Console
    // Author: Phinizy, Robin
    // Dated Created: 10/1/2020
    // Last Modified: 10/11/2020
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
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.SetWindowSize(150, 40);
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            Console.BackgroundColor = ConsoleColor.Gray;
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
            for (int i = 0; i <10; i++)
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

        static void DisplayMixingItUp(Finch finchRobot)
        {
            DisplayScreenHeader("Mixing It Up");

            Console.WriteLine("The Finch robot will now Mix Things!");
            DisplayContinuePrompt();

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

            DisplayContinuePrompt();
            DisplayMenuPrompt("Talent Show Menu");
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
            double[] temperatures= null;

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
                        temperatures= DataRecorderManualGetData(numberOfDataPoints);
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
            double totalTempC=0;

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

        /// <summary>
        /// collect data from finch temperature sensors
        /// </summary>
        /// <param name="numberOfDataPoints"></param>
        /// <param name="dataPointFrequency"></param>
        /// <param name="myFinch"></param>
        /// <returns></returns>
        static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency, Finch myFinch)
        {
            double[] temperatures = new double[numberOfDataPoints];
            double totalTemp=0;
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

            do
            {
                DisplayScreenHeader("Alarm System");

                Console.WriteLine("This module is still under development.");
                Console.WriteLine();
                //
                // get user menu choice
                //

                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {

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

        #endregion

        #region USER PROGRAMMING

        /// <summary>
        /// *****************************************************************
        /// *                 USER PROGRAMMING MENU                         *
        /// *****************************************************************
        /// </summary>
        static void UserProgrammingDisplayMenuScreen(Finch myFinch)
        {
            Console.CursorVisible = true;

            bool quitUserProgramming = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("User Programming");

                Console.WriteLine("This module is still under development.");
                Console.WriteLine();
                //
                // get user menu choice
                //

                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {

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
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
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
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        /// Bonus Disconnect Music and Lights
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

        /// Bonus Connected Music
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
