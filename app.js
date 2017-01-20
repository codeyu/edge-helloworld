var path = require('path')
var edge = require('edge')

var helloWorld = edge.func(path.resolve(__dirname, 'DLLs', 'HelloWorld.dll'));

helloWorld(12, function (error, result) {
    if(error) throw error;
    console.log(result);
})