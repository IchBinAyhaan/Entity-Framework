using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreWithEntity.Extensions
{
    public static class CharExtensions
    {
         public static bool IsvalidChoice(this char choice)
        {
            if(choice.ToString().ToLower() == "y" || choice.ToString().ToLower()=="n")
                    return true;

            return false;
            

            
        }
    }
}
