# edge-helloworld
Run .NET Core code from a Node.js process

### Building on OSX

Prerequisities:

* [Homebrew](http://brew.sh/)  
* [Mono x64](http://www.mono-project.com/download/#download-mac) and/or [.NET Core](https://dotnet.github.io/getting-started/) - see below  
* [Node.js x64](http://nodejs.org/) (tested with v4.1.1)  

You can use Edge.js on OSX with either Mono or .NET Core installed, or both.

If you choose to [install Mono](http://www.mono-project.com/download/#download-mac), select the universal installer to make sure you get the x64 version. Edge.js requires Mono x64.  If you choose to install .NET Core, follow the steps [here](https://www.microsoft.com/net/core#macosx)

Then install and build Edge.js:

```bash
brew install mono
brew install pkg-config
npm install edge
```

**NOTE** if the build process complains about being unable to locate Mono libraries, you may need to specify the search path explicitly. This may be installation dependent, but in most cases will look like: 

```bash
PKG_CONFIG_PATH=/Library/Frameworks/Mono.framework/Versions/Current/lib/pkgconfig \
  npm install edge
```

If you installed both Mono and .NET Core, by default Edge will use Mono. You opt in to using .NET Core with the `EDGE_USE_CORECLR` environment variable: 

```bash
EDGE_USE_CORECLR=1 node app.js
```

**note**: all dll command: `dotnet publish -f netcoreapp1.1 -c Release`