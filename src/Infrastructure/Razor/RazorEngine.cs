using System;
using System.Collections.Generic;

namespace Infrastructure.Razor
{
    /// <summary>
    /// Process razor templates.
    /// </summary>
    public static class RazorEngine
    {
        private static RazorCompiler Compiler;
        private static readonly IDictionary<string, ITemplate> Templates;
        private static readonly object _lock = new object();

        /// <summary>
        /// Statically initialises the <see cref="Razor"/> type.
        /// </summary>
        static RazorEngine()
        {
            Compiler = new RazorCompiler(new CSharpRazorProvider());
            Templates = new Dictionary<string, ITemplate>();
        }

        /// <summary>
        /// Gets an <see cref="ITemplate"/> for the specified template.
        /// </summary>
        /// <param name="template">The template to parse.</param>
        /// <param name="modelType">The model to use in the template.</param>
        /// <param name="name">[Optional] The name of the template.</param>
        /// <returns></returns>
        private static ITemplate GetTemplate(string template, Type modelType, string name = null)
        {
            lock (_lock) {
                var instance = string.IsNullOrEmpty(name) || !Templates.ContainsKey(name)
                        ? Compiler.CreateTemplate(template, modelType)
                        : Templates[name];

                if (instance == null) {
                    throw new InvalidOperationException("ITemplate cannot be null. Check compilation steps.");
                }

                if (!string.IsNullOrEmpty(name) && !Templates.ContainsKey(name)) {
                    Templates.Add(name, instance);
                }

                return instance;
            }
        }

        /// <summary>
        /// Parses the specified template using the specified model.
        /// </summary>
        /// <typeparam name="T">The model type.</typeparam>
        /// <param name="template">The template to parse.</param>
        /// <param name="model">The model to use in the template. Passed by reference, and any modifiers in the template will propogate.</param>
        /// <param name="name">[Optional] A name for the template used for caching.</param>
        /// <param name="strip">[Optional] Whether to strip the Environment.Newline string from the result.</param>
        /// <returns>The parsed template.</returns>
        public static string Parse<T>(string template, ref T model, string name = null, bool strip = false)
        {
            var instance = GetTemplate(template, typeof(T), name);
            var hasModel = instance is ITemplate<T>;

            // Stick it in ..
            if (hasModel) {
                ((ITemplate<T>)instance).Model = model;
            }

            instance.Execute();

            // Pull it out .. and re-assign to the reference
            if (hasModel) {
                model = ((ITemplate<T>)instance).Model;
            }

            return strip
                ? instance.Result.Replace(Environment.NewLine, "")
                : instance.Result;
        }

        /// <summary>
        /// Sets the razor provider used for compiling templates.
        /// </summary>
        /// <param name="provider">The razor provider.</param>
        public static void SetRazorProvider(IRazorProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            Compiler = new RazorCompiler(provider);
        }
    }
}