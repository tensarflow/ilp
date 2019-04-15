///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

namespace com.espertech.esper.core.service
{
	public class EPRuntimeIsolatedFactoryImpl : EPRuntimeIsolatedFactory
	{
	    public EPRuntimeIsolatedSPI Make(EPIsolationUnitServices isolatedServices, EPServicesContext unisolatedSvc)
        {
	        return new EPRuntimeIsolatedImpl(isolatedServices, unisolatedSvc);
	    }
	}
} // end of namespace
