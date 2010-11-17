﻿using System.CodeDom.Compiler;
using System.Web.Razor;
using Microsoft.CSharp;

namespace Infrastructure.Razor
{
    /// <summary>
    /// Provides a razor provider that supports the C# syntax.
    /// </summary>
    public class CSharpRazorProvider : IRazorProvider
    {
        /// <summary>
        /// Creates a code language service.
        /// </summary>
        /// <returns>Creates a language service.</returns>
        public RazorCodeLanguage CreateLanguageService()
        {
            return new CSharpRazorCodeLanguage();
        }

        /// <summary>
        /// Creates a <see cref="CodeDomProvider"/>.
        /// </summary>
        /// <returns>The a code dom provider.</returns>
        public CodeDomProvider CreateCodeDomProvider()
        {
            return new CSharpCodeProvider();
        }
    }
}