﻿#region License
// Copyright (c) 2021, FluentMigrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;

using FluentMigrator.Builders.Insert;
using FluentMigrator.Infrastructure;
using FluentMigrator.Infrastructure.Extensions;

namespace FluentMigrator.Postgres
{
    public static partial class PostgresExtensions
    {
        public const string OverridingIdentityValues = "PostgresOverridingSystemValue";

        /// <summary>
        /// Adds an OVERRIDING SYSTEM VALUE clause in the current <see cref="IInsertDataSyntax"/> expression.
        /// This enables the system-generated values to be overriden with the user-specified explicit values (other than <c>DEFAULT</c>)
        /// for identity columns defined as <c>GENERATED ALWAYS</c>
        /// </summary>
        /// <param name="expression">The current <see cref="IInsertDataSyntax"/> expression</param>
        /// <returns>The current <see cref="IInsertDataSyntax"/> expression</returns>
        public static IInsertDataSyntax WithOverridingSystemValue(this IInsertDataSyntax expression) =>
            SetOverridingIdentityValues(expression, PostgresOverridingIdentityValuesType.System, nameof(WithOverridingSystemValue));

        /// <summary>
        /// Adds an OVERRIDING USER VALUE clause in the current <see cref="IInsertDataSyntax"/> expression.
        /// Any user-specified values will be ignored and the system-generated values will be applied
        /// for identity columns defined as <c>GENERATED BY DEFAULT</c>
        /// </summary>
        /// <param name="expression">The current <see cref="IInsertDataSyntax"/> expression</param>
        /// <returns>The current <see cref="IInsertDataSyntax"/> expression</returns>
        public static IInsertDataSyntax WithOverridingUserValue(this IInsertDataSyntax expression) =>
            SetOverridingIdentityValues(expression, PostgresOverridingIdentityValuesType.User, nameof(WithOverridingUserValue));

        /// <summary>
        /// Set the additional feature for overriding identity values with the specified <see cref="PostgresOverridingIdentityValuesType"/>
        /// on the provided <see cref="IInsertDataSyntax"/> expression
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private static IInsertDataSyntax SetOverridingIdentityValues(
            IInsertDataSyntax expression,
            PostgresOverridingIdentityValuesType overridingType,
            string callingMethod)
        {
            if (!(expression is ISupportAdditionalFeatures castExpression))
            {
                throw new InvalidOperationException(
                    string.Format(
                        ErrorMessages.MethodXMustBeCalledOnObjectImplementingY,
                        callingMethod,
                        nameof(ISupportAdditionalFeatures)));
            }

            castExpression.SetAdditionalFeature(OverridingIdentityValues, overridingType);

            return expression;
        }
    }
}
