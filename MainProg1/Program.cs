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

            IDAL adal = new LINQ_DAL();
            IBL abl = new Product_BL(adal);
            IPL apl = new PL_CLI(abl);

            apl.Run();

        }
    }
}
