using System;
using System.Runtime.Serialization;

namespace SethCS.Exceptions
{
    /// <summary>
    /// Exception that is thrown when something isn't valid.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ValidationException()
            : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message that describes the error.</param>
        public ValidationException( string message ) :
            base( message )
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public ValidationException( SerializationInfo info, StreamingContext context )
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
        public ValidationException( string message, Exception innerException ) :
            base( message, innerException )
        {
        }
    }
}