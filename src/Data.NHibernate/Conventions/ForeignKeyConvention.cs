using System;
using FluentNHibernate;
using Infrastructure;

namespace Data.NHibernate.Conventions
{
    public class ForeignKeyConvention : FluentNHibernate.Conventions.ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            return Inflector.Underscore((property == null
                ? type : property.DeclaringType).Name) + "_id";
        }
    }
}
