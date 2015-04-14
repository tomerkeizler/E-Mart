using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using BL;

namespace PL
{
    public class PL_CLI : IPL
    {
        private IBL itsBL;

        public PL_CLI(IBL bl)
        {
            itsBL = bl;
        }

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
                switch (cmd){
                    case "1":
                        Console.WriteLine("Please enter the product name:");
                        cmd = ReceiveCmd();
                        q = itsBL.FindByName(cmd, StringFields.name).Cast<Product>().ToList();
                        DisplayResult(q);
                        Console.WriteLine("\nPress any key when ready");
                        Console.ReadLine();
                        break;
                    case "2":
                    //to compile implementation later...
                    Console.WriteLine("Sorry, this feature has not been implimented yet");
                    Console.WriteLine("\nPress any key when ready");
                    Console.ReadLine();
                    break;
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