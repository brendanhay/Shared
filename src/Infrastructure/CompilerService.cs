using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Infrastructure.Razor;
using Microsoft.CSharp;

namespace Infrastructure
{
    public interface ICompilerService
    {
        IDictionary<string, Type> Compile(CodeNamespace @namespace, params Type[] imports);

        CodeNamespace CreateNamespace(object parent, params Type[] imports);

        CodeTypeDeclaration CreateClass(CodeNamespace @namespace, string className, object parent);

        CodeTypeDeclaration CreateClass(CodeNamespace @namespace, string className, object parent,
            Action<CodeTypeDeclaration> attributeFactory);

        void AddProperty(CodeTypeDeclaration @class, string name, Type type,
            Action<CodeTypeMember> attributeFactory);

        void AddProperty(CodeTypeDeclaration @class, string name, string typeName,
            Action<CodeTypeMember> attributeFactory);

        void AddAttribute(CodeTypeMember member, string attributeType, string expression = null);
    }

    internal sealed class CompilerService : ICompilerService
    {
        public IDictionary<string, Type> Compile(CodeNamespace @namespace, params Type[] imports)
        {
            var unit = new CodeCompileUnit();
            unit.Namespaces.Add(@namespace);

            var parameters = new CompilerParameters {
                GenerateInMemory = true,
                IncludeDebugInformation = true,
                GenerateExecutable = false,
                CompilerOptions = "/target:library /optimize",
                WarningLevel = 4
            };

            parameters.ReferencedAssemblies.Add("mscorlib.dll");

            foreach (var import in imports) {
                parameters.ReferencedAssemblies.Add(import.Assembly.Location);
            }

            var codeProvider = new CSharpCodeProvider();

#if DEBUG
            string code;

            using (var writer = new StringWriter()) {
                codeProvider.GenerateCodeFromCompileUnit(unit, writer, new CodeGeneratorOptions());

                code = writer.GetStringBuilder().ToString();
            }
#endif

            var results = codeProvider.CompileAssemblyFromDom(parameters, unit);

            if (results.Errors.HasErrors) {
                throw new TemplateException(results.Errors);
            }

            var types = results.CompiledAssembly.GetExportedTypes();

            return types.ToDictionary(type => type.Name, type => type);
        }

        public CodeNamespace CreateNamespace(object parent, params Type[] imports)
        {
            // Create a namespace
            var @namespace = new CodeNamespace(parent.GetType().Namespace + ".Generated");

            foreach (var import in imports) {
                @namespace.Imports.Add(new CodeNamespaceImport(import.Namespace));
            }

            return @namespace;
        }

        public CodeTypeDeclaration CreateClass(CodeNamespace @namespace, string className,
            object parent)
        {
            return CreateClass(@namespace, className, parent, @class => NOOP());
        }

        public CodeTypeDeclaration CreateClass(CodeNamespace @namespace, string className,
            object parent, Action<CodeTypeDeclaration> attributeFactory)
        {
            // Create a class
            var @class = new CodeTypeDeclaration(className);
            @class.BaseTypes.Add(parent.GetType());

            // Add any class level attributes
            attributeFactory(@class);

            // Add it to the namespace
            @namespace.Types.Add(@class);

            return @class;
        }

        public void AddProperty(CodeTypeDeclaration @class, string name, Type type,
            Action<CodeTypeMember> attributeFactory)
        {
            AddProperty(@class, name, type.FullName, attributeFactory);
        }

        public void AddProperty(CodeTypeDeclaration @class, string name, string typeName,
            Action<CodeTypeMember> attributeFactory)
        {
            var autoPropertyName = string.Format("{0} {{ get; set; }} //", name);
            var property = new CodeMemberField {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = autoPropertyName,
                Type = new CodeTypeReference(typeName),
            };

            // Add the attributes
            attributeFactory(property);

            // Add the property to the class
            @class.Members.Add(property);
        }

        public void AddAttribute(CodeTypeMember member, string attributeType,
            string expression = null)
        {
            // Add [attributeType(expression)]
            var argument = new CodeAttributeArgument(new CodeSnippetExpression(expression));
            var attribute = new CodeAttributeDeclaration(attributeType, argument);

            member.CustomAttributes.Add(attribute);
        }

        private static void NOOP() { }
    }
}
