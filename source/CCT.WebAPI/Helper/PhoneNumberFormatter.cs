using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCT.WebAPI.Helper
{
    public static class PhoneNumberFormatter
    {
        public static string FormatPhoneNumber(string phoneNumber)
        {
            if(phoneNumber == null)
            {
                throw new ArgumentNullException("Phonenumber mustn't be null");
            }

            if(phoneNumber.Contains("/"))
            {
                phoneNumber = phoneNumber.Replace("/", "");
            }

            string formattedPhoneNumber = string.Empty;

            if(phoneNumber[0] == '+')
            {
                for (int i = 0; i < phoneNumber.Length; i++)
                {
                    if(i % 3 == 0)
                    {
                        formattedPhoneNumber += " ";
                    }

                    formattedPhoneNumber += phoneNumber[i];
                }
            }

            if(phoneNumber[0] == '0')
            {
                for (int i = 0; i < phoneNumber.Length; i++)
                {
                    if (i == 4 || ((i - 4) % 3 == 0 && i > 4))
                    {
                        formattedPhoneNumber += " ";
                    }

                    formattedPhoneNumber += phoneNumber[i];
                }
            }

            return formattedPhoneNumber;
        }
    }
}
