//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo( "Tests" )]
[assembly: InternalsVisibleTo( "TestsMono" )]

namespace SethCS.IO
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
        /// The default message that is printed to the console
        /// when asking the user for information.
        /// </summary>
        internal const string DefaultListPromptMessage = "Select a number and press enter:";

        /// <summary>
        /// Error message that appears when the user selects a prompt option not listed.
        /// </summary>
        internal const string ListPromptOutOfRangeMessage = "Option out of range.  Try again.";

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

        // -------- Functions --------

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

            bool? returnValue = null;
            string line = cin.ReadLine();
            while( ( line != null ) && ( returnValue == null ) )
            {
                bool parsedValue;
                if( bool.TryParse( line, out parsedValue ) )
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

            short? returnValue = null;
            string line = cin.ReadLine();
            while( ( line != null ) && ( returnValue == null ) )
            {
                short parsedValue;
                if( short.TryParse( line, out parsedValue ) )
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

            ushort? returnValue = null;
            string line = cin.ReadLine();
            while( ( line != null ) && ( returnValue == null ) )
            {
                ushort parsedValue;
                if( ushort.TryParse( line, out parsedValue ) )
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

            int? returnValue = null;
            string line = cin.ReadLine();
            while( ( line != null ) && ( returnValue == null ) )
            {
                int parsedValue;
                if( int.TryParse( line, out parsedValue ) )
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

            uint? returnValue = null;
            string line = cin.ReadLine();
            while( ( line != null ) && ( returnValue == null ) )
            {
                uint parsedValue;
                if( uint.TryParse( line, out parsedValue ) )
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

            long? returnValue = null;
            string line = cin.ReadLine();
            while( ( line != null ) && ( returnValue == null ) )
            {
                long parsedValue;
                if( long.TryParse( line, out parsedValue ) )
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

            ulong? returnValue = null;
            string line = cin.ReadLine();
            while( ( line != null ) && ( returnValue == null ) )
            {
                ulong parsedValue;
                if( ulong.TryParse( line, out parsedValue ) )
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

            float? returnValue = null;
            string line = cin.ReadLine();
            while( ( line != null ) && ( returnValue == null ) )
            {
                float parsedValue;
                if( float.TryParse( line, out parsedValue ) )
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

            double? returnValue = null;
            string line = cin.ReadLine();
            while( ( line != null ) && ( returnValue == null ) )
            {
                double parsedValue;
                if( double.TryParse( line, out parsedValue ) )
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

            decimal? returnValue = null;
            string line = cin.ReadLine();
            while( ( line != null ) && ( returnValue == null ) )
            {
                decimal parsedValue;
                if( decimal.TryParse( line, out parsedValue ) )
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

        // ---- Prompt Helpers ----

        /// <summary>
        /// Displays a prompt of options to the user.  The user will then
        /// provide a number of the option they want.  The user will keep
        /// being prompted until they give a valid option, or EOF.
        ///
        /// If endWith0 is true, the result is this:
        ///
        /// firstMessage
        /// 1. Option 1
        /// 2. Option 2
        /// ...
        /// x. Option x
        /// 0. Option 0
        /// >
        ///
        /// Otherwise, its this:
        ///
        /// firstMessage
        /// 0. Option 0
        /// 1. Option 1
        /// ...
        /// x. Option x
        /// >
        /// </summary>
        /// <param name="options">List of options to be shown.</param>
        /// <param name="endWith0">Whether or not we end with option zero or start with option 0.</param>
        /// <param name="firstMessage">The message to appear before the options are listed.</param>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        /// <returns>The number the user selected.  Null if EOF.</returns>
        public static int? ShowListPrompt(
            IList<string> options,
            bool endWith0,
            string firstMessage = DefaultListPromptMessage,
            TextReader cin = null,
            TextWriter cout = null
        )
        {
            // -- Function checks --
            if( options == null )
            {
                throw new ArgumentNullException( nameof( options ) );
            }
            else if( options.Count == 0 )
            {
                throw new ArgumentException(
                    nameof( options ) + " must have at least one option.",
                    nameof( options )
                );
            }

            // -- Build Message --
            string promptMessage = string.Empty;

            for( int i = 1; i < options.Count; ++i )
            {
                promptMessage += i + ".\t" + options[i] + Environment.NewLine;
            }

            if( endWith0 )
            {
                // If we end with 0, append it to the end.
                promptMessage += "0.\t" + options[0] + Environment.NewLine;
            }
            else
            {
                // Otherwise, prepend it.
                promptMessage = "0.\t" + options[0] + Environment.NewLine + promptMessage;
            }

            promptMessage = firstMessage + Environment.NewLine + promptMessage + "> ";

            // -- Get selection --
            int selection = -1;

            // Ensure our selection is valid.
            while( ( selection < 0 ) || ( selection >= options.Count ) )
            {
                int? userSelection = GetInt( promptMessage, cin, cout );

                // Bail if we reached EOF.
                if( userSelection == null )
                {
                    return null;
                }

                selection = userSelection.Value;
                if( ( selection < 0 ) || ( selection >= options.Count ) )
                {
                    cout.WriteLine( ListPromptOutOfRangeMessage );
                }
            }
            return selection;
        }

        // ---- Helper Functions ----

        /// <summary>
        /// Binds the given parameters to the console if they are null.
        /// Otherwise, it leaves them alone.
        /// </summary>
        /// <param name="cin">Console In.  If null, this becomes Console.In</param>
        /// <param name="cout">Console Out.  If null, this becomes Console.Out</param>
        private static void BindConsole( ref TextReader cin, ref TextWriter cout )
        {
            // If we are Windows Universal, bail.  Those platforms do not support Console In or Console Out.
#if NETFX_CORE
            if ( cin == null )
            {
                throw new PlatformNotSupportedException( "Windows Universal Apps do not support Console.In" );
            }
            if ( cout == null )
            {
                throw new PlatformNotSupportedException( "Windows Universal Apps do not support Console.Out" );
            }
#else
            if( cin == null )
            {
                cin = Console.In;
            }
            if( cout == null )
            {
                cout = Console.Out;
            }
#endif
        }
    }
}