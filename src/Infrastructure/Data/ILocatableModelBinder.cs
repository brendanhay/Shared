using System;
using System.Web.Mvc;

namespace Infrastructure.Data
{
    public interface ILocatableModelBinder : IModelBinder
    {
        Type Type { get; }
    }
}
