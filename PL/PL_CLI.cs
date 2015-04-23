using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using BL;
using System.Text.RegularExpressions;
using System.Reflection;


namespace PL
{
    public class PL_CLI : IPL
    {
        // atributes
        private IBL[] cats;
        private string[] catsNames;
        public static string[][] inputsInfo = new string[11][];

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
            // product status, Payment method
            inputsInfo[8] = new string[2] { "^((1|2|3){1})$", "one of the following:\n\t1 - Empty\n\t2 - LowQuantity\n\t3 - InStock" };

            // transaction type - one char of R,r,P,p
            inputsInfo[9] = new string[2] { "^((R|r|P|p){1})$", "one of the following:\n\tR - Return\n\tP - Purchase" };

            // at least 6 characters of letters and digits
            // username, password
            inputsInfo[10] = new string[2] { "^[A-Za-z0-9]{6,}$", "at least 6 characters.\nOnly letters (A-Z) or (a-z) and digits (0-9) are allowed" };
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
        {/*
            bool login = false; ;
            while (true)
            {
                // Check username and password
                Console.WriteLine("\n--- User Log in ---");
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
                catch (NullReferenceException e)
                {
                    Console.WriteLine("\n" + e);
                }
                catch (System.Data.DataException e)
                {
                    Console.WriteLine("\n" + e);
                }


                if (login)
                {*/
                    MainMenu(); // Main menu
                    /*break;
                }
                else
                    Console.WriteLine("\nIncorrect username and password\n\nAccess denied! Please try again...");
            }*/
        }


        /***************************
         * Menu methods
         ****************************/

        private void MainMenu()
        {
            WriteColor("\n--- Main menu ---\n", true, ConsoleColor.DarkGreen);
            Console.WriteLine("Please select a data entity to manage:\n");
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
            Console.WriteLine("Please select an action:\n");
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



            ClubMember tk = new ClubMember(203608096, "Tomer", "Keizler", new List<Transaction>(), new DateTime(1991, 9, 5), Gender.Male, 0);
            PropertyInfo[] props1 = tk.GetType().GetProperties();
            foreach (PropertyInfo field in props1)
            {
                //Console.WriteLine(field.ToString());
                Console.WriteLine(field.GetValue(tk, null));
                Console.WriteLine(field.Name);
                Console.WriteLine(field.PropertyType);
                Console.WriteLine();

            }
            Console.ReadLine();



            WriteColor("\n--- Running a query for a " + catsNames[categoryNum] + " ---", true, ConsoleColor.DarkGreen);
            PropertyInfo[] props = cats[categoryNum].GetType().GetProperties();
            int numOfProperties = props.Length;

            // showing the user all of the object properties
            int index = 1;
            Console.WriteLine("\nPlease choose one of the following fields:");
            foreach (PropertyInfo field in props)
                if (field.CanRead)
                    Console.WriteLine("\t{0} - {1}", index, field.Name);

            // getting a property number from the user
            int max = index - 1;
            int choice = inputMenuLoop(max, "", "a valid property number (1-" + max + ")");

            // running a suitable query by the user's choice
            bool foundType = false;
            PropertyInfo fieldSelected = props[choice];
            List<Object> queryResult = new List<object>();
            // Name
            foreach (StringFields stringType in Enum.GetValues(typeof(StringFields)))
                if (stringType.Equals(fieldSelected.Name))
                {
                    foundType = true;
                    Console.WriteLine("\nPlease type a value for searching by the field selected");
                    // information array about the input fields
                    string[][] info = new string[1][];
                    info[0] = new string[3] { fieldSelected.Name, "1", "" };
                    // getting inputs from the user
                    info = getInputsFromUser(info);
                    string s = fieldSelected.PropertyType.Name;
                    //queryResult = cats[categoryNum].FindByName(info[0][2], typeof(StringFields).GetProperty(s).PropertyType);

                }
            // Number
            if (!foundType)
            {
                foreach (IntFields intType in Enum.GetValues(typeof(IntFields)))
                    if (intType.Equals(fieldSelected.Name))
                    {
                        foundType = true;
                        //
                    }
            }
            // Type
            if (!foundType)
            {
                    foundType = true;
                    //
            }
            
            



        }






        /***************************
         *Adding methods
         ****************************/

        private void Add(int categoryNum)
        {
            WriteColor("\n--- Creating a new  " + catsNames[categoryNum] + " ---", true, ConsoleColor.DarkGreen);
            Console.WriteLine("\nPlease type the following details:");
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
            cats[categoryNum].Add(newObj);
            WriteColor("\n" + catsNames[categoryNum] + " was added successfully!", true, ConsoleColor.Blue);
            PressEnter();
            GoBack(categoryNum); // Action menu
        }


        private string[][] getInputsFromUser(string[][] info)
        {
            // getting inputs from the user
            string cmd;
            bool isSuits;
            int numOfTest, int32Test;
            for (int i = 0; i < info.Length; i++)
            {
                numOfTest = int.Parse(info[i][1]); // number of test information in static array inputsInfo
                Console.Write("\n{0}", inputsInfo[numOfTest][1]);
                Console.Write("\n{0}: ", info[i][0]);
                cmd = ReceiveCmd();
                isSuits = Regex.IsMatch(cmd, inputsInfo[numOfTest][0]);
                if (isSuits)
                {
                    if (numOfTest == 6) // only digits field
                        try
                        {
                            int32Test = int.Parse(cmd);
                        }
                        catch (OverflowException)
                        {
                            isSuits = false;
                            Console.WriteLine("\nToo big number...");
                        }
                }
                while (!isSuits) // checks validity of the input
                {
                    WriteColor("\nInvalid input! Please try again", true, ConsoleColor.Red);
                    Console.WriteLine("You should type {0}", inputsInfo[numOfTest][1]);
                    Console.Write("\n{0}: ", info[i][0]);
                    cmd = ReceiveCmd();
                }
                info[i][2] = cmd;
            }
            return info;
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
            info[1] = new string[3] { "Payment method", "8", "" };
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
            List<Product> prods = new List<Product>();
            Receipt receipt = new Receipt(prods);
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


        /***************************
         * Viewing methods
         ***************************/

        private void ShowAll(int categoryNum)
        {
            Console.WriteLine("\n--- List of all {0}s ---\n", catsNames[categoryNum]);
            List<Object> objList = cats[categoryNum].GetAll(); // get all the records
            DisplayResult(categoryNum, objList); // displays the records
            if (objList.Any<object>())
                ShowOne(categoryNum, objList);
        }


        private void ShowOne(int categoryNum, List<Object> objList)
        {
            // getting an input from the user
            Console.WriteLine("\n\nPlease select one of the following:\n");
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
                        Console.WriteLine("\n\nPlease type a record number to view");
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
            Console.WriteLine("\n\nPlease select an action:\n");
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
                Console.WriteLine("Sorry, there are no results for {0}s.", catsNames[categoryNum]);
                PressEnter();
                GoBack(categoryNum); // Action menu
            }
            else
            {
                Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------");

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black; 
                Console.Write("Index |");
                int blankSpace;
                foreach (PropertyInfo field in objList.First().GetType().GetProperties())
                {
                    blankSpace = field.Name.Length + 5;
                    Console.Write(" {0,-" + blankSpace + "} |", field.Name);
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------");
                int index = 1;
                foreach (Object obj in objList)
                {
                    Console.Write("{0,-5} |", index);
                    DisplayRecord(obj, false);
                    index++;
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                Console.WriteLine("\n---------------------------------------------------------------------------------------------------------------");

            }
        }


        private void DisplayRecord(Object obj, bool alone)
        {
            int blankSpace;
            foreach (PropertyInfo field in obj.GetType().GetProperties())
                if (field.CanRead)
                {
                    if (alone)
                        Console.Write("\n{0,-12}: {1}", field.Name, field.GetValue(obj, null));
                    else
                    {
                        blankSpace = field.Name.Length + 5;
                        Console.Write(" {0,-" + blankSpace + "} |", field.GetValue(obj, null));
                    }
                }
            Console.WriteLine();
        }


        /***************************
         * Editing methods
         ***************************/

        private void EditRecord(int categoryNum, Object currentRecord)
        {
            // to be implemented...
        }


        /***************************
         * Removing methods
         * ***************************/

        private void RemoveRecord(int categoryNum, Object currentRecord)
        {
            WriteColor("\n--- Removing a " + catsNames[categoryNum] + " ---", true, ConsoleColor.DarkGreen);
            try
            {
                cats[categoryNum].Remove(currentRecord);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("\n" + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
            WriteColor("\n" + catsNames[categoryNum] + " was removed successfully!", true, ConsoleColor.Blue);
            PressEnter();
            ShowAll(categoryNum); // Action menu
        }

















        

        public void test()
        {
            ClubMember tk = new ClubMember(203608096, "Tomer", "Keizler", new List<Transaction>(), new DateTime(1991, 9, 5), Gender.Male, 0);
            PropertyInfo[] props = tk.GetType().GetProperties();
            foreach (PropertyInfo field in props)
            {
                //Console.WriteLine(field.ToString());
                Console.WriteLine(field.GetValue(tk, null));
                Console.WriteLine(field.Name);
                Console.WriteLine(field.PropertyType);
                Console.WriteLine();

            }
            Console.ReadLine();
        }






        /*
        
        public void Run1()
        {
            List<Product> q;
            string cmd;
            while (true)
            {
                Console.WriteLine("Please select and option:");
                Console.WriteLine("\t1. Query product by name");
                Console.WriteLine("\t2. Add product");
                Console.WriteLine("\t3. Exit");
                cmd = ReceiveCmd();
                Console.Clear(); //clear the screen
                switch (cmd)
                {
                    case "1":
                        Console.WriteLine("Please enter the product name:");
                        cmd = ReceiveCmd();
                        q = itsProductBL.FindByName(cmd, StringFields.name).Cast<Product>().ToList();  //************************************ For QueryString!!
                        //q = itsBL.FindByNumber(Convert.ToInt32(cmd), IntFields.*INTTYPE*).Cast<Product>().ToList(); **************** For QueryInt!!
                        if (Enum.IsDefined(typeof(PType), cmd))  ******************************************************************* For QueryType!!
                        {
                            PType PTypeValue = (PType)Enum.Parse(typeof(PType), cmd);
                            q = itsBL.FindByType(PTypeValue).Cast<Product>().ToList();
                            DisplayResult(q);
                        }
                         
                        Console.WriteLine("\nPress any key when ready");
                        DisplayResult(q); //****************************************************************************************** For Display Result Of Query!!
                        Console.ReadLine();
                        break;
                    case "2":
                        Product current = new Product("Tomer", PType.a, 2, PStatus.Empty, 2, 12);
                        itsProductBL.Add(current);
                        current = new Product("Asaf", PType.b, 3, PStatus.LowQuantity, 10, 300);
                        itsProductBL.Add(current);
                        return;
                    case "3":
                        return;//quit the program
                    default:
                        Console.WriteLine("That was an invalid command, please try again\n\n");
                        break;
                }

            }
        }
        */





    }
}