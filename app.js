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