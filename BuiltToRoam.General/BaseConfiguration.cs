﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace BuiltToRoam
{
    public abstract class BaseConfiguration
    {
        protected IDictionary<string,string> Data { get; } = new Dictionary<string,string>();

        protected string Value([CallerMemberName] string propertyName = null)
        {
            return propertyName == null ? null : Data.SafeDictionaryValue<string, string, string>(propertyName);
        }

        protected BaseConfiguration(IDictionary<Expression<Func<string>>, string> initializers=null)
        {
            initializers?.DoForEach(initializer =>
                Data[(initializer.Key.Body as MemberExpression)?.Member.Name] = initializer.Value);

        }
    }

    public interface IConfigurationManager<TConfigurationKey, TConfiguration>
        where TConfigurationKey : struct
        where TConfiguration : BaseConfiguration
    {
        void Populate(IDictionary<TConfigurationKey, TConfiguration> configurations);

        void SelectConfiguration(TConfigurationKey key);

        TConfiguration Current { get; }
    }
}
