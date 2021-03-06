// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Routing.Constraints
{
    /// <summary>
    /// Constraints a route parameter to be an integer within a given range of values.
    /// </summary>
    public class RangeRouteConstraint : IRouteConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangeRouteConstraint" /> class.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <remarks>The minimum value should be less than or equal to the maximum value.</remarks>
        public RangeRouteConstraint(long min, long max)
        {
            if (min > max)
            {
                var errorMessage = Resources.FormatRangeConstraint_MinShouldBeLessThanOrEqualToMax("min", "max");
                throw new ArgumentOutOfRangeException("min", min, errorMessage);
            }

            Min = min;
            Max = max;
        }

        /// <summary>
        /// Gets the minimum allowed value of the route parameter.
        /// </summary>
        public long Min { get; private set; }

        /// <summary>
        /// Gets the maximum allowed value of the route parameter.
        /// </summary>
        public long Max { get; private set; }

        /// <inheritdoc />
        public bool Match([NotNull] HttpContext httpContext,
                          [NotNull] IRouter route,
                          [NotNull] string routeKey,
                          [NotNull] IDictionary<string, object> values,
                          RouteDirection routeDirection)
        {
            object value;
            if (values.TryGetValue(routeKey, out value) && value != null)
            {
                long longValue;
                var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
                if (Int64.TryParse(valueString, NumberStyles.Integer, CultureInfo.InvariantCulture, out longValue))
                {
                    return longValue >= Min && longValue <= Max;
                }
            }

            return false;
        }
    }
}