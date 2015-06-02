using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL;
using DAL;
using PL;

namespace MainProg
{
    public class MainProgram
    {
        public MainProgram()
        {
            IDAL myDal = new LINQ_DAL();

            IBL clubMember = new ClubMember_BL(myDal);
            IBL customer = new Customer_BL(myDal);
            IBL department = new Department_BL(myDal);
            IBL employee = new Employee_BL(myDal);
            IBL product = new Product_BL(myDal);
            IBL transaction = new Transaction_BL(myDal);
            IBL user = new User_BL(myDal);

            IPL myPL = new PL_GUI(clubMember, customer, department, employee, product, transaction, user);
            //myPL.Run();


            ///////////////////
            //Purchase p = new Purchase(product);
            //p.Show();
            ///////////////////
        }
    }
}
