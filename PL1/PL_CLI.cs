using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using BL;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;

namespace PL1
{
    public class PL_CLI : IPL1
    {
        // atributes
        private IBL[] cats;
        private string[] catsNames;
        public static string[][] inputsInfo = new string[13][];
        public static Dictionary<string, int> fieldsInfo = new Dictionary<string, int>();
        public static Dictionary<string, string> fieldsNames = new Dictionary<string, string>();

        // static constructor
        static PL_CLI()
        {
            // ID - 9 digits
            inputsInfo[0] = new string[2] { "^[0-9]{9}$", "exactly 9 digits (0-9)" };

            // only letters and possibly more than one word
            // firstName, lastName, Product - name, Department - name
            inputsInfo[1] = new string[2] { "^[A-Za-z]{2,}(( )[A-Za-z]{2,})*$", "only letters (A-Z) or (a-z)" };

            // day - 2-digit number between 01-31
            inputsInfo[2] = new string[2] { "^(0[1-9]|[12][0-9]|3[01])$", "a 2-digit number (01-31)" };

            // month - 2-digit number between 01-12
            inputsInfo[3] = new string[2] { "^(0[1-9]|1[012])$", "a 2-digit number (01-12)" };

            // year - 4-digit number starts with 19 or 20
            inputsInfo[4] = new string[2] { "^((19|20)[0-9][0-9])$", "a 4-digit number (19**) or (20**)" };

            // gender - M,m,F,f
            inputsInfo[5] = new string[2] { "^(M|m|F|f){1}$", "M for male or F for female" };

            // Number - unlimited digits
            inputsInfo[6] = new string[2] { "^[0-9]+$", "only digits (0-9)" };

            // one char of a,b,c
            // product type
            inputsInfo[7] = new string[2] { "^((a|b|c){1})$", "one of the types: a, b, c" };

            // one char of 1,2,3
            // product status
            inputsInfo[8] = new string[2] { "^((1|2|3){1})$", "one of the following:\n\t1 - Empty\n\t2 - LowQuantity\n\t3 - InStock" };

            // transaction type - one char of R,r,P,p
            inputsInfo[9] = new string[2] { "^((R|r|P|p){1})$", "one of the following:\n\tR - Return\n\tP - Purchase" };

            // at least 6 characters of letters and digits
            // username, password
            inputsInfo[10] = new string[2] { "^[A-Za-z0-9]{6,}$", "at least 6 characters.\nOnly letters (A-Z) or (a-z) and digits (0-9) are allowed" };

            // one char of 1,2,3
            // Payment method
            inputsInfo[11] = new string[2] { "^((1|2|3){1})$", "one of the following:\n\t1 - Cash\n\t2 - Check\n\t3 - visa" };

            // one char of 1,2,3
            // currentDate, dateOfBirth
            inputsInfo[12] = new string[2] { "^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/((19|20)[0-9][0-9])$", "a date in dd/mm/yyyy format\n\tDay:   a 2-digit number (01-31)\n\tMonth: a 2-digit number (01-12)\n\tYear:  a 4-digit number (19**) or (20**)" };

            //////////////////////////////////////////////////

            fieldsInfo.Add("Id", 0);

            fieldsInfo.Add("FirstName", 1);
            fieldsInfo.Add("LastName", 1);
            fieldsInfo.Add("Name", 1);

            fieldsInfo.Add("Day", 2); // only for add
            fieldsInfo.Add("Month", 3); // only for add
            fieldsInfo.Add("Year", 4); // only for add
            fieldsInfo.Add("DateOfBirth", 12);

            fieldsInfo.Add("Gender", 5);

            fieldsInfo.Add("MemberID", 6);
            fieldsInfo.Add("TransactionID", 6);
            fieldsInfo.Add("ProductID", 6);
            fieldsInfo.Add("DepartmentID", 6);
            fieldsInfo.Add("Price", 6);
            fieldsInfo.Add("StockCount", 6);
            fieldsInfo.Add("Location", 6);
            fieldsInfo.Add("DepID", 6);
            fieldsInfo.Add("Salary", 6);
            fieldsInfo.Add("SupervisiorID", 6);

            fieldsInfo.Add("Type", 7);

            fieldsInfo.Add("InStock", 8);

            fieldsInfo.Add("Is_a_Return", 9);

            fieldsInfo.Add("UserName", 10);
            fieldsInfo.Add("Password", 10);

            fieldsInfo.Add("Payment", 11);

            fieldsInfo.Add("CurrentDate", 12);

            //////////////////////////////////////////////////

            fieldsNames.Add("Id", "ID number");
            fieldsNames.Add("FirstName", "First name");
            fieldsNames.Add("LastName", "Last name");
            fieldsNames.Add("Name", "Name");
            fieldsNames.Add("DateOfBirth", "Date of birth");
            fieldsNames.Add("Gender", "Gender");
            fieldsNames.Add("MemberID", "Member ID");
            fieldsNames.Add("TransactionID", "Transaction ID");
            fieldsNames.Add("ProductID", "Product ID");
            fieldsNames.Add("DepartmentID", "Department ID");
            fieldsNames.Add("Price", "Price");
            fieldsNames.Add("StockCount", "Stock");
            fieldsNames.Add("Location", "Department ID");
            fieldsNames.Add("DepID", "Department ID");
            fieldsNames.Add("Salary", "Salary");
            fieldsNames.Add("SupervisiorID", "Supervisor ID");
            fieldsNames.Add("Type", "Product type");
            fieldsNames.Add("InStock", "Product status");
            fieldsNames.Add("Is_a_Return", "Transaction type");
            fieldsNames.Add("UserName", "User name");
            fieldsNames.Add("Password", "Password");
            fieldsNames.Add("Payment", "Payment method");
            fieldsNames.Add("CurrentDate", "Date");
            fieldsNames.Add("TransactionHistory", "Transaction History");
            fieldsNames.Add("Receipt", "Receipt");
        }

        // constructors
        public PL_CLI(IBL itsClubMemberBL, IBL itsDepartmentBL, IBL itsEmployeeBL, IBL itsProductBL, IBL itsTransactionBL, IBL itsUserBL)
        {
            cats = new IBL[7];
            cats[0] = null;
            cats[1] = itsClubMemberBL;
            cats[2] = itsDepartmentBL;
            cats[3] = itsEmployeeBL;
            cats[4] = itsProductBL;
            cats[5] = itsTransactionBL;
            cats[6] = itsUserBL;
            catsNames = new string[7] { "", "Club member", "Department", "Employee", "Product", "Transaction", "User" };
        }

        // methods

        private string ReceiveCmd()
        {
            return Console.ReadLine();
        }


        private void PressEnter()
        {
            WriteColor("\nPress ENTER to continue", true, ConsoleColor.Blue);
            Console.ReadLine();
            Console.Clear(); //clear the screen
        }


        private void GoBack(int categoryNum)
        {
            Console.Clear(); //clear the screen
            ActionMenu(categoryNum); // Action menu
        }


        public void ShowError(Exception e)
        {
            WriteColor("\n----- Error -----", true, ConsoleColor.Red);
            Console.WriteLine();
            WriteColor(e.Message, true, ConsoleColor.Red);
            Console.WriteLine();
        }


        private void WriteColor(string text, bool newLine, ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.ForegroundColor = ConsoleColor.White;
            if (newLine)
                Console.WriteLine(text);
            else
                Console.Write(text);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }


        private int internalMenu(string[] options, string instruction, string error)
        {
            // Show the menu options
            int index = 0;
            foreach (string opt in options)
            {
                if (opt != null)
                {
                    if (opt.Equals("Go back") || opt.Equals("Exit"))
                        Console.WriteLine();
                    Console.WriteLine("\t{0}. {1}", index, opt);
                }
                index++;
            }
            return inputMenuLoop(options.Length - 1, instruction, error); // Return the choice of the user;
        }


        private int inputMenuLoop(int max, string instruction, string error)
        {
            int choice = inputMenuSingle(instruction);
            while (choice == -1 || choice == -2 || choice == 0 || choice > max) // checks validity of the input
            {
                // pointing the error for the user
                if (choice == -1)
                    WriteColor("Invalid input! Please type only digits (0-9)\n", true, ConsoleColor.Red);
                else if (choice == -2)
                    WriteColor("Invalid input! Too big number\n", true, ConsoleColor.Red);
                else
                    WriteColor("Invalid input! Please select " + error + "\n", true, ConsoleColor.Red);
                choice = inputMenuSingle(instruction);
            }
            return choice;
        }


        private int inputMenuSingle(string instruction)
        {
            string cmd;
            int choice = -1;
            // instruction for the user
            if (instruction != "")
                Console.Write(instruction + ": ");
            // getting input from the user
            cmd = ReceiveCmd();
            if (Regex.IsMatch(cmd, @"^[0-9]+$"))
            {
                try
                {
                    choice = int.Parse(cmd);
                }
                catch (OverflowException)
                {
                    choice = -2;
                }
            }
            return choice;
        }


        // Start of program
        public void Run()
        {
            bool login = false; ;
            while (true)
            {
                // Check username and password
                WriteColor("\n--- User Log in ---", true, ConsoleColor.DarkGreen);
                Console.WriteLine("\nPlease enter your Username and Password for logging in the system");

                // information array about the input fields
                string[][] logIn = new string[2][];
                logIn[0] = new string[3] { "User name", "10", "" };
                logIn[1] = new string[3] { "Password", "10", "" };

                // getting inputs from the user
                logIn = getInputsFromUser(logIn);
                string username = logIn[0][2];
                string password = logIn[1][2];

                // check username and password validity
                try
                {
                    login = ((User_BL)cats[6]).isItValidUser(new User(username, password));
                }
                catch (System.Data.DataException e)
                {
                    Console.WriteLine(e.Message);
                }
                login = ((User_BL)cats[6]).isItValidUser(new User(username, password));


                if (login)
                {
                    Console.Clear();
                    MainMenu(); // Main menu
                    break;
                }
                else
                    WriteColor("\nIncorrect username and password\nAccess denied! Please try again...", true, ConsoleColor.Red);
            }
        }


        /***************************
         * Menu methods
         ****************************/

        private void MainMenu()
        {
            WriteColor("\n--- Main menu ---\n", true, ConsoleColor.DarkGreen);
            WriteColor("Please select a data entity to manage:\n", true, ConsoleColor.DarkMagenta);
            string[] options = new string[] { null, "Club member", "Department", "Employee", "Product", "Transaction", "User", "Exit" };
            int categoryNum = internalMenu(options, "", "a data entity (1-6) or Exit (7)");
            if (categoryNum == 7)
                return; //quit the program
            Console.Clear(); // clear the screen
            ActionMenu(categoryNum); // Action menu
        }


        private void ActionMenu(int categoryNum)
        {
            WriteColor("\n--- " + catsNames[categoryNum] + "s management ---\n", true, ConsoleColor.DarkGreen);
            WriteColor("Please select an action:\n", true, ConsoleColor.DarkMagenta);
            int numOfOptions = 5;
            string[] options = new string[numOfOptions + 1];
            options[1] = "Run a query for " + catsNames[categoryNum];
            options[2] = "Add " + catsNames[categoryNum];
            options[3] = "Show all " + catsNames[categoryNum] + "s";
            options[4] = "Go back";
            options[5] = "Exit";
            int choice = internalMenu(options, "", "an Action (1-3) or Go back (4) or Exit (5)");
            Console.Clear(); // clear the screen

            // calling for a method according to the action selected by the user
            switch (choice)
            {
                case 1: //Run a Query
                    RunQuery(categoryNum);
                    break;

                case 2: // Add
                    Add(categoryNum);
                    break;

                case 3: // Show all
                    ShowAll(categoryNum);
                    break;

                case 4:
                    MainMenu(); // go back
                    break;

                case 5:
                    return; //quit the program
            }
        }

        /***************************
         * Querying methods
         * ***************************/

        private void RunQuery(int categoryNum)
        {
            bool error = false;
            WriteColor("\n--- Running a query for a " + catsNames[categoryNum] + " ---\n", true, ConsoleColor.DarkGreen);

            // check if are there any recoreds of the current category
            List<Object> all = cats[categoryNum].GetAll();
            if (all.Any<object>())
            {
                PropertyInfo[] props = cats[categoryNum].GetEntityType().GetProperties();
                int numOfProperties = props.Length;

                // showing the user all of the object properties
                int index = 1;
                WriteColor("\nPlease choose one of the following fields:\n", true, ConsoleColor.DarkMagenta);

                foreach (PropertyInfo field in props)
                    if (field.CanRead)
                    {
                        Console.WriteLine("\t{0}. {1}", index, fieldsNames[field.Name]);
                        index++;
                    }

                // getting a property number from the user
                int max = index - 1;
                int choice = inputMenuLoop(max, "", "a valid property number (1-" + max + ")");

                // running a suitable query by the user's choice
                string[][] info = new string[1][];
                bool foundType = false;
                PropertyInfo fieldSelected = props[choice - 1];
                string fieldName = fieldsNames[fieldSelected.Name];
                List<Object> queryResult = new List<object>();

                /////////////////////////////////////////////////////////////////////////
                //////////////////////// findByName /////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////
                foreach (int numStringField in Enum.GetValues(typeof(StringFields)))
                {
                    if (string.Equals(Enum.GetName(typeof(StringFields), numStringField), fieldSelected.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        foundType = true;
                        WriteColor("\nPlease type a string to search by " + fieldName, true, ConsoleColor.DarkMagenta);
                        // information array about the input fields
                        int inputTestNumber = fieldsInfo[fieldSelected.Name];
                        info[0] = new string[3] { fieldName, Convert.ToString(inputTestNumber), "" };
                        // getting a value for search from the user
                        info = getInputsFromUser(info);
                        Array enums = Enum.GetValues(typeof(StringFields));
                        // check for exceptions
                        error = false;
                        try
                        {
                            queryResult = cats[categoryNum].FindByName(info[0][2], (StringFields)enums.GetValue(numStringField));
                        }
                        catch (InvalidDataException e)
                        {
                            error = true;
                            ShowError(e);
                        }
                        catch (System.Data.DataException e)
                        {
                            error = true;
                            ShowError(e);
                        }
                        catch (ArgumentNullException e)
                        {
                            error = true;
                            ShowError(e);
                        }
                        catch (Exception e)
                        {
                            error = true;
                            ShowError(e);
                        }
                        break;
                    }
                }
                /////////////////////////////////////////////////////////////////////////
                //////////////////////// findByNumber////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////

                if (!foundType)
                {
                    foreach (int numIntField in Enum.GetValues(typeof(IntFields)))
                    {
                        ///////
                        if (string.Equals("TransactionHistory", fieldSelected.Name) || string.Equals("Receipt", fieldSelected.Name))
                        {
                            error = true;
                            foundType = true;
                            break;
                        }
                        ///////

                        if (string.Equals(Enum.GetName(typeof(IntFields), numIntField), fieldSelected.Name, StringComparison.CurrentCultureIgnoreCase))
                        {
                            foundType = true;
                            // getting the requested search type from the user
                            if (fieldSelected.Name.Equals("Id")) // no range if this is an id search
                                choice = 1;
                            else
                            {
                                // showing the user the two options for a number search
                                WriteColor("\nPlease choose one of the following:\n", true, ConsoleColor.DarkMagenta);
                                Console.WriteLine("\t1. search for a specific number");
                                Console.WriteLine("\t2. search for a range of numbers");
                                choice = inputMenuLoop(2, "", "a valid number (1-2)");
                            }

                            // information array about the input fields
                            int inputTestNumber = fieldsInfo[fieldSelected.Name];
                            info[0] = new string[3] { fieldName, Convert.ToString(inputTestNumber), "" };
                            int minValue, maxValue;

                            // specific number search
                            if (choice == 1)
                            {
                                WriteColor("\nPlease type a numeric value to search by " + fieldName, true, ConsoleColor.DarkMagenta);
                                // getting a value for search from the user
                                info = getInputsFromUser(info);
                                minValue = int.Parse(info[0][2]);
                                maxValue = int.Parse(info[0][2]);
                            }

                                // range of numbers search
                            else
                            {
                                WriteColor("\nPlease type a range of numbers to search by " + fieldName, true, ConsoleColor.DarkMagenta);
                                // getting a --Mnimum-- value for search from the user
                                info[0][0] = "Miminum " + fieldName;
                                info = getInputsFromUser(info);
                                minValue = int.Parse(info[0][2]);
                                // getting a --Maximum-- value for search from the user
                                info[0][0] = "Maximum " + fieldName;
                                info = getInputsFromUser(info);
                                maxValue = int.Parse(info[0][2]);
                            }
                            Array enums = Enum.GetValues(typeof(IntFields));
                            // check for exceptions
                            error = false;
                            try
                            {
                                queryResult = cats[categoryNum].FindByNumber((IntFields)enums.GetValue(numIntField), minValue, maxValue);
                            }
                            catch (InvalidDataException e)
                            {
                                error = true;
                                ShowError(e);
                            }
                            catch (System.Data.DataException e)
                            {
                                error = true;
                                ShowError(e);
                            }
                            catch (ArgumentNullException e)
                            {
                                error = true;
                                ShowError(e);
                            }
                            catch (Exception e)
                            {
                                error = true;
                                ShowError(e);
                            }
                            break;
                        }
                    }
                }
                /////////////////////////////////////////////////////////////////////////
                //////////////////////// findByType /////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////

                string input;
                if (!foundType)
                {
                    // getting a value for search from the user
                    foreach (int numTypeField in Enum.GetValues(typeof(TypeFields)))
                    {
                        if (string.Equals(Enum.GetName(typeof(TypeFields), numTypeField), fieldSelected.Name, StringComparison.CurrentCultureIgnoreCase))
                        {
                            foundType = true;
                            WriteColor("\nPlease type a value to search by " + fieldName, true, ConsoleColor.DarkMagenta);
                            // information array about the input fields
                            int inputTestNumber = fieldsInfo[fieldSelected.Name];
                            info[0] = new string[3] { fieldName, Convert.ToString(inputTestNumber), "" };
                            // getting a value for search from the user
                            info = getInputsFromUser(info);
                        }
                    }
                    input = info[0][2];

                    //////////////////////////////////////////////////////////////////////////

                    if (foundType) // the user selected one of the 5 enum fields !!
                    {
                        Tuple<List<Object>, bool> back = new Tuple<List<object>, bool>(queryResult, error);
                        // gender
                        if (fieldSelected.Name.Equals("Gender"))
                        {
                            if (input.Equals("m") || input.Equals("M"))
                                back = SearchType(categoryNum, Gender.Male);
                            else
                                back = SearchType(categoryNum, Gender.Female);
                        }
                        // isStock
                        else if (fieldSelected.Name.Equals("InStock"))
                        {
                            if (input.Equals("1"))
                                back = SearchType(categoryNum, PStatus.Empty);
                            else if (input.Equals("2"))
                                back = SearchType(categoryNum, PStatus.LowQuantity);
                            else
                                back = SearchType(categoryNum, PStatus.InStock);
                        }
                        // type
                        else if (fieldSelected.Name.Equals("Type"))
                        {
                            if (input.Equals("a"))
                                back = SearchType(categoryNum, PType.a);
                            else if (input.Equals("b"))
                                back = SearchType(categoryNum, PType.b);
                            else
                                back = SearchType(categoryNum, PType.c);
                        }
                        // is_a_return
                        else if (fieldSelected.Name.Equals("Is_a_Return"))
                        {
                            if (input.Equals("r") || input.Equals("R"))
                                back = SearchType(categoryNum, Is_a_return.Return);
                            else
                                back = SearchType(categoryNum, Is_a_return.Purchase);
                        }
                        // payment
                        else if (fieldSelected.Name.Equals("Payment"))
                        {
                            if (input.Equals("1"))
                                back = SearchType(categoryNum, PaymentMethod.Cash);
                            else if (input.Equals("2"))
                                back = SearchType(categoryNum, PaymentMethod.Check);
                            else
                                back = SearchType(categoryNum, PaymentMethod.Visa);
                        }

                        ////.......try....catch......

                        queryResult = back.Item1;
                        error = back.Item2;
                    }
                } //main closer of type search


                ////************************************************
                ////************************************************
                ////************************************************
                ////************************************************
                ////************************************************



                // check whether the query succeeded or not
                if (foundType)
                {
                    if (!error) // if the category is not empty and no error in search
                    {
                        Console.Clear(); // clear the screen
                        WriteColor("\n--- Query results for " + catsNames[categoryNum] + "s ---", true, ConsoleColor.DarkGreen);
                        Console.WriteLine();
                        WriteColor("Search through field: " + fieldName, true, ConsoleColor.DarkGreen);
                        DisplayResult(categoryNum, queryResult);
                        if (queryResult.Any<object>())
                            ShowOne(categoryNum, queryResult);
                        else
                        {
                            PressEnter();
                            GoBack(categoryNum); // Action menu
                        }
                    }
                    else // if the category is not empty but there was an error in search
                    {
                        WriteColor("\nSorry, there are no results for " + catsNames[categoryNum] + "s.", true, ConsoleColor.Red);
                        PressEnter();
                        GoBack(categoryNum); // Action menu
                    }
                }
            }
            ///////////////////////////////////
            else // the category is empty
            {
                WriteColor("\nSorry, there are no results for " + catsNames[categoryNum] + "s.", true, ConsoleColor.Red);
                PressEnter();
                GoBack(categoryNum); // Action menu
            }
        }




        public Tuple<List<Object>, bool> SearchType(int categoryNum, ValueType input)
        {
            bool error = false;
            List<Object> queryResult = new List<object>();
            Tuple<List<Object>, bool> back;
            // check for exceptions
            error = false;
            try
            {
                queryResult = cats[categoryNum].FindByType(input);
            }
            catch (InvalidDataException e)
            {
                error = true;
                ShowError(e);
            }
            catch (System.Data.DataException e)
            {
                error = true;
                ShowError(e);
            }
            catch (ArgumentNullException e)
            {
                error = true;
                ShowError(e);
            }
            catch (Exception e)
            {
                error = true;
                ShowError(e);
            }

            back = new Tuple<List<Object>, bool>(queryResult, error);
            return back;
        }


        /***************************
         *Adding methods
         ****************************/

        private void Add(int categoryNum)
        {
            WriteColor("\n--- Creating a new " + catsNames[categoryNum] + " ---", true, ConsoleColor.DarkGreen);
            WriteColor("\nPlease type the following details:", true, ConsoleColor.DarkMagenta);
            Object newObj = new Object();
            switch (categoryNum)
            {
                case 1:
                    newObj = CreateClubMember();
                    break;
                case 2:
                    newObj = CreateDepartment();
                    break;
                case 3:
                    newObj = CreateEmployee();
                    break;
                case 4:
                    newObj = CreateProduct();
                    break;
                case 5:
                    newObj = CreateTransaction();
                    break;
                case 6:
                    newObj = CreateUser();
                    break;
            }
            // check for exceptions
            bool error = false;
            try
            {
                cats[categoryNum].Add(newObj);
            }
            catch (System.Data.DataException e)
            {
                error = true;
                ShowError(e);
            }
            catch (ArgumentNullException e)
            {
                error = true;
                ShowError(e);
            }
            catch (Exception e)
            {
                error = true;
                ShowError(e);
            }
            if (!error)
                WriteColor("\n" + catsNames[categoryNum] + " was added successfully!", true, ConsoleColor.Blue);
            PressEnter();
            GoBack(categoryNum); // Action menu
        }


        private string[][] getInputsFromUser(string[][] info)
        {
            // getting inputs from the user
            string cmd;
            bool isSuits;
            int numOfTest;
            for (int i = 0; i < info.Length; i++)
            {
                numOfTest = int.Parse(info[i][1]); // number of test information in static array inputsInfo
                if (numOfTest != 10)
                    Console.Write("\n{0}", inputsInfo[numOfTest][1]);
                cmd = getCmd(info[i][0]);
                isSuits = Regex.IsMatch(cmd, inputsInfo[numOfTest][0]);
                isSuits = inputIsSuits(isSuits, cmd, numOfTest);

                while (!isSuits) // checks validity of the input
                {
                    WriteColor("\nInvalid input! Please try again", true, ConsoleColor.Red);
                    Console.WriteLine("You should type {0}", inputsInfo[numOfTest][1]);
                    cmd = getCmd(info[i][0]);
                    isSuits = Regex.IsMatch(cmd, inputsInfo[numOfTest][0]);
                    isSuits = inputIsSuits(isSuits, cmd, numOfTest);
                }
                info[i][2] = cmd;
            }
            return info;
        }


        public string getCmd(string fieldName)
        {
            string cmd;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("\n{0}:", fieldName);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" ");
            cmd = ReceiveCmd();
            return cmd;
        }


        // check Overflow Exception
        public bool inputIsSuits(bool isSuits, string cmd, int numOfTest)
        {
            int int32Test;
            if (isSuits)
                if (numOfTest == 6 || numOfTest == 0) // only digits field
                    try
                    {
                        int32Test = int.Parse(cmd);
                    }
                    catch (OverflowException)
                    {
                        isSuits = false;
                        Console.WriteLine("\nToo big number...");
                    }
            return isSuits;
        }


        private ClubMember CreateClubMember()
        {
            // information array about the input fields
            string[][] info = new string[7][];
            info[0] = new string[3] { "ID", "0", "" };
            info[1] = new string[3] { "First name", "1", "" };
            info[2] = new string[3] { "Last name", "1", "" };
            info[3] = new string[3] { "Day of birth", "2", "" };
            info[4] = new string[3] { "Month of birth", "3", "" };
            info[5] = new string[3] { "Year of birth", "4", "" };
            info[6] = new string[3] { "Gender", "5", "" };
            // getting inputs from the user
            info = getInputsFromUser(info);
            ////// creation of fields
            int id = int.Parse(info[0][2]);
            string firstName = info[1][2];
            string lastName = info[2][2];
            // field: Transactions history
            List<Transaction> tranHistory = new List<Transaction>();
            // field: Date of Birth
            DateTime dateOfBirth = new DateTime(int.Parse(info[5][2]), int.Parse(info[4][2]), int.Parse(info[3][2]));
            // field: Gender
            Gender gender;
            if (info[6][2] == "m" || info[6][2] == "M")
                gender = Gender.Male;
            else
                gender = Gender.Female;
            // final creation
            return new ClubMember(id, firstName, lastName, tranHistory, dateOfBirth, gender, 0);
        }


        private Department CreateDepartment()
        {
            // information array about the input fields
            string[][] info = new string[1][];
            info[0] = new string[3] { "Department name", "1", "" };
            // getting inputs from the user
            info = getInputsFromUser(info);
            // creation of fields
            string name = info[0][2];
            // final creation
            return new Department(name, 0);
        }


        private Employee CreateEmployee()
        {
            // information array about the input fields
            string[][] info = new string[7][];
            info[0] = new string[3] { "ID", "0", "" };
            info[1] = new string[3] { "First name", "1", "" };
            info[2] = new string[3] { "Last name", "1", "" };
            info[3] = new string[3] { "Department ID", "6", "" };
            info[4] = new string[3] { "Salary", "6", "" };
            info[5] = new string[3] { "Supervisor ID", "6", "" };
            info[6] = new string[3] { "Gender", "5", "" };
            // getting inputs from the user
            info = getInputsFromUser(info);
            // creation of fields
            int id = int.Parse(info[0][2]);
            string firstName = info[1][2];
            string lastName = info[2][2];
            int depID = int.Parse(info[3][2]);
            int salary = int.Parse(info[4][2]);
            int supervisorID = int.Parse(info[5][2]);
            // field: Gender
            Gender gender;
            if (info[6][2] == "m" || info[6][2] == "M")
                gender = Gender.Male;
            else
                gender = Gender.Female;

            // final creation
            return new Employee(firstName, lastName, id, gender, depID, salary, supervisorID);
        }


        private Product CreateProduct()
        {
            // information array about the input fields
            string[][] info = new string[6][];
            info[0] = new string[3] { "Name", "1", "" };
            info[1] = new string[3] { "Product type", "7", "" };
            info[2] = new string[3] { "Price", "6", "" };
            info[3] = new string[3] { "Stock count", "6", "" };
            info[4] = new string[3] { "Product status", "8", "" };
            info[5] = new string[3] { "Location department ID", "6", "" };
            // getting inputs from the user
            info = getInputsFromUser(info);
            ////// creation of fields
            // field: Name
            string name = info[0][2];
            // field: Product type
            PType type;
            if (info[1][2] == "a")
                type = PType.a;
            else if (info[1][2] == "b")
                type = PType.b;
            else
                type = PType.c;
            // field: Price
            int price = int.Parse(info[2][2]);
            // field: Stock count
            int stockCount = int.Parse(info[3][2]);
            // field: Product status
            PStatus inStock;
            if (info[4][2] == "1")
                inStock = PStatus.Empty;
            else if (info[4][2] == "2")
                inStock = PStatus.LowQuantity;
            else
                inStock = PStatus.InStock;
            // field: Location department ID
            int location = int.Parse(info[5][2]);
            // final creation
            return new Product(name, type, location, inStock, stockCount, price, 0);
        }


        private Transaction CreateTransaction()
        {
            // information array about the input fields
            string[][] info = new string[2][];
            info[0] = new string[3] { "Transaction type", "9", "" };
            info[1] = new string[3] { "Payment method", "11", "" };
            // getting inputs from the user
            info = getInputsFromUser(info);
            ////// creation of fields
            // field: Transaction type
            Is_a_return is_a_return;
            if (info[0][2] == "R" || info[0][2] == "r")
                is_a_return = Is_a_return.Return;
            else
                is_a_return = Is_a_return.Purchase;
            // field: Payment method
            PaymentMethod payment;
            if (info[1][2] == "1")
                payment = PaymentMethod.Cash;
            else if (info[1][2] == "2")
                payment = PaymentMethod.Check;
            else
                payment = PaymentMethod.Visa;
            // field: Receipt
            Receipt receipt = CreateReceipt();
            // final creation
            return new Transaction(0, is_a_return, receipt, payment);
        }


        private User CreateUser()
        {
            // information array about the input fields
            string[][] info = new string[2][];
            info[0] = new string[3] { "User name", "10", "" };
            info[1] = new string[3] { "Password", "10", "" };
            // getting inputs from the user
            info = getInputsFromUser(info);
            // creation of fields
            string username = info[0][2];
            string password = info[1][2];
            // final creation
            return new User(username, password);
        }


        private Receipt CreateReceipt()
        {
            WriteColor("\n--- Creating a Receipt for the Transaction ---\n", true, ConsoleColor.DarkMagenta);
            // displays products
            WriteColor("\n--- List of all products exist ---", true, ConsoleColor.DarkGreen);
            List<Object> allProds = cats[4].GetAll();
            List<Product> requestedProds;

            // only if there are products
            if (allProds.Any<object>())
            {
                DisplayResult(4, allProds); // displays products

                // getting an input from the user
                WriteColor("\n\nPlease add at least one product to the receipt\n", true, ConsoleColor.DarkMagenta);
                requestedProds = new List<Product>();
                string[] options;
                int choice = 1;
                int newProd;

                while (choice == 1)
                {
                    // user choose a product to add
                    ////////WriteColor("\n\nProduct number to add to the receipt", true, ConsoleColor.DarkMagenta);
                    newProd = inputMenuLoop(allProds.Count, "Product number", "an existing record number from 1 to " + allProds.Count);
                    Product toAdd = (Product)allProds.ElementAt(newProd - 1);
                    requestedProds.Add(toAdd);

                    // user choose if he wants to add another producr
                    WriteColor("\nDo you want to add another product to the receipt?\n", true, ConsoleColor.DarkMagenta);
                    options = new string[] { null, "YES", "NO" };
                    choice = internalMenu(options, "", "1 for ADD or 2 for SKIP");
                }
                WriteColor("\nThe receipt  was created successfully\n", true, ConsoleColor.Blue);
            }
            else
            {
                WriteColor("\nThere are no Products to choose!\nAn empty receipt was created successfully\n", true, ConsoleColor.Blue);
                requestedProds = new List<Product>();
            }
            // final creation
            return new Receipt(requestedProds);
        }


        /***************************
         * Viewing methods
         ***************************/

        private void ShowAll(int categoryNum)
        {
            WriteColor("\n--- List of all " + catsNames[categoryNum] + "s ---\n", true, ConsoleColor.DarkGreen);
            List<Object> objList = cats[categoryNum].GetAll(); // get all the records
            DisplayResult(categoryNum, objList); // displays the records
            if (objList.Any<object>())
                ShowOne(categoryNum, objList);
            else
            {
                PressEnter();
                GoBack(categoryNum); // Action menu
            }
        }


        private void ShowOne(int categoryNum, List<Object> objList)
        {
            // getting an input from the user
            WriteColor("\n\nPlease select one of the following:\n", true, ConsoleColor.DarkMagenta);
            string[] options = new string[] { null, "View a single record", "Go back" };
            int choice = internalMenu(options, "", "1 or 2");

            // act according to the action selected by the user
            switch (choice)
            {
                // view a single record
                case 1:
                    // getting an input from the user
                    int numRecord;
                    Object currentRecord;
                    if (objList.Count == 1)
                    {
                        numRecord = 1;
                        currentRecord = objList.First();
                    }
                    else
                    {
                        WriteColor("\n\nPlease type a record number to view", true, ConsoleColor.DarkMagenta);
                        numRecord = inputMenuLoop(objList.Count, "Record number", "an existing record number from 1 to " + objList.Count);
                        // act according to the action selected by the user
                        currentRecord = objList.ElementAt(numRecord - 1);
                    }
                    Console.Clear();
                    WriteColor("\n--- View " + catsNames[categoryNum] + " no." + numRecord + " ---\n", true, ConsoleColor.DarkGreen);
                    DisplayRecord(currentRecord, true);
                    EditOrRemove(categoryNum, currentRecord); //menu for edit or remove or go back
                    break;

                // go back
                case 2:
                    GoBack(categoryNum); // Action menu
                    break;
            }
        }


        public void EditOrRemove(int categoryNum, Object currentRecord)
        {
            // getting an input from the user
            WriteColor("\n\nPlease select an action:\n", true, ConsoleColor.DarkMagenta);
            string[] options = new string[] { null, "Edit current record", "Remove current record", "Go back" };
            int choice = internalMenu(options, "", "1 for EDIT or 2 for REMOVE or 3 to GO BACK");

            // act according to the action selected by the user
            switch (choice)
            {
                case 1:
                    EditRecord(categoryNum, currentRecord);
                    break;
                case 2:
                    {
                        // getting an input from the user
                        WriteColor("Are you sure you want to REMOVE this " + catsNames[categoryNum] + "?", true, ConsoleColor.Red);
                        options = new string[] { null, "YES", "NO" };
                        choice = internalMenu(options, "", "1 for REMOVE or 2 for CANCEL REMOVAL");
                        // act according to the action selected by the user
                        switch (choice)
                        {
                            case 1:
                                Console.Clear();
                                RemoveRecord(categoryNum, currentRecord);
                                break;
                            case 2:
                                WriteColor("The removal action was canceled", true, ConsoleColor.Blue);
                                EditOrRemove(categoryNum, currentRecord); //menu for edit or remove or go back
                                break;
                        }
                        break;
                    }
                case 3:
                    Console.Clear(); // clear the screen
                    ShowAll(categoryNum);
                    break;
            }
        }


        private void DisplayResult(int categoryNum, List<Object> objList)
        {
            if (!objList.Any<object>())
            {
                WriteColor("\nSorry, there are no results for " + catsNames[categoryNum] + "s.", true, ConsoleColor.Red);
                //PressEnter();
                //GoBack(categoryNum); // Action menu
            }
            else
            {
                Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------------------------------");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Index |");
                int blankSpace;
                foreach (PropertyInfo field in objList.First().GetType().GetProperties())
                {
                    string fieldName = fieldsNames[field.Name];
                    blankSpace = AdjustBlankSpace(fieldName.Length);
                    Console.Write(" {0,-" + blankSpace + "} |", fieldName);
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------------------------------");
                int index = 1;
                foreach (Object obj in objList)
                {
                    Console.Write("{0,-5} |", index);
                    DisplayRecord(obj, false);
                    index++;
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------------------------------");
            }
        }


        private void DisplayRecord(Object obj, bool alone)
        {
            String fieldName;
            Object value;
            Type type;
            int blankSpace;
            foreach (PropertyInfo field in obj.GetType().GetProperties())
                if (field.CanRead)
                {
                    fieldName = fieldsNames[field.Name];
                    value = field.GetValue(obj, null);
                    type = field.PropertyType;
                    if (alone) // for a single record view
                    {
                        // for a Receipt field
                        if (type.Equals(typeof(Receipt)))
                            ((Receipt)value).ShowReceipt();
                        // for a List<Object> field
                        else if (type.Equals(typeof(List<Transaction>)))
                        {
                            if (value.GetType().Equals(typeof(List<Transaction>)))
                            {
                                Console.WriteLine("\n\n------------- Transaction History -------------");
                                List<Object> value1 = ((List<Transaction>)field.GetValue(obj, null)).ConvertAll(x => ((Object)x));
                                DisplayResult(5, value1);
                            }
                        }
                        else
                        {
                            // for a DateTime field
                            if (type.Equals(typeof(DateTime)))
                                value = ((DateTime)value).ToShortDateString();
                            // for all fields
                            Console.Write("\n{0,-12}: {1}", fieldName, value);
                        }
                    }
                    else // for a single record in a table view
                    {
                        blankSpace = AdjustBlankSpace(fieldName.Length);
                        // for a Receipt field
                        if (type.Equals(typeof(Receipt)))
                            value = ((Receipt)value).ProductsIDs.Count();
                        // for a List<Object> field
                        else if (type.Equals(typeof(List<Transaction>)))
                            value = ((List<Transaction>)value).Count();
                        // for a DateTime field
                        else if (type.Equals(typeof(DateTime)))
                            value = ((DateTime)value).ToShortDateString();
                        // for always
                        Console.Write(" {0,-" + blankSpace + "} |", value);
                    }
                }
            Console.WriteLine();
        }


        // adjust the space between each field in the view
        public int AdjustBlankSpace(int fieldNameLength)
        {
            int minimumSpace = 9;
            int blankSpace;
            blankSpace = fieldNameLength + 5;
            if (blankSpace < minimumSpace)
                blankSpace = minimumSpace;
            return blankSpace;
        }


        /***************************
         * Editing methods
         ***************************/

        private void EditRecord(int categoryNum, Object currentRecord)
        {
            Console.Clear(); // clear the screen
            WriteColor("\n--- Editing a " + catsNames[categoryNum] + " ---\n", true, ConsoleColor.DarkGreen);
            Object newRecord = new Object();
            switch (categoryNum)
            {
                case 1:
                    newRecord = new ClubMember((ClubMember)currentRecord);
                    break;
                case 2:
                    newRecord = new Department((Department)currentRecord);
                    break;
                case 3:
                    newRecord = new Employee((Employee)currentRecord);
                    break;
                case 4:
                    newRecord = new Product((Product)currentRecord);
                    break;
                case 5:
                    newRecord = new Transaction((Transaction)currentRecord);
                    break;
                case 6:
                    newRecord = new User((User)currentRecord);
                    break;
            }

            string[][] info = new string[1][];
            Object value;
            string fieldName;
            foreach (PropertyInfo field in currentRecord.GetType().GetProperties())
            {
                if (field.CanRead)
                {
                    if (field.Name != "Receipt" && field.Name != "TransactionHistory" && field.Name != "MemberID" && field.Name != "DepartmentID" && field.Name != "TransactionID" && field.Name != "ProductID")
                    {
                        value = field.GetValue(currentRecord, null);
                        fieldName = fieldsNames[field.Name];
                        // getting an input from the user
                        WriteColor("\nDo you want to edit the field of " + fieldName + "?", true, ConsoleColor.DarkMagenta);
                        Console.WriteLine("Current value: " + field.GetValue(currentRecord, null) + "\n");
                        string[] options = new string[] { null, "Edit this field", "Skip to the next field" };
                        int choice = internalMenu(options, "", "1 for EDIT or 2 for SKIP");
                        if (choice == 1)
                        {
                            // information array about the input fields
                            int inputTestNumber = fieldsInfo[field.Name];
                            info[0] = new string[3] { fieldName, Convert.ToString(inputTestNumber), "" };

                            // getting a value for search from the user
                            info = getInputsFromUser(info);
                            Object newField = info[0][2];

                            // convert the user's input to the type of current field
                            if (!field.PropertyType.IsEnum) // for no Enums
                                newField = Convert.ChangeType(newField, field.PropertyType);
                            else // for Enums
                            {
                                // field: Gender
                                if (field.PropertyType.Equals(typeof(Gender)))
                                {
                                    if (newField.Equals("m") || newField.Equals("M"))
                                        newField = Gender.Male;
                                    else
                                        newField = Gender.Female;
                                }

                                // field: Product status
                                else if (field.PropertyType.Equals(typeof(PStatus)))
                                {
                                    if (newField.Equals("1"))
                                        newField = PStatus.Empty;
                                    else if (newField.Equals("2"))
                                        newField = PStatus.LowQuantity;
                                    else
                                        newField = PStatus.InStock;
                                }

                                // field: Product type
                                else if (field.PropertyType.Equals(typeof(PType)))
                                {
                                    if (newField.Equals("a"))
                                        newField = PType.a;
                                    else if (newField.Equals("b"))
                                        newField = PType.b;
                                    else
                                        newField = PType.c;
                                }

                                // field: Is_a_return
                                else if (field.PropertyType.Equals(typeof(Is_a_return)))
                                {
                                    if (newField.Equals("R") || newField.Equals("r"))
                                        newField = Is_a_return.Return;
                                    else
                                        newField = Is_a_return.Purchase;
                                }

                                // field: Payment method
                                else if (field.PropertyType.Equals(typeof(PaymentMethod)))
                                {
                                    if (newField.Equals("1"))
                                        newField = PaymentMethod.Cash;
                                    else if (newField.Equals("2"))
                                        newField = PaymentMethod.Check;
                                    else
                                        newField = PaymentMethod.Visa;
                                }
                            }

                            // update the field value
                            field.SetValue(newRecord, newField);
                            WriteColor("\nThe field was edited successfully!\n", true, ConsoleColor.Blue);
                        }
                    }
                }
            }
            // check for exceptions
            bool error = false;
            try
            {
                cats[categoryNum].Edit(currentRecord, newRecord);
            }
            // cats[categoryNum].Add(currentRecord);
            catch (NullReferenceException e)
            {
                error = true;
                ShowError(e);
            }
            catch (IndexOutOfRangeException e)
            {
                error = true;
                ShowError(e);
            }
            catch (ArgumentNullException e)
            {
                error = true;
                ShowError(e);
            }
            catch (System.Data.DataException e)
            {
                error = true;
                ShowError(e);
            }
            catch (System.InvalidCastException e)
            {
                error = true;
                ShowError(e);
            }
            catch (Exception e)
            {
                error = true;
                ShowError(e);
            }
            if (!error)
                WriteColor("\n" + catsNames[categoryNum] + " was edited successfully!", true, ConsoleColor.Blue);
            PressEnter();
            ShowAll(categoryNum); // Action menu
        }


        /***************************
         * Removing methods
         * ***************************/

        private void RemoveRecord(int categoryNum, Object currentRecord)
        {
            WriteColor("\n--- Removing a " + catsNames[categoryNum] + " ---", true, ConsoleColor.DarkGreen);
            // check for exceptions
            bool error = false;
            try
            {
                cats[categoryNum].Remove(currentRecord);
            }
            catch (NullReferenceException e)
            {
                error = true;
                ShowError(e);
            }
            catch (IndexOutOfRangeException e)
            {
                error = true;
                ShowError(e);
            }
            catch (ArgumentNullException e)
            {
                error = true;
                ShowError(e);
            }
            catch (Exception e)
            {
                error = true;
                ShowError(e);
            }
            if (!error)
                WriteColor("\n" + catsNames[categoryNum] + " was removed successfully!", true, ConsoleColor.Blue);
            PressEnter();
            ShowAll(categoryNum); // Action menu
        }



    }
}
