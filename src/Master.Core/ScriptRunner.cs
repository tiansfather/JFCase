using Master.Module;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Master
{
    public class ScriptRunner
    {
        public static async Task<object> EvaluateScript(string script, ScriptOptions option = null, object globals = null, Type globalsType = null)
        {
            var op = option ?? ScriptOptions.Default;
            op = op.AddImports(new string[] { "System", "System.Math" });
            op = op.AddReferences(typeof(ModuleInfo).Assembly);
            var result = await CSharpScript.EvaluateAsync(script, op, globals, globalsType);
            return result;
        }
        public static async Task<T> EvaluateScript<T>(string script, ScriptOptions option = null, object globals = null, Type globalsType = null)
        {
            var op = option ?? ScriptOptions.Default;
            op = op.AddImports(new string[] { "System", "System.Math" });
            op = op.AddReferences(typeof(ModuleInfo).Assembly);
            var result = await CSharpScript.EvaluateAsync<T>(script, op, globals, globalsType);
            return result;
        }

        public static Type CompileType(string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var type = CompileType("GenericGenerator", syntaxTree);

            return type;
        }


        private static Type CompileType(string originalClassName, SyntaxTree syntaxTree)
        {
            // 指定编译选项。
            var assemblyName = $"{originalClassName}.g";
            var compilation = CSharpCompilation.Create(assemblyName, new[] { syntaxTree },
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(
                    // 这算是偷懒了吗？我把 .NET Core 运行时用到的那些引用都加入到引用了。
                    // 加入引用是必要的，不然连 object 类型都是没有的，肯定编译不通过。
                    AppDomain.CurrentDomain.GetAssemblies().Select(x => MetadataReference.CreateFromFile(x.Location)));

            // 编译到内存流中。
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());
                    return assembly.GetTypes().First(x => x.Name == originalClassName);
                }
                throw new CompilationErrorException("编译错误",result.Diagnostics);
            }
        }
    }
}
