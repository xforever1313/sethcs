using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SethCS.Basic
{
    /// <summary>
    /// Class that abstracts semantic versioning.
    /// </summary>
    public class SemanticVersion
    {
        // -------- Fields --------

        /// <summary>
        /// The pattern to look for with regexes.
        /// </summary>
        private const string regexPattern = @"(?<major>\d+)\.(?<minor>\d+).(?<rev>\d+)";

        // -------- Constructors --------

        /// <summary>
        /// Constructor.  Sets the version to 0.0.0
        /// </summary>
        public SemanticVersion()
        {
            this.Major = 0;
            this.Minor = 0;
            this.Revision = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="major">The major number in the release.  In X.Y.Z, major is X.</param>
        /// <param name="minor">The minor number in the release.  In X.Y.Z, minor is Y.</param>
        /// <param name="revision">The revision number in the release.  In X.Y.Z, revision is Z.</param>
        public SemanticVersion( int major, int minor, int revision )
        {
            this.Major = major;
            this.Minor = minor;
            this.Revision = revision;
        }

        /// <summary>
        /// Tries to parse the given string to a semantic version.
        /// Versionstring must be X.Y.Z, where X, Y, and Z are ints.
        /// </summary>
        /// <param name="versionString">The string to parse.</param>
        /// <param name="version">The resulting Semantic Version object.  Null if not successful.</param>
        /// <returns>True if parse successful, else false.</returns>
        public static bool TryParse( string versionString, out SemanticVersion version )
        {
            version = null;
            try
            {
                version = SemanticVersion.Parse( versionString );
            }
            catch( Exception )
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Parses the version string with no Exception protection.
        /// Versionstring must be X.Y.Z, where X, Y, and Z are ints.
        /// </summary>
        /// <exception cref="ArgumentNullException">versionString is null.</exception>
        /// <exception cref="FormatException">versionString is not in the correct format.</exception>
        /// <exception cref="OverflowException">If one of the version numbers is bigger than int.MinVale or int.MaxValue</exception>
        /// <param name="versionString">The string to parse.</param>
        /// <returns>The parsed Semantic Verion Object.</returns>
        public static SemanticVersion Parse( string versionString )
        {
            if ( versionString == null )
            {
                throw new ArgumentNullException(
                    "versionString"
                );
            }

            Match match = Regex.Match( versionString, regexPattern );
            if ( match.Success )
            {
                SemanticVersion version = new SemanticVersion(
                    int.Parse( match.Groups["major"].Value as string ),
                    int.Parse( match.Groups["minor"].Value as string ),
                    int.Parse( match.Groups["rev"].Value as string )
                );

                return version;
            }
            else
            {
                throw new FormatException(
                    "Can not parse versionString " + versionString
                );
            }
        }

        // -------- Properties --------

        /// <summary>
        /// The major number in the version.
        /// In X.Y.Z, major is X.
        /// </summary>
        public int Major { get; set; }

        /// <summary>
        /// The minor number in the version.
        /// In X.Y.Z, minor is Y.
        /// </summary>
        public int Minor { get; set; }

        /// <summary>
        /// The revision number in the version.
        /// In X.Y.Z, revision is Z.
        /// </summary>
        public int Revision { get; set; }

        // ------- Functions --------

        /// <summary>
        /// Returns Major.Minor.Revision in string form.
        /// </summary>
        /// <returns>Major.Minor.Revision in string form.</returns>
        public override string ToString()
        {
            return Major.ToString() + "." + Minor.ToString() + "." + Revision.ToString();
        }

        // ---- Operators ----

        /// <summary>
        /// Checks to see if the objects are equal.
        /// </summary>
        /// <param name="obj">The other object to check.</param>
        /// <returns>True if major, minor, and revision numbers match, else false.</returns>
        public override bool Equals( object obj )
        {
            SemanticVersion other = obj as SemanticVersion;
            if ( other == null )
            {
                return false;
            }
            return
                ( this.Major == other.Major ) &&
                ( this.Minor == other.Minor ) &&
                ( this.Revision == other.Revision );
        }

        /// <summary>
        /// Gets the hash code (just uses base.GetHashCode()).
        /// </summary>
        /// <returns>The hashcode.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Checks to see if v1 is an earlier version than v2.
        /// Starting with the Major number, the comparision compares each number in the version.
        /// If the number matches, it moves on to the next one and compares the two.
        /// If the number does not match, it returns the operator%lt; of that number.
        /// </summary>
        /// <param name="v1">The first version to check.</param>
        /// <param name="v2">The second version to check.</param>
        /// <returns>True if v1 is an earlier version than v2.</returns>
        public static bool operator <( SemanticVersion v1, SemanticVersion v2 )
        {
            // Null checks.
            if ( v1 == null ) { throw new ArgumentNullException( "v1" ); }
            if ( v2 == null ) { throw new ArgumentNullException( "v2" ); }

            if ( v1.Major == v2.Major )
            {
                if ( v1.Minor == v2.Minor )
                {
                    return v1.Revision < v2.Revision;
                }
                else
                {
                    return v1.Minor < v2.Minor;
                }
            }
            else
            {
                return v1.Major < v2.Major;
            }
        }

        /// <summary>
        /// Checks to see if v1 is a later version than v2.
        /// Starting with the Major number, the comparision compares each number in the version.
        /// If the number matches, it moves on to the next one and compares the two.
        /// If the number does not match, it returns the operator%gt; of that number.
        /// </summary>
        /// <param name="v1">The first version to check.</param>
        /// <param name="v2">The second version to check.</param>
        /// <returns>True if v1 is a later version than v2.</returns>
        public static bool operator >( SemanticVersion v1, SemanticVersion v2 )
        {
            // Null checks.
            if ( v1 == null ) { throw new ArgumentNullException( "v1" ); }
            if ( v2 == null ) { throw new ArgumentNullException( "v2" ); }

            if ( v1.Major == v2.Major )
            {
                if ( v1.Minor == v2.Minor )
                {
                    return v1.Revision > v2.Revision;
                }
                else
                {
                    return v1.Minor > v2.Minor;
                }
            }
            else
            {
                return v1.Major > v2.Major;
            }
        }

        /// <summary>
        /// Checks to see if v1 is an earlier or the same version as v2.
        /// Starting with the Major number, the comparision compares each number in the version.
        /// If the number matches, it moves on to the next one and compares the two.
        /// If the number does not match, it returns the operator%lt; of that number.
        /// </summary>
        /// <param name="v1">The first version to check.</param>
        /// <param name="v2">The second version to check.</param>
        /// <returns>True if v1 is an earlier version or matches v2.</returns>
        public static bool operator<=( SemanticVersion v1, SemanticVersion v2 )
        {
            return ( v1 < v2 ) || v1.Equals( v2 );
        }

        /// <summary>
        /// Checks to see if v1 is a later or the same version as v2.
        /// Starting with the Major number, the comparision compares each number in the version.
        /// If the number matches, it moves on to the next one and compares the two.
        /// If the number does not match, it returns the operator%lt; of that number.
        /// </summary>
        /// <param name="v1">The first version to check.</param>
        /// <param name="v2">The second version to check.</param>
        /// <returns>True if v1 is a later version or matches v2.</returns>
        public static bool operator >=( SemanticVersion v1, SemanticVersion v2 )
        {
            return ( v1 > v2 ) || v1.Equals( v2 );
        }
    }
}
