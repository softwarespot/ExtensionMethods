#region Required project assemblies

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

#endregion Required project assemblies

/*
* Created by softwarespot
* User: softwarespot
* Date: 2015/09/05
*/

namespace ExtensionMethods
{
    // The following links were used to aid in the creation of these extension methods:
    // http://extensionoverflow.codeplex.com/SourceControl/latest#ExtensionMethods/
    // http://blog.sortedbits.com/string-extension-methods-for-c-2/
    // http://extensionmethod.net/csharp/string
    // https://www.autoitscript.com/autoit3/docs/functions/String%20Management.htm

#pragma warning disable 1591

    public enum Cipher : byte
    {
        Rot1,
        Rot2,
        Rot3,
        Rot4,
        Rot5,
        Rot6,
        Rot7,
        Rot8,
        Rot9,
        Rot10,
        Rot11,
        Rot12,
        Rot13,
        Rot14,
        Rot15,
        Rot16,
        Rot17,
        Rot18,
        Rot19,
        Rot20,
        Rot21,
        Rot22,
        Rot23,
        Rot24,
        Rot25
    }

#pragma warning restore 1591

    /// <summary>
    ///     ISBN related flags
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public enum ISBN : byte
    {
        /// <summary>
        ///     10-digit ISBN
        /// </summary>
        Ten,

        /// <summary>
        ///     13-digit ISBN
        /// </summary>
        Thirteen
    }

    /// <summary>
    ///     Flags related to between
    /// </summary>
    public enum StringBetween : byte
    {
        /// <summary>
        ///     The end string at the end of a match starts the next possible match
        /// </summary>
        None = 0,

        /// <summary>
        ///     A further instance of the start starts the next match
        /// </summary>
        EndIsStart = 1
    }

    /// <summary>
    ///     Flags related to stripping white space
    /// </summary>
    [Flags] // http://hugoware.net/blog/enums-flags-and-csharp
    // ReSharper disable once InconsistentNaming
    public enum StringStripWS : byte
    {
        /// <summary>
        ///     Strip leading white space
        /// </summary>
        Leading = 1 << 0,

        /// <summary>
        ///     Strip trailing white space
        /// </summary>
        Trailing = 1 << 1,

        /// <summary>
        ///     Strip double (or more) spaces between words
        /// </summary>
        Spaces = 1 << 2,

        /// <summary>
        ///     Strip all spaces (over-rides all other flags)
        /// </summary>
        All = 1 << 3
    }

    /// <summary>
    ///     Extension methods related to System.Object
    /// </summary>
    public static class ObjectIs
    {
        /// <summary>
        ///     Checks if an object is not null
        /// </summary>
        /// <param name="value">A object to check</param>
        /// <returns>true if object was not null; otherwise, false</returns>
        public static bool IsNotNull(this object value)
        {
            return value != null;
        }

        /// <summary>
        ///     Checks if an object is null
        /// </summary>
        /// <param name="value">A object to check</param>
        /// <returns>true if object was null; otherwise, false</returns>
        public static bool IsNull(this object value)
        {
            return value == null;
        }
    }

    /// <summary>
    ///     Methods related to string conversion and manipulation
    /// </summary>
    public static class StringConvert
    {
        /// <summary>
        ///     Prefixes all line-feed characters ( (char) 10 ) with a carriage return character ( (char) 13 )
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>
        ///     The string with all instances of line-feed characters ( (char) 10 ) prefixed with a carriage return character
        ///     ( (char) 13 )
        /// </returns>
        // ReSharper disable once InconsistentNaming
        public static string AddCR(this string value)
        {
            return String.IsNullOrEmpty(value) ? String.Empty : new Regex("\n(?<!\r)").Replace(value, "\r\n");
        }

        /// <summary>
        ///     Postfixes all carriage return characters ( (char) 13 ) with a line-feed character ( (char) 10 )
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>
        ///     The string with all instances of carriage return characters ( (char) 13 ) prefixed with a line-feed character
        ///     ( (char) 10 )
        /// </returns>
        // ReSharper disable once InconsistentNaming
        public static string AddLF(this string value)
        {
            return String.IsNullOrEmpty(value) ? String.Empty : new Regex("\r(?!\n)").Replace(value, "\r\n");
        }

        /// <summary>
        ///     Find strings between two string delimiters
        /// </summary>
        /// <param name="value">The string to evaluate</param>
        /// <param name="start">The beginning of the string to find. Passing an empty or null string starts at the beginning</param>
        /// <param name="end">
        ///     The end of the string to find. Passing an empty or null string searches from start to end of the
        ///     string
        /// </param>
        /// <param name="mode">Search mode</param>
        /// <param name="isCaseSensitive">Is casesensitive matching</param>
        /// <returns>A string list collection. If no matches, then the count is zero</returns>
        public static List<string> Between(this string value, string start, string end,
            StringBetween mode = StringBetween.None, bool isCaseSensitive = false)
        {
            end = String.IsNullOrEmpty(end)
                ? @"\z"
                : String.Format(mode == StringBetween.None ? "{0}" : "(?={0})", Regex.Escape(end));
            start = String.IsNullOrEmpty(start) ? @"\A" : Regex.Escape(start);

            var matches = Regex.Matches(value,
                String.Format(@"(?{0}s){1}(.*?){2}", isCaseSensitive ? "" : "i", start, end));

            var list = new List<string>();
            for (var i = 0; i < matches.Count; i++)
            {
                list.Add(matches[i].Groups[1].Value);
            }

            return list;
        }

        /// <summary>
        ///     Compact a string to a certain number of characters left and right
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="countLeftAndRight">Number of characters left and right</param>
        /// <returns>Compacted string; otherwise, an empty string</returns>
        public static string Compact(this string value, int countLeftAndRight)
        {
            if (String.IsNullOrEmpty(value) || countLeftAndRight <= 0)
            {
                return String.Empty;
            }

            return value.Length > countLeftAndRight * 2 ?
                String.Format("{0}...{1}", value.Substring(0, countLeftAndRight), value.Substring(value.Length - countLeftAndRight, countLeftAndRight))
                : value;
        }

        /// <summary>
        ///     Checks if a string contains a given substring
        /// </summary>
        /// <param name="value">The string to evaluate</param>
        /// <param name="search">The string to seek</param>
        /// <param name="comparison">Flag related to comparison</param>
        /// <param name="occurrence">
        ///     Which occurrence of the substring to find in the string. Use a negative occurrence to search
        ///     from the right side. The default value is 1 (finds first occurrence)
        /// </param>
        /// <returns></returns>
        public static int InStr(this string value, string search,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase, int occurrence = 1)
        // Based on AutoIt's StringInStr()
        {
            const int error = -1;
            if (String.IsNullOrEmpty(value) || String.IsNullOrEmpty(search) || occurrence == 0)
            {
                return error;
            }

            int index = error, position = 0;
            if (occurrence > 0)
            {
                while (position < occurrence && (index = value.IndexOf(search, index + 1, comparison)) >= 0)
                {
                    position++;
                }
            }
            else
            {
                occurrence = Math.Abs(occurrence);
                if (occurrence <= 0)
                {
                    return index;
                }

                index = value.Length + 1;
                while (position < occurrence && (index = value.LastIndexOf(search, index - 1, comparison)) >= 0)
                {
                    position++;
                }
            }

            return index;
        }

        /// <summary>
        ///     Retrieve a number of characters from the left-hand side of a string
        /// </summary>
        /// <param name="value">The string to evaluate</param>
        /// <param name="count">The number of characters to retrieve</param>
        /// <returns>A string containing the leftmost count characters of the string; otherwise, an empty string</returns>
        public static string Left(this string value, int count)
        {
            return String.IsNullOrEmpty(value) || count < 0
                ? String.Empty
                : count < value.Length ? value.Substring(0, count) : value;
        }

        /// <summary>
        ///     Convert a string to lowercase
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>String to lowercase; otherwise, an empty string</returns>
        public static string Lower(this string value)
        {
            return String.IsNullOrEmpty(value) ? String.Empty : value.ToLower();
        }

        /// <summary>
        ///     Extracts a number of characters from a string
        /// </summary>
        /// <param name="value">The string to evaluate</param>
        /// <param name="start">The character position to start. ( 0 = first character )</param>
        /// <param name="count">The number of characters to extract. By default the entire remainder of the string</param>
        /// <returns>Extracted string</returns>
        public static string Mid(this string value, int start, int? count = null)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            if (count.IsNull())
            {
                count = value.Length - start;
            }

            return count < 0 || start < 0 || start + count > value.Length
                ? String.Empty
                // ReSharper disable once PossibleInvalidOperationException
                : value.Substring(start, (int)count);
        }

        /// <summary>
        ///     Convert a null string to an empty string
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>An empty string; otherwise, the original string</returns>
        public static string NullToEmpty(this string value)
        {
            return value.IsNull() ? String.Empty : value;
        }

        /// <summary>
        ///     Splits a path into the drive, directory, file name and file extension parts
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>A string array with 0 = filepath, 1 = drive, 2 = directory, 3 = filename, 4 = extension</returns>
        public static string[] PathSplit(this string value)
        {
            const int eFilePath = 0, eDrive = 1, eDir = 2, eFileName = 3, eExtension = 4, eMax = 5;
            var pathSplit = new string[eMax];
            pathSplit[eFilePath] = value;

            if (String.IsNullOrWhiteSpace(value))
            {
                return pathSplit;
            }

            int extension = value.LastIndexOf(".", StringComparison.OrdinalIgnoreCase), slash;

            if (value.IndexOf("/", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                value = value.Replace("/", @"\"); // Replace '/' with '\'
            }
            pathSplit[eDir] = value;
            pathSplit[eDrive] = value.Substring(0, 2); // Drive
            if (pathSplit[eDrive] == @"\\") // UNC path
            {
                slash = -1;
                var position = 1;
                while (position <= 3 &&
                       (slash = value.IndexOf(@"\", slash + 1, StringComparison.OrdinalIgnoreCase)) >= 0)
                {
                    position++;
                }
                if (slash >= 0)
                {
                    pathSplit[eDrive] = value.Substring(0, slash - 1);
                }
                pathSplit[eDir] = "A:" + pathSplit[eDir].Substring(pathSplit[eDrive].Length);
                // A: is appended to mimic the standard value if it wasn't UNC
                extension = value.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
            }

            slash = pathSplit[eDir].LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase);
            if (extension >= 0) // If an extension exists
            {
                pathSplit[eExtension] = pathSplit[eDir].Substring(extension); // Extension
                pathSplit[eFileName] = pathSplit[eDir].Substring(slash);
                pathSplit[eFileName] = pathSplit[eFileName].Substring(1,
                    pathSplit[eFileName].Length - pathSplit[eExtension].Length - 1);
            }
            else
            {
                if (pathSplit[eDir][pathSplit[eDir].Length - 1] != '\\')
                // If backslash doesn't exist (when it's a directory) then append to the end
                {
                    pathSplit[eDir] += @"\";
                    slash += 1; // slash = pathSplit[eDir].LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase);
                }
            }

            pathSplit[eDir] = pathSplit[eDir].Substring(0, slash + 1); // Path
            pathSplit[eDir] = pathSplit[eDir].Substring(2);

            return pathSplit;
        }

        /// <summary>
        ///     Repeats a string a specified number of times
        /// </summary>
        /// <param name="value">The string to evaluate</param>
        /// <param name="count">Number of times to repeat the string</param>
        /// <returns>Repeated string; otherwise, an empty string</returns>
        public static string Repeat(this string value, int count)
        {
            if (String.IsNullOrEmpty(value) || count <= 0)
            {
                return String.Empty;
            }

            var result = String.Empty; // From AutoIt's _StringRepeat()
            while (count > 1)
            {
                if ((count & 1) == 1)
                {
                    result += value;
                }
                value += value;
                count = count >> 1;
            }

            return value + result;

            // return String.Join(value, new string[++count]);
            // http://rosettacode.org/wiki/Repeat_a_string#C.23

            // var @string = new StringBuilder(value.Length * count--);
            // @string.Append(value);

            // for (int i = 0; i < count; i++)
            // {
            //    @string.Append(value);
            // }
            // return @string.ToString();
        }

        /// <summary>
        ///     Reverses the contents of a string
        /// </summary>
        /// <param name="value">The string to evaluate</param>
        /// <returns>Reversed string; otherwise, an empty string</returns>
        public static string Reverse(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            var array = value.ToCharArray();
            for (int i = 0, j = array.Length - 1; i < j; i++, j--)
            // This version is a slightly optimised version than Array.Reverse()
            {
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }

            return new string(array);

            // char[] array = value.ToCharArray();
            // Array.Reverse(array);
            // return new string(array);
        }

        /// <summary>
        ///     Retrieve a number of characters from the right-hand side of a string
        /// </summary>
        /// <param name="value">The string to evaluate</param>
        /// <param name="count">The number of characters to retrieve</param>
        /// <returns>A string containing the rightmost count characters of the string; otherwise, an empty string</returns>
        public static string Right(this string value, int count)
        {
            return String.IsNullOrEmpty(value) || count < 0
                ? String.Empty
                : count < value.Length ? value.Substring(value.Length - count, count) : value;
        }

        /// <summary>
        ///     Convert the string to Rot13 (alpha characters only)
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>Rot13 string; otherwise, an empty string</returns>
        public static string Rot13(this string value)
        {
            return String.IsNullOrEmpty(value)
                ? String.Empty
                : new string(value.ToCharArray().Select(
                    @char =>
                        (char) (@char >= 'A' && @char <= 'M' || @char >= 'a' && @char <= 'm'
                                ? @char + 13
                                : @char >= 'N' && @char <= 'Z' || @char >= 'n' && @char <= 'z' ? @char - 13 : @char)
                    ).ToArray());

            // if (String.IsNullOrEmpty(value))
            // {
            //    return String.Empty;
            // }

            // char[] array = value.ToCharArray();
            // for (int i = 0; i < array.Length; i++)
            // {
            //    if (array[i] >= 'A' && array[i] <= 'M' || array[i] >= 'a' && array[i] <= 'm')
            //    {
            //        array[i] = (char)(array[i] + 13);
            //    }
            //    else if (array[i] >= 'N' && array[i] <= 'Z' || array[i] >= 'n' && array[i] <= 'z')
            //    {
            //        array[i] = (char)(array[i] - 13);
            //    }
            // }

            // return new string(array);
        }

        /// <summary>
        ///     Convert the string to Rot18 (alphanumeric only)
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>Rot18 string; otherwise, an empty string</returns>
        public static string Rot18(this string value)
        {
            return String.IsNullOrEmpty(value)
                ? String.Empty
                : new string(value.ToCharArray().Select(
                    @char => (char) (@char >= '0' && @char <= '4'
                                    ? @char + 5
                                    : @char >= '5' && @char <= '9'
                                        ? @char - 5
                                        : @char >= 'A' && @char <= 'M' || @char >= 'a' && @char <= 'm'
                                            ? @char + 13
                                            : @char >= 'N' && @char <= 'Z' || @char >= 'n' && @char <= 'z'
                                                ? @char - 13
                                                : @char)
                    ).ToArray());
        }

        /// <summary>
        ///     Convert the string to Rot47 (alphanumeric and symbols only)
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>Rot47 string; otherwise, an empty string</returns>
        public static string Rot47(this string value)
        {
            return String.IsNullOrEmpty(value)
                ? String.Empty
                : new string(value.ToCharArray().Select(
                    @char =>
                        (char) (@char >= 33 && @char <= 79
                                    ? @char + 47
                                    : @char >= 80 && @char <= 126 ? @char - 47 : @char)
                    ).ToArray());
        }

        /// <summary>
        ///     Convert the string to Rot5 (digits only)
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>Rot5 string; otherwise, an empty string</returns>
        public static string Rot5(this string value)
        {
            return String.IsNullOrEmpty(value)
                ? String.Empty
                : new string(value.ToCharArray().Select(@char => (char) (!Char.IsDigit(@char)
                                    ? @char
                                    : @char >= '0' && @char <= '4' ? @char + 5 : @char - 5)).ToArray());
        }

        /// <summary>
        ///     Removes all carriage return values ( (char) 13 ) from a string
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>The string with all instances of the (char) 13 character removed; otherwise, an empty string</returns>
        // ReSharper disable once InconsistentNaming
        public static string StripCR(this string value)
        {
            return String.IsNullOrEmpty(value) ? String.Empty : new Regex("\r").Replace(value, String.Empty);
        }

        /// <summary>
        ///     Removes all line-feed values ( (char) 10 ) from a string
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>The string with all instances of the (char) 10 character removed; otherwise, an empty string</returns>
        // ReSharper disable once InconsistentNaming
        public static string StripLF(this string value)
        {
            return String.IsNullOrEmpty(value) ? String.Empty : new Regex("\n").Replace(value, String.Empty);
        }

        /// <summary>
        ///     Strips the white space ( (char) 0, (char) 9 - (char) 13 and (char) 32 ) in a string
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <param name="flags">Flags related to stripping white space</param>
        /// <returns>The new string stripped of the requested white space; otherwise, an empty string</returns>
        // ReSharper disable once InconsistentNaming
        public static string StripWS(this string value, StringStripWS flags)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            if ((flags & StringStripWS.All) == StringStripWS.All)
            {
                return new Regex(@"\s").Replace(value, String.Empty);
            }

            if ((flags & StringStripWS.Spaces) == StringStripWS.Spaces)
            {
                value = Regex.Replace(value, @"\s{2,}", String.Empty);
            }

            if ((flags & StringStripWS.Leading) == StringStripWS.Leading)
            {
                value = value.TrimStart();
            }

            if ((flags & StringStripWS.Trailing) == StringStripWS.Trailing)
            {
                value = value.TrimEnd();
            }

            return value;
        }

        /// <summary>
        ///     Trims a number of characters from the left hand side of a string
        /// </summary>
        /// <param name="value">The string to evaluate</param>
        /// <param name="count">The number of characters to trim</param>
        /// <returns>The string trimmed by count characters from the left; otherwise, an empty string</returns>
        public static string TrimLeft(this string value, int count)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            count = (count < 0 || count >= value.Length ? value.Length : count);

            return value.Substring(count, value.Length - count);
        }

        /// <summary>
        ///     Trims a number of characters from the right hand side of a string
        /// </summary>
        /// <param name="value">The string to evaluate</param>
        /// <param name="count">The number of characters to trim</param>
        /// <returns>The string trimmed by count characters from the right; otherwise, an empty string</returns>
        public static string TrimRight(this string value, int count)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            count = value.Length - (count < 0 || count > value.Length ? value.Length : count);

            return value.Substring(0, count);
        }

        /// <summary>
        ///     Convert a string to uppercase
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>String to uppercase; otherwise, an empty string</returns>
        public static string Upper(this string value)
        {
            return String.IsNullOrEmpty(value) ? String.Empty : value.ToUpper();
        }
    }

    /// <summary>
    ///     Extension methods related to checking if a string is equal to a condition
    /// </summary>
    public static class StringIs
    {
        /// <summary>
        ///     Checks if a string is representing T
        /// </summary>
        /// <typeparam name="T">Specifies the type to check</typeparam>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was a T datatype; otherwise, false</returns>
        public static bool Is<T>(this string value)
        {
            // http://stackoverflow.com/questions/1654871/generic-tryparse-extension-method
            var type = typeof(T);
            var tryParseMethod = type.GetMethod(
                "TryParse",
                new[]
                {
                    typeof (string),
                    type.MakeByRefType()
                });

            if (tryParseMethod.IsNull())
            {
                return false;
            }

            var result = false;
            object[] parameters = { value, default(T) };
            var isExecuted = tryParseMethod.Invoke(null, parameters);
            if (isExecuted is bool)
            {
                result = (bool)isExecuted;
            }

            return result;
        }

        /// <summary>
        ///     Checks if a string contains only alphanumeric characters ( 0-9 and A-Z )
        /// </summary>
        /// <param name="value">A string to check</param>
        /// <returns>true if string contains only alphanumeric characters; otherwise, false</returns>
        public static bool IsAlNum(this string value)
        {
            return !String.IsNullOrEmpty(value) && value.All(Char.IsLetterOrDigit);
        }

        /// <summary>
        ///     Checks if a string contains only alphabetic characters ( A-Z )
        /// </summary>
        /// <param name="value">A string to check</param>
        /// <returns>true if string contains only alphabetic characters; otherwise, false</returns>
        public static bool IsAlpha(this string value)
        {
            return !String.IsNullOrEmpty(value) && value.All(Char.IsLetter);
        }

        /// <summary>
        ///     Checks if a string contains only ASCII characters ( 0-127 )
        /// </summary>
        /// <param name="value">A string to check</param>
        /// <returns>true if string contains only ASCII characters; otherwise, false</returns>
        // ReSharper disable once InconsistentNaming
        public static bool IsASCII(this string value)
        {
            // Not any chars that are above 127
            return !String.IsNullOrEmpty(value) && !value.Any(@char => @char > '\x007f');
        }

        /// <summary>
        ///     Checks if a string is representing a boolean value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsBool(this string value)
        {
            bool result;

            return Boolean.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string is representing an 8-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsByte(this string value)
        {
            byte result;

            return Byte.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string is representing a char value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsChar(this string value)
        {
            char result;

            return Char.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string is representing a DateTime value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsDateTime(this string value)
        {
            DateTime result;

            return DateTime.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string is representing a decimal value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsDecimal(this string value)
        {
            decimal result;

            return Decimal.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string contains only digit characters ( 0-9 )
        /// </summary>
        /// <param name="value">A string to check</param>
        /// <returns>true if value contains only digit characters; otherwise, false</returns>
        public static bool IsDigit(this string value)
        {
            return !String.IsNullOrEmpty(value) && new Regex(@"^\d+$", RegexOptions.Compiled).IsMatch(value);
        }

        /// <summary>
        ///     Checks if a string is representing a double value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsDouble(this string value)
        {
            double result;

            return Double.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string is representing an email
        /// </summary>
        /// <param name="value">A string to check</param>
        /// <returns>true if string was an email; otherwise, false</returns>
        public static bool IsEmail(this string value)
        {
            return !String.IsNullOrEmpty(value) && new Regex(
                @"^[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?$")
                .IsMatch(value); // http://www.regular-expressions.info/email.html, modified by softwarespot
        }

        /// <summary>
        ///     Check if a string is empty
        /// </summary>
        /// <param name="value">A string to check</param>
        /// <returns>true if string was empty; otherwise, false</returns>
        public static bool IsEmpty(this string value)
        {
            return value.IsNotNull() && value.Length == 0;
        }

        /// <summary>
        ///     Checks if a string is representing a float value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsFloat(this string value)
        {
            float result;

            return Single.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string is representing a Guid value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsGuid(this string value)
        {
            return !String.IsNullOrEmpty(value) &&
                   new Regex(@"^(?:(?<curly>\{)?[0-9A-Fa-f]{8}-(?:[0-9A-Fa-f]{4}-){3}[0-9A-Fa-f]{12}(?(curly)\}))$")
                       .IsMatch(value);
            // Guid result;
            // return Guid.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string is representing a hexadecimal value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsHex(this string value)
        {
            return !String.IsNullOrEmpty(value) && new Regex(@"^0[xX][\dA-Fa-f]+$").IsMatch(value);
        }

        /// <summary>
        ///     Checks if a string is representing a 32-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsInt(this string value)
        {
            int result;

            return Int32.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string is representing a 16-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsInt16(this string value)
        {
            return IsShort(value);
        }

        /// <summary>
        ///     Checks if a string is representing a 32-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsInt32(this string value)
        {
            return IsInt(value);
        }

        /// <summary>
        ///     Checks if a string is representing a 64-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsInt64(this string value)
        {
            return IsLong(value);
        }

        /// <summary>
        ///     Checks if a string is representing an IP address
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was an IP address; otherwise, false</returns>
        // ReSharper disable once InconsistentNaming
        public static bool IsIPAddress(this string value)
        {
            // Regular expression modified by softwarespot
            return !String.IsNullOrEmpty(value) && new Regex(value).IsMatch(
                @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
        }

        /// <summary>
        ///     Checks if a string contains a valid ISBN number
        /// </summary>
        /// <param name="isbn">A string containing the value to check</param>
        /// <param name="type">An ISBN flag of the type to check</param>
        /// <returns>true if a valid ISBN number; otherwise, false</returns>
        // ReSharper disable once InconsistentNaming
        public static bool IsISBN(string isbn, ISBN type)
        {
            if (String.IsNullOrWhiteSpace(isbn))
            {
                return false;
            }

            var length = isbn.Length - 1;
            int counter, sum = 0;
            switch (type)
            {
                case ISBN.Ten:
                    {
                        if (isbn[length] == 'x' || isbn[length] == 'X')
                        {
                            length -= 1;
                            sum = 10;
                        }

                        counter = 10;
                        for (var i = 0; i <= length; i++)
                        {
                            if (isbn[i] < '0' || isbn[i] > '9')
                            {
                                continue;
                            }
                            sum += (isbn[i] - '0') * counter;
                            counter -= 1;
                        }
                        return sum % 11 == 0; // Divisible by 11
                    }
                case ISBN.Thirteen:
                    {
                        const int one = 1, three = 3;
                        counter = one;
                        for (var i = 0; i <= length; i++)
                        {
                            if (isbn[i] < '0' || isbn[i] > '9')
                            {
                                continue;
                            }
                            sum += (isbn[i] - '0') * counter;
                            counter = (counter == one) ? three : one;
                        }
                        return sum % 10 == 0; // Divisible by 10
                    }
            }

            return false;
        }

        /// <summary>
        ///     Checks if a string is representing a 64-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsLong(this string value)
        {
            long result;

            return Int64.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string contains only lowercase characters
        /// </summary>
        /// <param name="value">A string to check</param>
        /// <returns>true if string contains only lowercase characters.; otherwise, false</returns>
        public static bool IsLower(this string value)
        {
            return !String.IsNullOrEmpty(value) && value.All(Char.IsLower);
        }

        /// <summary>
        ///     Check if a string is null or empty
        /// </summary>
        /// <param name="value">A string to check</param>
        /// <returns>true if string was null or empty; otherwise, false</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        /// <summary>
        ///     Checks if a string is representing a numerical representation
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if string was a numerical representation; otherwise, false</returns>
        public static bool IsNumerical(this string value)
        {
            return !String.IsNullOrEmpty(value) && new Regex(
                    String.Format(@"^[-+]?\d+(?:{0}\d+)?$", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator),
                    RegexOptions.Compiled).IsMatch(value);
            // return IsDouble(value);
        }

        /// <summary>
        ///     Checks if a string is a palindrome
        /// </summary>
        /// <param name="value">A string to check</param>
        /// <returns>true if string was a palindrome; otherwise, false</returns>
        public static bool IsPalindrome(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return false;
            }

            for (int i = 0, j = value.Length - 1; i < j; i++, j--)
            {
                while (!Char.IsLetterOrDigit(value[i]))
                {
                    i++;
                }

                while (!Char.IsLetterOrDigit(value[j]))
                {
                    j--;
                }

                if (!Char.ToUpper(value[i]).Equals(Char.ToUpper(value[j])))
                {
                    return false;
                }
            }
            return true;
            // value = Regex.Replace(value, @"\W", ""); // Remove all non-word characters
            // char[] array = value.ToCharArray();
            // Array.Reverse(array);
            // return String.Equals(new string(array), value, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        ///     Checks if a string is representing an 8-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsSByte(this string value)
        {
            sbyte result;

            return SByte.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string is representing a 16-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsShort(this string value)
        {
            short result;

            return Int16.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string contains only whitespace characters ( (char) 0, (char) 9 - (char) 13 and (char) 32 )
        /// </summary>
        /// <param name="value">A string to check</param>
        /// <returns>true if string contains only whitespace characters; otherwise, false</returns>
        public static bool IsSpace(this string value)
        {
            return String.IsNullOrEmpty(value) || value.All(Char.IsWhiteSpace);
            // return !String.IsNullOrEmpty(value) && value.All(@char => (@char >= '\x0009' && @char <= '\x000d') || @char == '\x0000' || @char == '\x0020');
        }

        /// <summary>
        ///     Checks if a string contains characters
        /// </summary>
        /// <param name="value">A string containing characters to check</param>
        /// <returns>true if string contained characters; otherwise, false</returns>
        public static bool IsString(this string value)
        {
            return !String.IsNullOrEmpty(value);
        }

        /// <summary>
        ///     Determines the TypeCode the string is representing
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>TypeCode the string is representing</returns>
        public static TypeCode IsType(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return TypeCode.Empty;
            }

            if (new Regex(@"^-?\d+$").IsMatch(value)) // Integer
            {
                if (value.Substring(0, 1) == "-") // Negative number
                {
                    SByte @sbyte;
                    if (SByte.TryParse(value, out @sbyte))
                    {
                        return TypeCode.SByte;
                    }

                    Int16 int16;
                    if (Int16.TryParse(value, out int16))
                    {
                        return TypeCode.Int16;
                    }

                    Int32 int32;
                    if (Int32.TryParse(value, out int32))
                    {
                        return TypeCode.Int32;
                    }

                    Int64 int64;
                    if (Int64.TryParse(value, out int64))
                    {
                        return TypeCode.Int64;
                    }
                }
                else
                {
                    Byte @byte;
                    if (Byte.TryParse(value, out @byte))
                    {
                        return TypeCode.Byte;
                    }

                    UInt16 uint16;
                    if (UInt16.TryParse(value, out uint16))
                    {
                        return TypeCode.UInt16;
                    }

                    UInt32 uint32;
                    if (UInt32.TryParse(value, out uint32))
                    {
                        return TypeCode.Int32;
                    }

                    UInt64 uint64;
                    if (UInt64.TryParse(value, out uint64))
                    {
                        return TypeCode.UInt64;
                    }
                }
            }
            else // Double
            {
                var formatted = String.Format(@"^[\d{0}]+$", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                if (new Regex(formatted).IsMatch(value))
                {
                    Single @float;
                    if (Single.TryParse(value, out @float))
                    {
                        return TypeCode.Single;
                    }

                    Double @double;
                    if (Double.TryParse(value, out @double))
                    {
                        return TypeCode.Double;
                    }

                    Decimal @decimal;
                    if (Decimal.TryParse(value, out @decimal))
                    {
                        return TypeCode.Decimal;
                    }
                }
                else
                {
                    Boolean @bool;
                    if (Boolean.TryParse(value, out @bool))
                    {
                        return TypeCode.Boolean;
                    }

                    Char @char;
                    if (Char.TryParse(value, out @char))
                    {
                        return TypeCode.Char;
                    }

                    DateTime datetime;
                    if (DateTime.TryParse(value, out datetime))
                    {
                        return TypeCode.DateTime;
                    }
                }
            }

            return TypeCode.String;
        }

        /// <summary>
        ///     Checks if a string is representing a 32-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsUInt(this string value)
        {
            uint result;

            return UInt32.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string is representing a 16-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsUInt16(this string value)
        {
            return IsUShort(value);
        }

        /// <summary>
        ///     Checks if a string is representing a 32-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsUInt32(this string value)
        {
            return IsUInt(value);
        }

        /// <summary>
        ///     Checks if a string is representing a 64-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsUInt64(this string value)
        {
            return IsULong(value);
        }

        /// <summary>
        ///     Checks if a string is representing a 64-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsULong(this string value)
        {
            ulong result;

            return UInt64.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string contains only uppercase characters
        /// </summary>
        /// <param name="value">A string to check</param>
        /// <returns>true if string contains contains only uppercase characters; otherwise, false</returns>
        public static bool IsUpper(this string value)
        {
            return !String.IsNullOrEmpty(value) && value.All(Char.IsUpper);
        }

        /// <summary>
        ///     Checks if a string is representing a 16-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was the correct datatype; otherwise, false</returns>
        public static bool IsUShort(this string value)
        {
            ushort result;

            return UInt16.TryParse(value, out result);
        }

        /// <summary>
        ///     Checks if a string doesn't contain a set of characters
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <param name="invalid">A pattern of characters that shouldn't be present e.g. '\/|:"'</param>
        /// <returns>true if string didn't contain the set of characters; otherwise, false</returns>
        public static bool IsValid(this string value, string invalid)
        {
            return !String.IsNullOrEmpty(value) && !String.IsNullOrEmpty(invalid) &&
                new Regex(String.Format(@"[{0}]", Regex.Escape(invalid)), RegexOptions.IgnoreCase).IsMatch(value);
        }

        /// <summary>
        ///     Checks if a string doesn't contain a set of characters
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <param name="invalid">A char array of characters that shouldn't be present e.g. '\/|:"'</param>
        /// <returns>true if string didn't contain the set of characters; otherwise, false</returns>
        public static bool IsValid(this string value, char[] invalid)
        {
            return !invalid.IsNull() && invalid.Length != 0 && value.IsValid(new string(invalid));
        }

        /// <summary>
        ///     Checks if a filepath contains an approved filetype extension
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <param name="fileTypes">A string of filetype extensions delimited with ';' e.g. 'exe;txt;bat'</param>
        /// <returns>true if filepath was approved; otherwise, false</returns>
        public static bool IsValidType(this string value, string fileTypes)
        {
            return
                !String.IsNullOrEmpty(value) && !String.IsNullOrEmpty(fileTypes) &&
                new Regex(String.Format(@"\.(?:{0})$", Regex.Escape(fileTypes).Replace(';', '|')),
                    RegexOptions.IgnoreCase).IsMatch(value);
        }

        /// <summary>
        ///     Checks if a string is representing a web safe color
        /// </summary>
        /// <param name="value">A string containing the value to check</param>
        /// <returns>true if value was a web safe color; otherwise, false</returns>
        public static bool IsWebSafeColor(this string value)
        {
            return !String.IsNullOrEmpty(value) &&
                new Regex(@"^(?:#|0[xX])?(([CcFf0369])\2){3}$").IsMatch(value);
        }

        /// <summary>
        ///     Checks if a string contains only hexadecimal digit characters ( 0-9 and A-F )
        /// </summary>
        /// <param name="value">A string to check</param>
        /// <returns>true if string contains only hexadecimal digit characters; otherwise, false</returns>
        public static bool IsXDigit(this string value)
        {
            return !String.IsNullOrEmpty(value) && value.All(@char => Char.IsDigit(@char) ||
                                                                      (@char >= 'a' && @char <= 'f') ||
                                                                      (@char >= 'A' && @char <= 'F'));
            // return !String.IsNullOrEmpty(value) && new Regex(@"^[\dA-Fa-f]+$", RegexOptions.Compiled).IsMatch(value);
        }
    }

    /// <summary>
    ///     Extension methods related to parsing strings to another datatype
    /// </summary>
    public static class StringParse
    {
        /// <summary>
        ///     Convert a string as a T value
        /// </summary>
        /// <typeparam name="T">Specifies the type to convert to</typeparam>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a T datatype</returns>
        public static T ParseAs<T>(this string value)
        // http://stackoverflow.com/questions/1654871/generic-tryparse-extension-method
        {
            var type = typeof(T);
            var tryParseMethod = type.GetMethod(
                "TryParse",
                new[]
                {
                    typeof (string),
                    type.MakeByRefType()
                });

            var result = default(T);
            if (tryParseMethod.IsNull())
            {
                return result;
            }

            object[] parameters = { value, result };
            var isExecuted = tryParseMethod.Invoke(null, parameters);
            if (!(isExecuted is bool))
            {
                return result;
            }

            var successful = (bool)isExecuted;
            if (successful)
            {
                result = (T) parameters[1];
            }

            return result;
        }

        /// <summary>
        ///     Convert a string as a boolean value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a boolean datatype</returns>
        public static bool ParseAsBool(this string value)
        {
            bool result;
            Boolean.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as an 8-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of the 8-bit unsigned integer datatype</returns>
        public static byte ParseAsByte(this string value)
        {
            byte result;
            Byte.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as a char value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a char datatype</returns>
        public static char ParseAsChar(this string value)
        {
            char result;
            Char.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as a DateTime value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a DateTime datatype</returns>
        public static DateTime ParseAsDateTime(this string value)
        {
            DateTime result;
            DateTime.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as a decimal value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a decimal datatype</returns>
        public static decimal ParseAsDecimal(this string value)
        {
            decimal result;
            Decimal.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as a double value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a double datatype</returns>
        public static double ParseAsDouble(this string value)
        {
            double result;
            Double.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as a float value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a float datatype</returns>
        public static float ParseAsFloat(this string value)
        {
            float result;
            Single.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as a hexadecimal number to an unsigned 32-bit integer
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 32-bit unsigned integer datatype</returns>
        public static uint ParseAsHex(this string value)
        {
            var result = default(uint);
            if (!value.IsHex())
            {
                return result;
            }

            value = value.Substring(2);
            UInt32.TryParse(value, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as a 32-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 32-bit signed integer datatype</returns>
        public static int ParseAsInt(this string value)
        {
            int result;
            Int32.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as a 16-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 16-bit signed integer datatype</returns>
        public static Int16 ParseAsInt16(this string value)
        {
            return ParseAsShort(value);
        }

        /// <summary>
        ///     Convert a string as a 32-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 32-bit signed integer datatype</returns>
        public static Int32 ParseAsInt32(this string value)
        {
            return ParseAsInt(value);
        }

        /// <summary>
        ///     Convert a string as a 64-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 64-bit signed integer datatype</returns>
        public static Int64 ParseAsInt64(this string value)
        {
            return ParseAsLong(value);
        }

        /// <summary>
        ///     Convert a string as a 64-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 64-bit signed integer datatype</returns>
        public static long ParseAsLong(this string value)
        {
            long result;
            Int64.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as an 8-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 8-bit signed integer datatype</returns>
        public static sbyte ParseAsSByte(this string value)
        {
            sbyte result;
            SByte.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as a 16-bit signed integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 16-bit signed integer datatype</returns>
        public static short ParseAsShort(this string value)
        {
            short result;
            Int16.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as a 32-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 32-bit unsigned integer datatype</returns>
        public static uint ParseAsUInt(this string value)
        {
            uint result;
            UInt32.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as a 16-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 16-bit unsigned integer datatype</returns>
        public static UInt16 ParseAsUInt16(this string value)
        {
            return ParseAsUShort(value);
        }

        /// <summary>
        ///     Convert a string as a 32-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 32-bit unsigned integer datatype</returns>
        public static UInt32 ParseAsUInt32(this string value)
        {
            return ParseAsUInt(value);
        }

        /// <summary>
        ///     Convert a string as a 64-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 64-bit unsigned integer datatype</returns>
        public static UInt64 ParseAsUInt64(this string value)
        {
            return ParseAsULong(value);
        }

        /// <summary>
        ///     Convert a string as a 64-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 64-bit unsigned integer datatype</returns>
        public static ulong ParseAsULong(this string value)
        {
            ulong result;
            UInt64.TryParse(value, out result);

            return result;
        }

        /// <summary>
        ///     Convert a string as a 16-bit unsigned integer value
        /// </summary>
        /// <param name="value">A string containing the value to convert</param>
        /// <returns>converted value; otherwise, default value of a 16-bit unsigned integer datatype</returns>
        public static ushort ParseAsUShort(this string value)
        {
            ushort result;
            UInt16.TryParse(value, out result);

            return result;
        }
    }
}
