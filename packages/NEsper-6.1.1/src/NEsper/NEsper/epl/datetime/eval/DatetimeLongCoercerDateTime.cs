///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

using com.espertech.esper.compat;

namespace com.espertech.esper.epl.datetime.eval
{
    public class DatetimeLongCoercerDateTime : DatetimeLongCoercer
    {
        public long Coerce(Object value)
        {
            return value.AsDateTimeOffset().TimeInMillis();
        }
    }
}