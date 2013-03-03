using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security;

namespace Xunit.Sdk
{
    /// <summary>
    /// Exception thrown when a value is unexpectedly not in the given range.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [Serializable]
    public class InRangeException : AssertException
    {
        readonly string actual;
        readonly string high;
        readonly string low;

        /// <summary>
        /// Creates a new instance of the <see cref="InRangeException"/> class.
        /// </summary>
        /// <param name="actual">The actual object value</param>
        /// <param name="low">The low value of the range</param>
        /// <param name="high">The high value of the range</param>
        public InRangeException(object actual, object low, object high)
            : base("Assert.InRange() Failure")
        {
            this.low = low == null ? null : low.ToString();
            this.high = high == null ? null : high.ToString();
            this.actual = actual == null ? null : actual.ToString();
        }

        /// <inheritdoc/>
        protected InRangeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            actual = info.GetString("Actual");
            high = info.GetString("High");
            low = info.GetString("Low");
        }

        /// <summary>
        /// Gets the actual object value
        /// </summary>
        public string Actual
        {
            get { return actual; }
        }

        /// <summary>
        /// Gets the high value of the range
        /// </summary>
        public string High
        {
            get { return high; }
        }

        /// <summary>
        /// Gets the low value of the range
        /// </summary>
        public string Low
        {
            get { return low; }
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                return String.Format(CultureInfo.CurrentCulture,
                                     "{0}\r\nRange:  ({1} - {2})\r\nActual: {3}",
                                     base.Message, Low, High, Actual ?? "(null)");
            }
        }

        /// <inheritdoc/>
        [SecurityCritical]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Protected with the Guard class")]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.ArgumentNotNull("info", info);

            info.AddValue("Actual", actual);
            info.AddValue("High", high);
            info.AddValue("Low", low);

            base.GetObjectData(info, context);
        }
    }
}