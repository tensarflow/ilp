///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

namespace com.espertech.esper.supportunit.bean
{
    public class SupportTimeStartEndA : SupportTimeStartBase
    {
        public SupportTimeStartEndA(String key, String datestr, long duration)
            : base(key, datestr, duration)
        {
        }

        public static SupportTimeStartEndA Make(String key, String datestr, long duration)
        {
            return new SupportTimeStartEndA(key, datestr, duration);
        }
    }
}