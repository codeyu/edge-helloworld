using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
namespace ScriptingAPISamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var script = @"int Add(int x, int y) 
                            {
                                return x+y;
                            }
                            Add(1, 4)";
            //note: we block here, because we are in Main method, normally we could await as scripting APIs are async
            var resultInt = CSharpScript.EvaluateAsync<int>(script).Result;
            //result is now 5
            Console.WriteLine(resultInt);
            var code = @"public class Hello
                        {
                            public string HelloWorld()
                            {
                                return ""Hello World"";
                            }
                        }
                        new Hello().HelloWorld()";
            var exeResult = Execute(code).Result;
            Console.WriteLine(exeResult);
            var result = EvaluateACSharpExpression().Result;
            Console.WriteLine(result);
            Console.WriteLine(EvaluateACSharpExpression().Result);
            EvaluateACSharpExpressionWithErrorHandling();
            result = AddReferences().Result;
            Console.WriteLine(result);
            result = AddImports().Result;
            Console.WriteLine(result);
            ParameterizeAScript();
            CreateAndBuildACSharpScriptAndExecuteItMultipleTimes();

            CreateADelegateToAScript();
            RunACSharpSnippetAndInspectDefinedScriptVariables();
            ChainCodeSnippetsToFormAScript();
            ContinueScriptExecutionFromAPreviousState();
            CreateAndAnalyzeACSharpScript();
            CustomizeAssemblyLoading();
        }
        private static Task<ScriptState<object>> scriptState = null;
        public static async Task<object> Execute(string code)
        {

            scriptState = scriptState == null ? CSharpScript.RunAsync(code) : scriptState.Result.ContinueWithAsync(code);
            return (await scriptState).ReturnValue;
        }
        public static async Task<object> EvaluateACSharpExpression()
        {
            return await CSharpScript.EvaluateAsync("1 + 2");
        }
        public static async void EvaluateACSharpExpressionWithErrorHandling()
        {
            try
            {
                Console.WriteLine(await CSharpScript.EvaluateAsync("2+2"));
            }
            catch (CompilationErrorException e)
            {
                Console.WriteLine(string.Join(Environment.NewLine, e.Diagnostics));
            }
        }
        public static async Task<object> AddReferences()
        {
            return await CSharpScript.EvaluateAsync("System.Net.Dns.GetHostName()",
                            ScriptOptions.Default.WithReferences(typeof(System.Net.Dns).AssemblyQualifiedName));

        }
        public static async Task<object> AddImports()
        {
            return await CSharpScript.EvaluateAsync("Sqrt(2)",
                    ScriptOptions.Default.WithImports("System.Math"));
        }
        public static async void ParameterizeAScript()
        {
            var globals = new Globals { X = 1, Y = 2 };
            Console.WriteLine(await CSharpScript.EvaluateAsync<int>("X+Y", globals: globals));
        }
        public static async void CreateAndBuildACSharpScriptAndExecuteItMultipleTimes()
        {
            var script = CSharpScript.Create<int>("X*Y", globalsType: typeof(Globals));
            script.Compile();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine((await script.RunAsync(new Globals { X = i, Y = i })).ReturnValue);
            }
        }
        public static async void CreateADelegateToAScript()
        {
            var script = CSharpScript.Create<int>("X*Y", globalsType: typeof(Globals));
            ScriptRunner<int> runner = script.CreateDelegate();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(await runner(new Globals { X = i, Y = i }));
            }
        }
        public static async void RunACSharpSnippetAndInspectDefinedScriptVariables()
        {
            var state = await CSharpScript.RunAsync<int>("int answer = 42;");
            foreach (var variable in state.Variables)
                Console.WriteLine($"{variable.Name} = {variable.Value} of type {variable.Type}");
        }
        public static async void ChainCodeSnippetsToFormAScript()
        {
            var script = CSharpScript.
                        Create<int>("int x = 1;").
                        ContinueWith("int y = 2;").
                        ContinueWith("x + y");

            Console.WriteLine((await script.RunAsync()).ReturnValue);
        }
        public static async void ContinueScriptExecutionFromAPreviousState()
        {
            var state = await CSharpScript.RunAsync("int x = 1;");
            state = await state.ContinueWithAsync("int y = 2;");
            state = await state.ContinueWithAsync("x+y");
            Console.WriteLine(state.ReturnValue);
        }

        //Compilation gives access to the full set of Roslyn APIs.
        public static string CreateAndAnalyzeACSharpScript()
        {
            var script = CSharpScript.Create<int>("3");
            Compilation compilation = script.GetCompilation();
            return compilation.AssemblyName;
        }
        public static string CustomizeAssemblyLoading()
        {
            using (var loader = new InteractiveAssemblyLoader())
            {
                var script = CSharpScript.Create<int>("1", assemblyLoader: loader);
                //do stuff 
                return script.Code;
            }
        }
    }
    public class Globals
    {
        public int X;
        public int Y;
    }
}
