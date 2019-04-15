///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////


using System;
using com.espertech.esper.client;

namespace com.espertech.esper.events.vaevent
{
    /// <summary>
    /// Strategy for resolving a property against any of the variant types.
    /// </summary>
    public interface VariantPropResolutionStrategy
    {
        /// <summary>
        /// Resolve the property for each of the types.
        /// </summary>
        /// <param name="propertyName">to resolve</param>
        /// <param name="variants">the variants to resolve the property for</param>
        /// <returns>property descriptor</returns>
        VariantPropertyDesc ResolveProperty(String propertyName, EventType[] variants);
    }
}
