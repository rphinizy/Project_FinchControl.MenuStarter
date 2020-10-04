using System;
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
    // Last Modified: 10/4/2020
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
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
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
            Console.CursorVisible = true;

            bool quitDataRecorder = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Data Recorder");

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

            // TODO test connection and provide user feedback - text, lights, sounds
            while (robotConnected == true)
            {
                //LIGHT FLASHING WHILE CONNECTING
                for (int i = 0; i < 2; i++)
                {
                    finchRobot.setLED(255, 0, 0);
                    finchRobot.wait(1000);
                    finchRobot.setLED(255, 150, 0);
                    finchRobot.wait(1000);
                    finchRobot.setLED(0, 255, 0);
                    finchRobot.wait(1000);
                }
                finchRobot.setLED(0, 255, 255);
                finchRobot.wait(1000);
                Console.Clear();
                Console.WriteLine("\tCongratulations! Your Finch Robot is now Connected");

                // Call Method to Play Music
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
            finchRobot.noteOn(217);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.noteOn(243);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.noteOn(289);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.noteOn(243);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.noteOn(365);
            finchRobot.wait(400);
            finchRobot.noteOff();

            finchRobot.noteOn(365);
            finchRobot.wait(200);
            finchRobot.noteOff();

            finchRobot.noteOn(325);
            finchRobot.wait(900);
            finchRobot.noteOff();
        }


        #endregion
    }
}
