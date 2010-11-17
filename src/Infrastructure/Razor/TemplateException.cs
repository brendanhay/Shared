using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace Infrastructure.Razor
{
    /// <summary>
    /// Defines an exception that occurs during compilation of a template.
    /// </summary>
    public class TemplateException : Exception
    {
        private readonly string _message;

        /// <summary>
        /// Initialises a new instance of <see cref="TemplateException"/>
        /// </summary>
        /// <param name="errors">The collection of compilation errors.</param>
        public TemplateException(CompilerErrorCollection errors)
            : base("Unable to compile template.")
        {
            var list = new List<CompilerError>();
            var builder = new StringBuilder();

            foreach (CompilerError error in errors) {
                list.Add(error);
                builder.AppendFormat("Error: {0}, Line: {1}, Column: {2}, File: {3}.{4}",
                    error.ErrorText, error.Column, error.Line, error.FileName, Environment.NewLine);
            }

            Errors = new ReadOnlyCollection<CompilerError>(list);
            _message = builder.ToString();
        }

        public override string Message { get { return _message; } }

        /// <summary>
        /// Gets the collection of compiler errors.
        /// </summary>
        public ReadOnlyCollection<CompilerError> Errors { get; private set; }
    }
}