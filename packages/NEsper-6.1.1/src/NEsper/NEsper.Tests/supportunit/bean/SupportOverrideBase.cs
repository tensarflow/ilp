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
    [Serializable]
    public class SupportOverrideBase : SupportMarkerInterface
	{
		public virtual String Val
		{
			get { return _val; }
		}

        private readonly String _val;
		
		public SupportOverrideBase(String val)
		{
			this._val = val;
		}
	}
}
