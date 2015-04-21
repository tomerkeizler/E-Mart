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
            inputsInfo[0] = new string[2] { "^[0-9]{9}$", "exactly 9 digits (0-9)"};

            // only letters and possibly more than one word
            // firstName, lastName, Product - name, Department - name
            inputsInfo[1] = new string[2] { "^[A-Za-z]{2,}(( )[A-Za-z]{2,})*$", "only letters (A-Z) or (a-z)" };

            // day - 2-digit number between 01-31
            inputsInfo[2] = new string[2] { "^(0[1-9]|[12][0-9]|3[01])$", "a 2-digit number (01-31)"};
            
            // month - 2-digit number between 01-12
            inputsInfo[3] = new string[2] { "^(0[1-9]|1[012])$", "a 2-digit number (01-12)"};
            
            // year - 4-digit number starts with 19 or 20
            inputsInfo[4] = new string[2] { "^((19|20)[0-9][0-9])$", "a 4-digit number (19**) or (20**)"};
            
            // gender - M,m,F,f
            inputsInfo[5] = new string[2] { "^M|m|F|f$", "M for male or F for female"};
            
            // Number - unlimited digits
            inputsInfo[6] = new string[2] { "^[0-9]+$", "only digits (0-9)"};
            
            // one char of a,b,c
            // product type
            inputsInfo[7] = new string[2] { "^(a|b|c)$", "one of the types: a, b, c"};
            
            // one char of 1,2,3
            // product status, Payment method
            inputsInfo[8] = new string[2] { "^(1|2|3)$", "one of the following: \n\t1 - Empty, \n\t2 - LowQuantity, \n\t3 - InStock"};
            
            // transaction type - one char of R,r,P,p
            inputsInfo[9] = new string[2] { "^(R|r|P|p)$", "one of the following: \n\tR - Return, \n\tP - Purchase"};
            
            // at least 6 characters of letters and digits
            // username, password
            inputsInfo[10] = new string[2] { "^[A-Za-z0-9]{6,}$", "at least 6 characters.\nOnly letters (A-Z) or (a-z) and digits (0-9) are allowed"};
        }

        // constructors
        public PL_CLI(IBL itsClubMemberBL, IBL itsDepartmentBL, IBL itsEmployeeBL, IBL itsProductBL, IBL itsTransactionBL, IBL itsUserBL)
        {
            cats = new IBL[7];
            cats[1] = null;
            cats[1] = itsClubMemberBL;
            cats[2] = itsDepartmentBL;
            cats[3] = itsEmployeeBL;
            cats[4] = itsProductBL;
            cats[5] = itsTransactionBL;
            cats[6] = itsUserBL;
            catsNames = new string[6] { "Club member", "Department", "Employee", "Product", "Transaction", "User" };
        }

        // methods

        private string ReceiveCmd()
        {
            return Console.ReadLine();
        }

        // Start of program
        public void Run()
        {
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
                if (((User_BL)cats[6]).isItValidUser(new User(username, password)))
                    MainMenu(); // Main menu
                else
                    Console.WriteLine("\nIncorrect username and password\n\nAccess denied! Please try again...");
            }
        }


        /***************************
         *Menu methods
         ****************************/

        private void MainMenu()
        {
            int categoryNum;
            string cmd; 
            Console.WriteLine("\n--- Main menu ---\n");
            Console.WriteLine("Please select a data entity to manage:");
            Console.WriteLine("\t1. Club member");
            Console.WriteLine("\t2. Department");
            Console.WriteLine("\t3. Employee");
            Console.WriteLine("\t4. Product");
            Console.WriteLine("\t5. Transaction");
            Console.WriteLine("\t6. User");
            Console.WriteLine("\n\t7. Exit");

            // getting input from the user
            cmd = ReceiveCmd();
            while (!Regex.IsMatch(cmd, @"^[1-7]{1}$")) // checks validity of the input
            {
                Console.WriteLine("\nInvalid input! Please select a Data Entity (1-6) or Exit (7)");
                cmd = ReceiveCmd();
            }
            categoryNum = int.Parse(cmd);
            if (categoryNum == 7)
                return; //quit the program
            Console.Clear(); //clear the screen
            ActionMenu(categoryNum); // Action menu
        }


        private void ActionMenu(int categoryNum)
        {
            string cmd;
            Console.WriteLine("\n--- " + catsNames[categoryNum] + "s management ---\n");
            Console.WriteLine("Please select an action:");
            Console.WriteLine("\t1. Run a query for " + catsNames[categoryNum]);
            Console.WriteLine("\t2. Add " + catsNames[categoryNum]);
            Console.WriteLine("\t3. Show all " + catsNames[categoryNum] + "s");
            Console.WriteLine("\n\t4. Go back");
            Console.WriteLine("\t5. Exit");
            cmd = ReceiveCmd();
            while (!Regex.IsMatch(cmd, @"^[1-5]{1}$")) // checks validity of the input
            {
                Console.WriteLine("\nInvalid input! Please select an Action (1-3) or Go back (4) or Exit (5)");
                cmd = ReceiveCmd();
            }
            Console.Clear(); //clear the screen

            // calling for a method according to the action selected by the user
            switch (cmd)
            {
                case "1": //Run a Query
                    // to be continued...
                    break;

                case "2": // Add
                    Add(categoryNum);
                    break;

                case "3": // Show all
                    ShowAllRecords(categoryNum);
                    break;

                case "4":
                    MainMenu(); // go back
                    break;

                case "5":
                    return; //quit the program
            }
        }


        /***************************
         *Adding methods
         ****************************/

        private void Add(int categoryNum)
        {
            Console.WriteLine("\n--- Creating a new {0} ---", catsNames[categoryNum]);
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
            Console.WriteLine("\n{0} was added successfully!", catsNames[categoryNum]);
            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
            Console.Clear(); // clear the screen
            ActionMenu(categoryNum); // Action menu
        }


        private string[][] getInputsFromUser(string[][] info)
        {
            // getting inputs from the user
            string cmd;
            int numOfTest;
            for (int i = 0; i < info.Length; i++)
            {
                Console.Write("\n{0}: ", info[i][0]);
                cmd = ReceiveCmd();
                numOfTest = int.Parse(info[i][1]); // number of test information in static array inputsInfo
                while (!Regex.IsMatch(cmd, inputsInfo[numOfTest][0])) // checks validity of the input
                {
                    Console.WriteLine("\nInvalid input! Please try again");
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
            Console.WriteLine("\nPlease enter the following details:");
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
            Console.WriteLine("\nPlease enter the following details:");
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
            Console.WriteLine("\nPlease enter the following details:");
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
            Console.WriteLine("\nPlease enter the following details:");
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
            Console.WriteLine("\nPlease enter the following details:");
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
            Console.WriteLine("\nPlease enter the following details:");
            info = getInputsFromUser(info);
            // creation of fields
            string username = info[0][2];
            string password = info[1][2];
            // final creation
            return new User(username, password);
        }


       /***************************
        Showing methods
        ***************************/

        private void ShowAllRecords(int categoryNum)
        {
            Console.WriteLine("\n--- List of all {0}s ---\n", catsNames[categoryNum]);
            List<Object> objList = cats[categoryNum].GetAll(); // get all the records
            int maxRecord = DisplayResult(objList); // displays the records and returns the maximal record number
            AskOneRecord(objList, maxRecord);
        }


        private void AskOneRecord(List<Object> objList, int maxRecord)
        {
            Console.WriteLine("Please select one of the following:"); 
            Console.WriteLine("\t1 - Watch a single record");
            Console.WriteLine("\t2 - Go back");

            string cmd;
            int numRecord;
            cmd = ReceiveCmd();
            while (!Regex.IsMatch(cmd, @"^[1-2]{1}$")) // checks validity of the input
            {
                Console.WriteLine("\nInvalid input! Please select 1 or 2");
                cmd = ReceiveCmd();
            }
           
            // act according to the action selected by the user
            switch (cmd)
            {
                case "1":
                    // watch a single record
                    Console.WriteLine("\n\nPlease type a record number in order to watch it");
                    Console.Write("\nRecord number: ");
                    cmd = ReceiveCmd();
                    numRecord = int.Parse(cmd);
                    while (!Regex.IsMatch(cmd, @"^[0-9]+$") || numRecord > maxRecord || numRecord == 0) // checks validity of the input
                    {
                        if (numRecord > maxRecord || numRecord == 0)
                            Console.WriteLine("\nInvalid input!\nPlease type an existing record number from {0} to (1)", 1, maxRecord);
                        else
                            Console.WriteLine("\nInvalid input! Please type only digits (0-9)");
                        Console.Write("\nRecord number: ");
                        cmd = ReceiveCmd();
                        numRecord = int.Parse(cmd);
                    }
                    DisplayOneRecord(objList, numRecord);
                    break;

                case "2":
                    Console.Clear(); //clear the screen
                    MainMenu(); // go back
                    break;
            }
        }

        
        private int DisplayResult(List<Object> objList)
        {
            int index = 1;
            foreach (Object obj in objList)
            {
                Console.Write("[{0}]", index);
                foreach (PropertyInfo field in obj.GetType().GetProperties())
                    if (field.CanRead)
                        Console.Write("\t{0}", field.GetValue(obj, null));
                index++;
            }
            return index - 1;
        }


        private void DisplayOneRecord(List<Object> objList, int numRecord)
        {
            Object obj = objList.ElementAt(numRecord - 1);
                foreach (PropertyInfo field in obj.GetType().GetProperties())
                    if (field.CanRead)
                        Console.Write("\t{0}", field.GetValue(obj, null));

            /////// edit.....remove......
        }








        /*

        public void test(int categoryNum)
        {
            List<Object> objList = new List<Object>();
            objList.Add(new ClubMember(203608096, "Tomer", "Keizler", new List<Transaction>(), new DateTime(1991, 9, 5), Gender.Male, 0));

            string lstType = objList.GetType().Name;
            Console.WriteLine(lstType);

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