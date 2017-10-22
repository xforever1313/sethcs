//
//          Copyright Seth Hendrick 2017.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SethCS.IO
{
    /// <summary>
    /// This class downloads the string of a given URL and returns it.
    /// </summary>
    public static class HttpGetter
    {
        // ----------------- Fields -----------------

        public const int DefaultTimeout = 30 * 1000;

        public const string DefaultUserAgent = "SethCS HttpGetter";

        // ----------------- Constructor -----------------

        /// <summary>
        /// Downloads the string content from the given URL.
        /// Encoded as UTF-8... This also means that downloading binaries will give you garbage.
        /// </summary>
        /// <param name="url">The URL to download from.</param>
        /// <param name="userAgent">The user agent to use, if null we use the default.</param>
        /// <param name="timeout">How long to wait before giving up.  Default is 30 seconds.</param>
        /// <returns>The string that was downloaded.</returns>
        public static string DownloadString( string url, string userAgent = DefaultUserAgent, int timeout = DefaultTimeout )
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create( url );
            request.Method = "GET";
            request.Timeout = timeout;
            request.UserAgent = userAgent ?? "SethCS HttpGetter";

            string str;

            using( HttpWebResponse response = (HttpWebResponse)request.GetResponse() )
            {
                if( response.StatusCode != HttpStatusCode.OK )
                {
                    throw new WebException(
                        "Did not get OK status code.  Got " + response.StatusCode + " " + response.StatusDescription
                    );
                }

                using( StreamReader reader = new StreamReader( response.GetResponseStream(), Encoding.UTF8 ) )
                {
                    str = reader.ReadToEnd();
                }
            }

            return str;
        }

        /// <summary>
        /// Downloads the string content from the given URL in a background thread.
        /// </summary>
        /// <param name="url">The URL to download from.</param>
        /// <param name="userAgent">The user agent to use, if null we use the default.</param>
        /// <param name="timeout">How long to wait before giving up.</param>
        /// <returns>The string that was downloaded.</returns>
        public static Task<string> AsyncDownloadString( string url, string userAgent = DefaultUserAgent, int timeout = DefaultTimeout )
        {
            return Task.Run( () => DownloadString( url, userAgent, timeout ) );
        }
    }
}
