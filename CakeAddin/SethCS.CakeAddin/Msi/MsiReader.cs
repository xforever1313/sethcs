//
//          Copyright Seth Hendrick 2019.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.IO;
using System.Text;

namespace SethCS.CakeAddin.Msi
{
    /// <summary>
    /// Reads information from an MSI.
    /// </summary>
    public interface IMsiReader
    {
        /// <summary>
        /// Opens the given MSI, reads the version of it and returns it as a string.
        /// </summary>
        /// <param name="MsiPath">Path to the MSI.</param>
        /// <returns>The version of the MSI as a string.</returns>
        string ReadVersion( string msiPath );
    }

    public class MsiReader : IMsiReader
    {
        // ---------------- Fields ----------------

        private readonly IMsiNativeMethods native;

        // ---------------- Constructor ----------------

        public MsiReader() :
            this( new MsiNativeMethods() )
        {
        }

        internal MsiReader( IMsiNativeMethods native )
        {
            this.native = native;
        }

        // ---------------- Functions ----------------

        public string ReadVersion( string msiPath )
        {
            // Big thanks to https://stackoverflow.com/questions/4347325/checking-productversion-of-an-msi-programmatically
            // for inspiration.

            const string sql = "SELECT * FROM Property WHERE Property = 'ProductVersion'";

            IntPtr databasePointer = IntPtr.Zero;
            IntPtr viewPointer = IntPtr.Zero;
            IntPtr recordHandle = IntPtr.Zero;

            StringBuilder buffer = new StringBuilder();
            int bufferSize = 255;

            if( File.Exists( msiPath ) == false )
            {
                throw new FileNotFoundException( "Can not find MSI at: " + msiPath );
            }

            try
            {
                // Open File.
                this.native.NativeMsiOpenDatabase( msiPath, IntPtr.Zero, out databasePointer );
                recordHandle = this.native.NativeMsiCreateRecord( 1 );

                // Open property table, and execute SQL.
                this.native.NativeMsiDatabaseOpenViewW( databasePointer, sql, out viewPointer );
                this.native.NativeMsiViewExecute( viewPointer, recordHandle );

                this.native.NativeMsiCloseHandle( recordHandle );
                recordHandle = IntPtr.Zero;

                // Get the view fromthe record
                this.native.NativeMsiViewFetch( viewPointer, out recordHandle ); // Do we need to create a new recordHandle?

                // Get the string
                this.native.NativeMsiRecordGetString( recordHandle, 2, buffer, ref bufferSize );
            }
            finally
            {
                // Free our resources.
                this.native.NativeMsiCloseHandle( databasePointer );
                this.native.NativeMsiCloseHandle( viewPointer );
                this.native.NativeMsiCloseHandle( recordHandle );
            }

            return buffer.ToString();
        }
    }
}
