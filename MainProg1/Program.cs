using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PL;
using BL;
using DAL;

namespace MainProg1
{
    class Program
    {
        static void Main(string[] args)
        {
            IDAL myDal = new LINQ_DAL();

            IBL clubMember = new ClubMember_BL(myDal);
            IBL department = new Department_BL(myDal);
            IBL employee = new Employee_BL(myDal);
            IBL product = new Product_BL(myDal);
            IBL transaction = new Transaction_BL(myDal);
            IBL user = new User_BL(myDal);

            IPL myPl = new PL_CLI(clubMember, department, employee, product, transaction, user);

            myPl.Run();

        }
    }
}
