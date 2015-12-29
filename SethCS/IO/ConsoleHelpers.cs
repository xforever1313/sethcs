//          Copyright Seth Hendrick 2015.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file ../../LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo( "Tests" )]
[assembly: InternalsVisibleTo( "TestsMono" )]
namespace SethCS
{
namespace IO
{
    public static class ConsoleHelpers
    {
        // -------- Fields --------

        /// <summary>
        /// The default message that is printed to the console
        /// when asking the user for information.
        /// </summary>
        internal const string DefaultCinMessage = "> ";

        /// <summary>
        /// Error message that appears when the user does not provide a valid bool.
        /// </summary>
        internal const string ParseBoolErrorMessage = "Error - Not given a bool, try again.";

        /// <summary>
        /// Error message that appears when the user does not provide a valid ushort.
        /// </summary>
        internal const string ParseUShortErrorMessage = "Error - Not given a ushort, try again.";

        /// <summary>
        /// Error message that appears when the user does not provide a valid short.
        /// </summary>
        internal const string ParseShortErrorMessage = "Error - Not given a short, try again.";

        /// <summary>
        /// Error message that appears when the user does not provide a valid int.
        /// </summary>
        internal const string ParseIntErrorMessage = "Error - Not given an int, try again.";

        /// <summary>
        /// Error message that appears when the user does not provide a valid uint.
        /// </summary>
        internal const string ParseUIntErrorMessage = "Error - Not given a uint, try again.";

        /// <summary>
        /// Error message that appears when the user does not provide a valid long.
        /// </summary>
        internal const string ParseLongErrorMessage = "Error - Not given a long, try again.";

        /// <summary>
        /// Error message that appears when the user does not provide a valid ulong.
        /// </summary>
        internal const string ParseULongErrorMessage = "Error - Not given a ulong, try again.";

        /// <summary>
        /// Error message that appears when the user does not provide a valid float.
        /// </summary>
        internal const string ParseFloatErrorMessage = "Error - Not given a float, try again.";

        /// <summary>
        /// Error message that appears when the user does not provide a valid double.
        /// </summary>
        internal const string ParseDoubleErrorMessage = "Error - Not given a double, try again.";

        /// <summary>
        /// Error message that appears when the user does not provide a valid decimal.
        /// </summary>
        internal const string ParseDecimalErrorMessage = "Error - Not given a decimal, try again.";

        // -------- Console In Helpers --------

        // ---- Number Conversions ----

        /// <summary>
        /// Prompts the user for an bool from the console.  It will keep prompting until
        /// the user provides a valid bool.  The function will then return the bool.
        /// </summary>
        /// <param name="inputMessage">
        /// The message gets gets printed to the console before we prompt the user for input.
        /// Default: "> ".
        /// </param>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        /// <returns>The first bool from cin.  Null if EOF is reached before we get one.</returns>
        public static bool? GetBool( string inputMessage = DefaultCinMessage, TextReader cin = null, TextWriter cout = null )
        {
            BindConsole( ref cin, ref cout );

            cout.Write( inputMessage );

            bool? returnValue  = null;
            string line = cin.ReadLine();
            while ( ( line != null ) && ( returnValue == null ) )
            {
                bool parsedValue;
                if ( bool.TryParse ( line, out parsedValue ) )
                {
                    returnValue = parsedValue;
                }
                else
                {
                    cout.WriteLine( ParseBoolErrorMessage );
                    cout.Write( inputMessage );
                    line = cin.ReadLine();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Prompts the user for an short from the console.  It will keep prompting until
        /// the user provides a valid short.  The function will then return the short.
        /// </summary>
        /// <param name="inputMessage">
        /// The message gets gets printed to the console before we prompt the user for input.
        /// Default: "> ".
        /// </param>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        /// <returns>The first short from cin.  Null if EOF is reached before we get one.</returns>
        public static short? GetShort( string inputMessage = DefaultCinMessage, TextReader cin = null, TextWriter cout = null )
        {
            BindConsole( ref cin, ref cout );

            cout.Write( inputMessage );

            short? returnValue  = null;
            string line = cin.ReadLine();
            while ( ( line != null ) && ( returnValue == null ) )
            {
                short parsedValue;
                if ( short.TryParse ( line, out parsedValue ) )
                {
                    returnValue = parsedValue;
                }
                else
                {
                    cout.WriteLine( ParseShortErrorMessage );
                    cout.Write( inputMessage );
                    line = cin.ReadLine();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Prompts the user for an ushort from the console.  It will keep prompting until
        /// the user provides a valid short.  The function will then return the ushort.
        /// </summary>
        /// <param name="inputMessage">
        /// The message gets gets printed to the console before we prompt the user for input.
        /// Default: "> ".
        /// </param>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        /// <returns>The first ushort from cin.  Null if EOF is reached before we get one.</returns>
        public static ushort? GetUShort( string inputMessage = DefaultCinMessage, TextReader cin = null, TextWriter cout = null )
        {
            BindConsole( ref cin, ref cout );

            cout.Write( inputMessage );

            ushort? returnValue  = null;
            string line = cin.ReadLine();
            while ( ( line != null ) && ( returnValue == null ) )
            {
                ushort parsedValue;
                if ( ushort.TryParse ( line, out parsedValue ) )
                {
                    returnValue = parsedValue;
                }
                else
                {
                    cout.WriteLine( ParseUShortErrorMessage );
                    cout.Write( inputMessage );
                    line = cin.ReadLine();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Prompts the user for an int from the console.  It will keep prompting until
        /// the user provides a valid int.  The function will then return the int.
        /// </summary>
        /// <param name="inputMessage">
        /// The message gets gets printed to the console before we prompt the user for input.
        /// Default: "> ".
        /// </param>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        /// <returns>The first int from cin.  Null if EOF is reached before we get one.</returns>
        public static int? GetInt( string inputMessage = DefaultCinMessage, TextReader cin = null, TextWriter cout = null )
        {
            BindConsole( ref cin, ref cout );

            cout.Write( inputMessage );

            int? returnValue  = null;
            string line = cin.ReadLine();
            while ( ( line != null ) && ( returnValue == null ) )
            {
                int parsedValue;
                if ( int.TryParse ( line, out parsedValue ) )
                {
                    returnValue = parsedValue;
                }
                else
                {
                    cout.WriteLine( ParseIntErrorMessage );
                    cout.Write( inputMessage );
                    line = cin.ReadLine();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Prompts the user for an uint from the console.  It will keep prompting until
        /// the user provides a valid short.  The function will then return the uint.
        /// </summary>
        /// <param name="inputMessage">
        /// The message gets gets printed to the console before we prompt the user for input.
        /// Default: "> ".
        /// </param>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        /// <returns>The first uint from cin.  Null if EOF is reached before we get one.</returns>
        public static uint? GetUInt( string inputMessage = DefaultCinMessage, TextReader cin = null, TextWriter cout = null )
        {
            BindConsole( ref cin, ref cout );

            cout.Write( inputMessage );

            uint? returnValue  = null;
            string line = cin.ReadLine();
            while ( ( line != null ) && ( returnValue == null ) )
            {
                uint parsedValue;
                if ( uint.TryParse ( line, out parsedValue ) )
                {
                    returnValue = parsedValue;
                }
                else
                {
                    cout.WriteLine( ParseUIntErrorMessage );
                    cout.Write( inputMessage );
                    line = cin.ReadLine();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Prompts the user for a long from the console.  It will keep prompting until
        /// the user provides a valid long.  The function will then return the long.
        /// </summary>
        /// <param name="inputMessage">
        /// The message gets gets printed to the console before we prompt the user for input.
        /// Default: "> ".
        /// </param>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        /// <returns>The first long from cin.  Null if EOF is reached before we get one.</returns>
        public static long? GetLong( string inputMessage = DefaultCinMessage, TextReader cin = null, TextWriter cout = null )
        {
            BindConsole( ref cin, ref cout );

            cout.Write( inputMessage );

            long? returnValue  = null;
            string line = cin.ReadLine();
            while ( ( line != null ) && ( returnValue == null ) )
            {
                long parsedValue;
                if ( long.TryParse ( line, out parsedValue ) )
                {
                    returnValue = parsedValue;
                }
                else
                {
                    cout.WriteLine( ParseLongErrorMessage );
                    cout.Write( inputMessage );
                    line = cin.ReadLine();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Prompts the user for a ulong from the console.  It will keep prompting until
        /// the user provides a valid ulong.  The function will then return the ulong.
        /// </summary>
        /// <param name="inputMessage">
        /// The message gets gets printed to the console before we prompt the user for input.
        /// Default: "> ".
        /// </param>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        /// <returns>The first ulong from cin.  Null if EOF is reached before we get one.</returns>
        public static ulong? GetULong( string inputMessage = DefaultCinMessage, TextReader cin = null, TextWriter cout = null )
        {
            BindConsole( ref cin, ref cout );

            cout.Write( inputMessage );

            ulong? returnValue  = null;
            string line = cin.ReadLine();
            while ( ( line != null ) && ( returnValue == null ) )
            {
                ulong parsedValue;
                if ( ulong.TryParse ( line, out parsedValue ) )
                {
                    returnValue = parsedValue;
                }
                else
                {
                    cout.WriteLine( ParseULongErrorMessage );
                    cout.Write( inputMessage );
                    line = cin.ReadLine();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Prompts the user for a float from the console.  It will keep prompting until
        /// the user provides a valid float.  The function will then return the float.
        /// </summary>
        /// <param name="inputMessage">
        /// The message gets gets printed to the console before we prompt the user for input.
        /// Default: "> ".
        /// </param>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        /// <returns>The first float from cin.  Null if EOF is reached before we get one.</returns>
        public static float? GetFloat( string inputMessage = DefaultCinMessage, TextReader cin = null, TextWriter cout = null )
        {
            BindConsole( ref cin, ref cout );

            cout.Write( inputMessage );

            float? returnValue  = null;
            string line = cin.ReadLine();
            while ( ( line != null ) && ( returnValue == null ) )
            {
                float parsedValue;
                if ( float.TryParse ( line, out parsedValue ) )
                {
                    returnValue = parsedValue;
                }
                else
                {
                    cout.WriteLine( ParseFloatErrorMessage );
                    cout.Write( inputMessage );
                    line = cin.ReadLine();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Prompts the user for a double from the console.  It will keep prompting until
        /// the user provides a valid double.  The function will then return the double.
        /// </summary>
        /// <param name="inputMessage">
        /// The message gets gets printed to the console before we prompt the user for input.
        /// Default: "> ".
        /// </param>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        /// <returns>The first double from cin.  Null if EOF is reached before we get one.</returns>
        public static double? GetDouble( string inputMessage = DefaultCinMessage, TextReader cin = null, TextWriter cout = null )
        {
            BindConsole( ref cin, ref cout );

            cout.Write( inputMessage );

            double? returnValue  = null;
            string line = cin.ReadLine();
            while ( ( line != null ) && ( returnValue == null ) )
            {
                double parsedValue;
                if ( double.TryParse ( line, out parsedValue ) )
                {
                    returnValue = parsedValue;
                }
                else
                {
                    cout.WriteLine( ParseDoubleErrorMessage );
                    cout.Write( inputMessage );
                    line = cin.ReadLine();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Prompts the user for a decimal from the console.  It will keep prompting until
        /// the user provides a valid decimal.  The function will then return the decimal.
        /// </summary>
        /// <param name="inputMessage">
        /// The message gets gets printed to the console before we prompt the user for input.
        /// Default: "> ".
        /// </param>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        /// <returns>The first decimal from cin.  Null if EOF is reached before we get one.</returns>
        public static decimal? GetDecimal( string inputMessage = DefaultCinMessage, TextReader cin = null, TextWriter cout = null )
        {
            BindConsole( ref cin, ref cout );

            cout.Write( inputMessage );

            decimal? returnValue  = null;
            string line = cin.ReadLine();
            while ( ( line != null ) && ( returnValue == null ) )
            {
                decimal parsedValue;
                if ( decimal.TryParse ( line, out parsedValue ) )
                {
                    returnValue = parsedValue;
                }
                else
                {
                    cout.WriteLine( ParseDecimalErrorMessage );
                    cout.Write( inputMessage );
                    line = cin.ReadLine();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Binds the given parameters to the console if they are null.
        /// Otherwise, it leaves them alone.
        /// </summary>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        private static void BindConsole( ref TextReader cin, ref TextWriter cout )
        {
            if ( cin == null )
            {
                cin = Console.In;
            }
            if ( cout == null )
            {
                cout = Console.Out;
            }
        }
    }
}
}

