using System.Text;

namespace Infrastructure.Razor
{
    /// <summary>
    /// Provides a base implementation of a template.
    /// </summary>
    public abstract class TemplateBase : ITemplate
    {
        private readonly StringBuilder _builder = new StringBuilder();

        /// <summary>
        /// Gets the parsed result of the template.
        /// </summary>
        public string Result { get { return _builder.ToString(); } }

        /// <summary>
        /// Clears the template.
        /// </summary>
        public void Clear()
        {
            _builder.Clear();
        }

        /// <summary>
        /// Executes the template.
        /// </summary>
        public virtual void Execute() { }

        /// <summary>
        /// Writes the specified object to the template.
        /// </summary>
        /// <param name="object"></param>
        public void Write(object @object)
        {
            if (@object == null)
                return;

            _builder.Append(@object);
        }

        /// <summary>
        /// Writes a literal to the template.
        /// </summary>
        /// <param name="literal"></param>
        public void WriteLiteral(string literal)
        {
            if (literal == null)
                return;

            _builder.Append(literal);
        }
    }

    /// <summary>
    /// Provides a base implementation of a template.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    public abstract class TemplateBase<TModel> : TemplateBase, ITemplate<TModel>
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public TModel Model { get; set; }

        //public TModel ModifiedModel { get; set; }
    }
}
