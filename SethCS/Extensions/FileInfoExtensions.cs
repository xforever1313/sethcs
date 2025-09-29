//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.IO;

namespace SethCS.Extensions
{
    public static class FileInfoExtensions
    {
        // ---------------- Methods ----------------

        public static string GetMimeType( this string path )
        {
            return GetMimeType( new FileInfo( path ) );
        }

        public static string GetMimeType( this FileInfo fileInfo )
        {
            // Taken from:
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/MIME_types/Common_types
            string extension = fileInfo.Extension.ToLower();

            return extension switch
            {
                ".acc" => "audio/acc", // AAC audio
                ".abw" => "application/x-abiword", // AbiWord document
                ".apng" => "image/apng", // Animated Portable Network Graphics (APNG) image
                ".arc" => "appliation/x-freearc", // Archive document (multiple files embedded)
                ".atom" => "application/atom+xml", // Atom feed
                ".avif" => "image/avif", // AVIF image
                ".avi" => "video/x-msvideo", // AVI: Audio Video Interleave
                ".azw" => "application/vnd.amazon.ebook", // Amazon Kindle eBook format
                ".bmp" => "image/bmp", // Windows OS/2 Bitmap Graphics
                ".bz" => "application/x-bzip", // BZip archive
                ".bz2" => "application/x-bzip2", // BZip2 archive
                ".cda" => "application/x-cdf", // CD audio
                ".csh" => "application/x-csh", // C-Shell script
                ".css" => "text/css", // Cascading Style Sheets (CSS)
                ".csv" => "text/csv", // Comma-separated values (CSV)
                ".doc" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document", // MS Word
                ".eot" => "application/vnd.ms-fontobject", // MS Embedded OpenType fonts
                ".epub" => "application/epub+zip", // Electronic publication (epub).
                ".gz" => "application/gzip", // GZip Compressed Archive
                ".gif" => "image/gif", // Graphics Interchange Format (GIF)
                ".htm" => "text/html", // HyperText Markup Language (HTML)
                ".html" => "text/html",
                ".ico" => "image/vnd.microsoft.icon", // Icon format
                ".ics" => "text/calendar", // ICalendar
                ".jar" => "application/java-archive",
                ".jpg" => "image/jpeg", // JPEG images
                ".jpeg" => "image/jpeg",
                ".js" => "text/javascript", // JavaScript
                ".json" => "application/json", // JSON format
                ".jsonld" => "application/ld+json", // JSON-LD Format
                ".mid" => "audio/midi", // Musical Instrument Digital Interface (MIDI).
                ".midi" => "audio/midi",
                ".mjs" => "text/javascript", // JavaScript module
                ".mp3" => "audio/mpeg", // MP3 audio
                ".mp4" => "video/mp4", // MP4 video
                ".mpeg" => "video/mpeg", // MPEG Video
                ".mpkg" => "application/vnd.apple.installer+xml", // Apple Installer Package
                ".odp" => "application/vnd.oasis.opendocument.presentation", // OpenDocument presentation document
                ".ods" => "application/vnd.oasis.opendocument.spreadsheet", // OpenDocument spreadsheet document
                ".odt" => "application/vnd.oasis.opendocument.text", // OpenDocument text document
                ".oga" => "audio/ogg", // Ogg audio
                ".ogv" => "video/ogg", // Ogg video
                ".ogx" => "application/ogg", // Ogg
                ".opus" => "audio/ogg", // Opus audio in Ogg container
                ".otf" => "font/otf", // OpenType 
                ".png" => "image/png", // Portable Network Graphics
                ".pdf" => "application/pdf", // Adobe Portable Document Format (PDF)
                ".php" => "application/x-httpd-php", // Hypertext Preprocessor (Personal Home Page)
                ".ppt" => "application/vnd.ms-powerpoint", // MS PowerPoint,
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation", // Microsoft PowerPoint (OpenXML)
                ".rar" => "application/vnd.rar", // RAR archive
                ".rtf" => "application/rtf", // Rich Text Format (RTF)
                ".sh" => "application/x-sh", // Bourne shell script
                ".svg" => "image/svg+xml", // Scalable Vector Graphics (SVG)
                ".tar" => "application/x-tar", // Tape Archive (TAR)
                ".text" => "text/plain",
                ".tif" => "image/tiff", // Tagged Image File Format (TIFF)
                ".tiff" => "image/tiff",
                ".ts" => "video/mp2t", // MPEG transport stream
                ".ttf" => "font/ttf", // TrueType Font
                ".txt" => "text/plain",
                ".vsd" => "application/vnd.visio", // Microsoft Visio
                ".wav" => "audio/wav", // Waveform Audio Format
                ".weba" => "audio/webm", // WEBM audio
                ".webm" => "video/webm", // WEBM video
                ".webp" => "image/webp", // WEBP image
                ".woff" => "font/woff", // Web Open Font Format (WOFF)
                ".woff2" => "font/woff2", // Web Open Font Format (WOFF)
                ".xhtml" => "application/xhtml+xml", // XHTML
                ".xls" => "application/vnd.ms-excel", // MS Excel
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", // MS Excel (OpenXml)
                ".xml" => "application/xml", // XML
                ".xul" => "application/vnd.mozilla.xul+xml", // XUL
                ".zip" => "application/zip", // ZIP archive
                ".3gp" => "video/3gpp", // 3GPP Audio/Video container
                ".3g2" => "video/3gpp2", // 3GPP2 Audio/Video container
                ".7z" => "application/x-7z-compressed", // 7-zip archive
                _ => "application/octet-stream"
            };
        }
    }
}
