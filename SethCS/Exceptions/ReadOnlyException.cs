using System;
using System.Runtime.Serialization;

namespace SethCS.Exceptions
{
    /// <summary>
    /// Exception that is thrown when something was being written
    /// to when it shouldn't have been.
    /// </summary>
    public class ReadOnlyException : Exception
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ReadOnlyException()
            : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message that describes the error.</param>
        public ReadOnlyException( string message ) :
            base( message )
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public ReadOnlyException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message that describes the error.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public ReadOnlyException( string message, Exception innerException ) :
            base( message, innerException )
        {
        }
    }
}
