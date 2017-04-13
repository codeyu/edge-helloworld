var path = require('path')
var edge = require('edge')

var helloWorld = edge.func({
    assemblyFile: path.resolve(__dirname, 'DLLs', 'HelloWorld.dll'),
    typeName: 'HelloWorld.Other.OtherClass',
    methodName: 'OtherMethod'
});

helloWorld(12, function (error, result) {
    if(error) throw error;
    console.log(result);
})

var toScriptEngine = edge.func({
    assemblyFile: path.resolve(__dirname, 'DLLs', 'RoslynDemo.dll'),
    typeName: 'RoslynDemo.ScriptEngine',
    methodName: 'Execute' // This must be Func<object,Task<object>>
});
toScriptEngine(getSelectedText(), function (error, result) { console.log(result); });

        
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
