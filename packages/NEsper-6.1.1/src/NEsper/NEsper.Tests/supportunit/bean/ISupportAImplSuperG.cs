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
	public abstract class ISupportAImplSuperG : ISupportA
	{
		public abstract String G {get;}
		public abstract String A {get;}
		public abstract String BaseAB {get;}
	}
}
