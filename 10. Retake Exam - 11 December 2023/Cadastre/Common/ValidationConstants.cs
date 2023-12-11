using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Common
{
    public class ValidationConstants
    {
        //District
        public const int DisctrictNameMinLength = 2;
        public const int DisctrictNameMaxLength = 80;
        public const string DistrictPostalCodeRegex = @"^[A-Z]{2}-\d{5}";
        public const int DistrictPostalCodeMaxLength = 8;

        //Property
        public const int PropertyIdentifierMinLength = 16;
        public const int PropertyIdentifierMaxLength = 20;
        public const int PropertyDetailsMinLength = 5;
        public const int PropertyDetailsMaxLength = 500;
        public const int PropertyAddressMinLength = 5;
        public const int PropertyAddressMaxLength = 200;
        public const int PropertyAreaMinValue = 0;

        //Citizen
        public const int CitizenFirstNameMinLength = 2;
        public const int CitizenFirstNameMaxLength = 30;
        public const int CitizenLastNameMinLength = 2;
        public const int CitizenLastNameMaxLength = 30;
    }
}
