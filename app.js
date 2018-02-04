const path = require('path');
const baseNetAppPath = path.join(__dirname, '\\DLLs');

process.env.EDGE_USE_CORECLR = 1;
process.env.EDGE_APP_ROOT = baseNetAppPath;

var edge = require('edge-js');

var baseHelloWorldDll = path.join(baseNetAppPath, 'HelloWorld.dll');
var baseRoslynDemoDll = path.join(baseNetAppPath, 'RoslynDemo.dll');
var helloWorld = edge.func({
    assemblyFile: baseHelloWorldDll,
    typeName: 'HelloWorld.Other.OtherClass',
    methodName: 'OtherMethod'
});

helloWorld(12, function (error, result) {
    if(error) throw error;
    console.log(result);
})

var toScriptEngine = edge.func({
    assemblyFile: baseRoslynDemoDll,
    typeName: 'RoslynDemo.ScriptEngine',
    methodName: 'Execute' // This must be Func<object,Task<object>>
});
toScriptEngine(getSelectedText(), function (error, result) { 
    if(error) throw error;
    console.log(result); 
});

        
function getSelectedText() {
    
    selectedText = heredoc(function(){/*
        public class Hello
        {
            public string HelloWorld()
            {
                return "Hello World";
            }
        }
        new Hello().HelloWorld()
 */});
    console.log(selectedText);
    return selectedText;

}
function heredoc(fn) {
    return fn.toString().split('\n').slice(1,-1).join('\n') + '\n'
}
