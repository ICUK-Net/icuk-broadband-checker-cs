using System.Text.RegularExpressions;

namespace IcukBroadbandCheckerCs {
    internal static class Validation {
        internal static string PostcodeRegex = "^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\\s?[0-9][A-Za-z]{2})$";
        internal static string PhoneNumberRegex = @"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$";
        public static bool IsValidPostcode(string postcode) {
            return Regex.IsMatch(postcode, PostcodeRegex);
        }

        public static bool IsValidPhonenumber(string phonenumber) {
            return Regex.IsMatch(phonenumber, PhoneNumberRegex);
        }
    }
}
