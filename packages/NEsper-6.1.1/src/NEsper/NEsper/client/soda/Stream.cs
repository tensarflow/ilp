///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;

namespace com.espertech.esper.client.soda
{
    /// <summary>
    /// An abstract base class for a named or unnamed stream.
    /// <para />
    /// Named streams provide an as-name for the stream, for example "select * from MyEvents(id=10) as StreamZero".
    /// Unnamed streams provide no as-name for the stream, for example "select * from MyEvents(id=10)".
    /// </summary>
    [Serializable]
    public abstract class Stream
    {
        /// <summary>Ctor. </summary>
        protected Stream()
        {
        }

        /// <summary>Renders the stream in textual representation. </summary>
        /// <param name="writer">to output to</param>
        /// <param name="formatter">for NewLine-whitespace formatting</param>
        public abstract void ToEPLStream(TextWriter writer, EPStatementFormatter formatter);

        /// <summary>Renders the stream in textual representation any stream options, if present. </summary>
        /// <param name="writer">to output to</param>
        public abstract void ToEPLStreamOptions(TextWriter writer);

        /// <summary>Renders the stream type under a non-complete textual representation for tool use </summary>
        /// <param name="writer">to output to</param>
        public abstract void ToEPLStreamType(TextWriter writer);

        /// <summary>Ctor. </summary>
        /// <param name="streamName">is null for unnamed streams, or a stream name for named streams.</param>
        protected Stream(String streamName)
        {
            StreamName = streamName;
        }

        /// <summary>Returns the stream name. </summary>
        /// <value>name of stream, or null if unnamed.</value>
        public string StreamName { get; set; }

        /// <summary>Renders the clause in textual representation. </summary>
        /// <param name="writer">to output to</param>
        /// <param name="formatter">for NewLine-whitespace formatting</param>
        public void ToEPL(TextWriter writer, EPStatementFormatter formatter)
        {
            ToEPLStream(writer, formatter);

            if (StreamName != null)
            {
                writer.Write(" as ");
                writer.Write(StreamName);
            }

            ToEPLStreamOptions(writer);
        }
    }
}
