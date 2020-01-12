//
//          Copyright Seth Hendrick 2019.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SethCS.CakeAddin.Msi
{
    internal interface IMsiNativeMethods
    {
        /// <summary>
        /// Opens a database file for data access.
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/windows/win32/api/msiquery/nf-msiquery-msiopendatabasea"/>
        /// <param name="szDatabasePath">Path to the MSI file</param>
        /// <param name="szPersist">How it the database should be opened.</param>
        /// <param name="phDatabase">
        /// Pointer to the database.  This is a 32-bit pointer.  Should be closed with <see cref="MsiCloseHandle(IntPtr)"/>.
        /// </param>
        uint NativeMsiOpenDatabase( string szDatabasePath, IntPtr szPersist, out IntPtr phDatabase );

        /// <summary>
        /// Prepares a database query and creates an object view.
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/windows/win32/api/msiquery/nf-msiquery-msidatabaseopenvieww"/>
        /// <param name="hDatabase">
        /// Handle to the database to which we want to open a view object.
        /// Use <see cref="MsiCreateRecord"/> to create this handle.
        /// </param>
        /// <param name="szQuery">SQL query for querying the database.</param>
        /// <param name="phView">Pointer to a handler to the view returned.  Should be cleaned up with <see cref="MsiCloseHandle"/></param>
        int NativeMsiDatabaseOpenViewW( IntPtr hDatabase, [MarshalAs( UnmanagedType.LPWStr )] string szQuery, out IntPtr phView );

        /// <summary>
        /// Creates a new record object with the specified number of fields.
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/windows/win32/api/msiquery/nf-msiquery-msicreaterecord"/>
        /// <param name="cParams">
        /// Specifies the number of fields the record will have. The maximum number of fields in a record is limited to 65535.
        /// </param>
        /// <returns>
        /// A handle that should be closed with <see cref="MsiCloseHandle(IntPtr)"/>
        /// If this function fails, this returns null.
        /// </returns>
        IntPtr NativeMsiCreateRecord( uint cParams );

        /// <summary>
        /// Actually executes the SQL view query.
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/windows/win32/api/msiquery/nf-msiquery-msiviewexecute"/>
        /// <param name="hView">Handle to the view to execute the query.</param>
        /// <param name="hRecord">Handle to the record that supplies the parameters.</param>
        /// <returns></returns>
        int NativeMsiViewExecute( IntPtr hView, IntPtr hRecord );

        /// <summary>
        /// Fetches the next sequencital record from the view.
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/windows/win32/api/msiquery/nf-msiquery-msiviewfetch"/>
        /// <param name="hView">Handle to view the fetch from.</param>
        /// <param name="hRecord">Should be closed with <see cref="MsiCloseHandle(IntPtr)"/></param>
        /// <returns></returns>
        uint NativeMsiViewFetch( IntPtr hView, out IntPtr hRecord );

        /// <summary>
        /// Gets the string value of a record field.
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/windows/win32/api/msiquery/nf-msiquery-msirecordgetstringw"/>
        /// <param name="hRecord">Handle to record.</param>
        /// <param name="iField">Requested Field</param>
        /// <param name="szValueBuf">String that contains the data.</param>
        /// <param name="pcchValueBuf">Pointer to the variable that specifies the size of the buffer.</param>
        /// <returns></returns>
        int NativeMsiRecordGetString( IntPtr hRecord, int iField, [Out] StringBuilder szValueBuf, ref int pcchValueBuf );

        /// <summary>
        /// Closes an MSI handle.
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/windows/win32/api/msi/nf-msi-msiclosehandle"/>
        /// <remarks>Apparenlty must be called from the same thread that requested the creation of the handle.</remarks>
        uint NativeMsiCloseHandle( IntPtr hAny );
    }

    internal class MsiNativeMethods : IMsiNativeMethods
    {
        // ---------------- Constructor ----------------

        public MsiNativeMethods()
        {
        }

        // ---------------- Native Functions ----------------

        [DllImport( "msi.dll", SetLastError = true )]
        static extern uint MsiOpenDatabase( string szDatabasePath, IntPtr szPersist, out IntPtr phDatabase );

        [DllImport( "msi.dll", CharSet = CharSet.Unicode )]
        static extern int MsiDatabaseOpenViewW( IntPtr hDatabase, [MarshalAs( UnmanagedType.LPWStr )] string szQuery, out IntPtr phView );

        [DllImport( "msi.dll", ExactSpelling = true )]
        static extern IntPtr MsiCreateRecord( uint cParams );

        [DllImport( "msi.dll", CharSet = CharSet.Unicode )]
        static extern int MsiViewExecute( IntPtr hView, IntPtr hRecord );

        [DllImport( "msi.dll", CharSet = CharSet.Unicode )]
        static extern uint MsiViewFetch( IntPtr hView, out IntPtr hRecord );

        [DllImport( "msi.dll", CharSet = CharSet.Unicode )]
        static extern int MsiRecordGetString( IntPtr hRecord, int iField, [Out] StringBuilder szValueBuf, ref int pcchValueBuf );

        [DllImport( "msi.dll", ExactSpelling = true )]
        static extern uint MsiCloseHandle( IntPtr hAny );

        // ---------------- Functions ----------------

        public uint NativeMsiOpenDatabase( string szDatabasePath, IntPtr szPersist, out IntPtr phDatabase )
        {
            return MsiOpenDatabase( szDatabasePath, szPersist, out phDatabase );
        }

        public int NativeMsiDatabaseOpenViewW( IntPtr hDatabase, [MarshalAs( UnmanagedType.LPWStr )] string szQuery, out IntPtr phView )
        {
            return MsiDatabaseOpenViewW( hDatabase, szQuery, out phView );
        }

        public IntPtr NativeMsiCreateRecord( uint cParams )
        {
            return MsiCreateRecord( cParams );
        }

        public int NativeMsiViewExecute( IntPtr hView, IntPtr hRecord )
        {
            return MsiViewExecute( hView, hRecord );
        }

        public uint NativeMsiViewFetch( IntPtr hView, out IntPtr hRecord )
        {
            return MsiViewFetch( hView, out hRecord );
        }

        public int NativeMsiRecordGetString( IntPtr hRecord, int iField, [Out] StringBuilder szValueBuf, ref int pcchValueBuf )
        {
            return MsiRecordGetString( hRecord, iField, szValueBuf, ref pcchValueBuf );
        }

        public uint NativeMsiCloseHandle( IntPtr hAny )
        {
            return MsiCloseHandle( hAny );
        }
    }
}
