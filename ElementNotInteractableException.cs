﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JABAutomation
{
    /// <summary>
    /// The exception that is thrown when an element is not visible.
    /// </summary>
    [Serializable]
    public class ElementNotInteractableException : InvalidElementStateException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementNotInteractableException"/> class.
        /// </summary>
        public ElementNotInteractableException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementNotInteractableException"/> class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ElementNotInteractableException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementNotInteractableException"/> class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or <see langword="null"/> if no inner exception is specified.</param>
        public ElementNotInteractableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementNotInteractableException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized
        /// object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual
        /// information about the source or destination.</param>
        protected ElementNotInteractableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
