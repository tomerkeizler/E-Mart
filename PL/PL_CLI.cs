using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using BL;
using System.Text.RegularExpressions;

namespace PL
{
    public class PL_CLI : IPL
    {
        // atributes
        private IBL[] cats;
        private string[] catsNames;

        // constructors
        public PL_CLI(IBL itsClubMemberBL, IBL itsDepartmentBL, IBL itsEmployeeBL, IBL itsProductBL, IBL itsTransactionBL, IBL itsUserBL)
        {
            cats = new IBL[6];
            cats[0] = itsClubMemberBL;
            cats[1] = itsDepartmentBL;
            cats[2] = itsEmployeeBL;
            cats[3] = itsProductBL;
            cats[4] = itsTransactionBL;
            cats[5] = itsUserBL;
            catsNames = new string[6] { "Club member", "Department", "Employee", "Product", "Transaction", "User" };
        }

        // methods

        // to be changes.....
        private void DisplayResult(List<Product> prod)
        {
            foreach (Product p in prod)
            {
                Console.WriteLine(p.Name + " " + p.Type);
            }
        }

        private string ReceiveCmd()
        {
            return Console.ReadLine();
        }








        public void Run()
        {
            int categoryNum;
            string cmd;
            while (true)
            {
                // Main menu
                Console.WriteLine("--- Main menu ---");
                Console.WriteLine("Please select a data entity to manage:");
                Console.WriteLine("\t1. Club member");
                Console.WriteLine("\t2. Department");
                Console.WriteLine("\t3. Employee");
                Console.WriteLine("\t4. Product");
                Console.WriteLine("\t5. Transaction");
                Console.WriteLine("\t6. User");
                Console.WriteLine("\n\n\t7. Exit");
                // getting input from the user
                cmd = ReceiveCmd();
                while (!Regex.IsMatch(cmd, @"^[1-7]{1}$")) // checks validity of the input
                {
                    Console.WriteLine("\nInvalid input! Please select a data entity (1-6) or exit (7)");
                    cmd = ReceiveCmd();
                }
                categoryNum = int.Parse(cmd);
                if (categoryNum == 7)
                    return; //quit the program
                Console.Clear(); //clear the screen

                // Action menu
                Console.WriteLine("\n--- " + catsNames[categoryNum - 1] + "s management ---");
                Console.WriteLine("Please select an action:");
                Console.WriteLine("\t1. Run a query for " + catsNames[categoryNum - 1]);
                Console.WriteLine("\t2. Add " + catsNames[categoryNum - 1]);
                Console.WriteLine("\t3. Show all " + catsNames[categoryNum - 1] + "s");
                Console.WriteLine("\t4. Exit");
                cmd = ReceiveCmd();
                while (!Regex.IsMatch(cmd, @"^[1-4]{1}$")) // checks validity of the input
                {
                    Console.WriteLine("\nInvalid input! Please select an action (1-3) or exit (4)");
                    cmd = ReceiveCmd();
                }
                Console.Clear(); //clear the screen


                switch (cmd)
                {
                    case "1":
                        // to be continued...
                        break;

                    case "2":
                        // to be continued...
                        break;

                    case "3":
                        // to be continued...
                        break;

                    case "4":
                        return; //quit the program
                }
            }
        }







        private void Add(int categoryNum)
        {          
            Object newObj;  
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
            cats[categoryNum-1].Add(newObj);
        }


        private ClubMember CreateClubMember()
        {
            // information array about the input fields
            string[][] info = new string[7][];
            info[0] = new string[4] { "ID", "^[0-9]{9}$", "exactly 9 digits (0-9)", "" };
            info[1] = new string[4] { "First name", "^[A-Za-z]{2,}(( )[A-Za-z]{2,})*$", "only letters (A-Z) or (a-z)", "" };
            info[2] = new string[4] { "Last name", "^[A-Za-z]{2,}(( )[A-Za-z]{2,})*$", "only letters (A-Z) or (a-z)", "" };
            info[3] = new string[4] { "Day of birth", "^(0[1-9]|[12][0-9]|3[01])$", "a 2-digit number (01-31)", "" };
            info[4] = new string[4] { "Month of birth", "^(0[1-9]|1[012])$", "a 2-digit number (01-31)", "" };
            info[5] = new string[4] { "Year of birth", "^((19|20)[0-9][0-9])$", "a 4-digit number (19**) or (20**)", "" };
            info[6] = new string[4] { "Gender", "^M|m|F|f$", "M for male or F for female", "" };
            // getting inputs from the user
            string cmd;
            Console.WriteLine("\n--- Creating a new Club member ---");
            Console.WriteLine("Please enter the following details:");
            for (int i = 0; i < info.Length; i++)
            {
                Console.Write("\n{0}: ", info[i][0]);
                cmd = ReceiveCmd();
                while (!Regex.IsMatch(cmd, @info[i][1])) // checks validity of the input
                {
                    Console.WriteLine("\nInvalid input! Please try again");
                    Console.WriteLine("You should type {0}", info[i][2]);
                    Console.Write("\n{0}: ", info[i][0]);
                    cmd = ReceiveCmd();
                }
                info[i][3] = cmd;
            }
            // creation of fields
            int memberID = 0;
            int id = int.Parse(info[0][3]);
            string firstName = info[1][3];
            string lastName = info[2][3];
            List<Transaction> tranHistory = new List<Transaction>();
            DateTime dateOfBirth = new DateTime(int.Parse(info[5][3]), int.Parse(info[4][3]), int.Parse(info[3][3]));
            Gender gender;
            if (info[6][3] == "m" || info[6][3] == "M")
                gender = Gender.Male;
            else
                gender = Gender.Female;
            // final creation
            return new ClubMember(memberID, id, firstName, lastName, tranHistory, dateOfBirth, gender);
        }
















        
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
                        /*if (Enum.IsDefined(typeof(PType), cmd))  ******************************************************************* For QueryType!!
                        {
                            PType PTypeValue = (PType)Enum.Parse(typeof(PType), cmd);
                            q = itsBL.FindByType(PTypeValue).Cast<Product>().ToList();
                            DisplayResult(q);
                        }
                         */
                        Console.WriteLine("\nPress any key when ready");
                        DisplayResult(q); //****************************************************************************************** For Display Result Of Query!!
                        Console.ReadLine();
                        break;
                    case "2":
                        //to compile implementation later...
                        /*Console.WriteLine("Sorry, this feature has not been implimented yet");
                        Console.WriteLine("\nPress any key when ready");*/
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
        





    }
}