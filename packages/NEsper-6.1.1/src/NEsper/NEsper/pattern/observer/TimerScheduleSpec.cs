///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.client.util;
using com.espertech.esper.compat;

namespace com.espertech.esper.pattern.observer
{
    [Serializable]
    public class TimerScheduleSpec
    {
        public TimerScheduleSpec(
            DateTimeEx optionalDate,
            long? optionalRemainder,
            long? optionalRepeatCount,
            TimePeriod optionalTimePeriod)
        {
            OptionalDate = optionalDate;
            OptionalRepeatCount = optionalRepeatCount;
            OptionalTimePeriod = optionalTimePeriod;
            OptionalRemainder = optionalRemainder;
        }

        public DateTimeEx OptionalDate { get; private set; }

        public long? OptionalRepeatCount { get; private set; }

        public TimePeriod OptionalTimePeriod { get; private set; }

        public long? OptionalRemainder { get; private set; }
    }
} // end of namespace