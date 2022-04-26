# Graph Theory project

Group member(s) 
1. Ashish Jacob Sam (M21CS003)

## Project title: Demo of 5 graph theory topics in an offline OpenGL app

## Application summary

This project will be provide visual demonstration of 5 graph theory algorithms. A user can use to create a custom graph (with less than 20 vertices, but edges are only limited by the number of vertices), move the vertices manually to arrange the graph, choose to keep the graph directed or undirected, and edit the edges in the graph. After creating the graph, the user can then see the visualized workings of the following algorithms on their custom graph:

- [Depth First Search](https://en.wikipedia.org/wiki/Depth-first_search): Provide a pass-by-pass view of Depth first search in a graph.
- [Breadth First Search](https://en.wikipedia.org/wiki/Breadth-first_search): Provide a pass-by-pass view of Breadth first search in a graph.
- [Dijkstra Search](https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm): Provide a pass-by-pass view of Dijkstra search (also known as the single-source shortest path search) in a graph.
- [Floyd-Warshall](https://en.wikipedia.org/wiki/Floyd%E2%80%93Warshall_algorithm): Provide a pass-by-pass view of All-Pairs shortest path search (using Floyd-warshall algorithn) in a graph.
- [Kruskal's MST](https://en.wikipedia.org/wiki/Kruskal%27s_algorithm): Provude a pass-by-pass view of finding the minimum spanning tree of a graph using Kruskal's algorithm.

## Development details:

This project will be using OpenGL bindings provided by the [Monogame framework](https://monogame.net). Monogame is a game engine/framework for creating simple games in C#. These games can be ported to all platforms, including Windows, Mac, Linux.

The project will be developed using C#, with [.NET SDK 6](https://dotnet.microsoft.com/en-us/download) which supports development with C# in Windows, Mac and Linux.

## Running the application

To run the application, you can download the binary from the [Releases](https://github.com/AzuxirenLeadGuy/GTT-Project/releases) (highly recommended), or build the program in your machine.

The executable to run is `GTT.GL.exe` for windows, and `GTT.GL` for linux and mac.

## Build details

This application is built using the 'develop' branch of the Monogame framework in github. As such building this project is a complicated process. Although the MonoGame project is very much active, the last official release is quite old and does not work in many machines. Therefore, the latest source code must be built in order to use the MonoGame framework to build projects. The MonoGame repository needs to be cloned, then the tools need to built as nuget packages, and then these nuget packages must be referenced by the projects `src/GTT/GTT.csproj` and `src/GTT.GL/GTT.GL.csproj.`.

The following is an excerpt from the Monogame Readme 
> The full source code is available here from GitHub:
> 
> - Clone the source: git clone https://github.com/MonoGame/MonoGame.git
> - Set up the submodules: git submodule update --init
> - Open the solution for your target platform to build the game framework.
> - Open the Tools solution for your development platform to build the pipeline and content tools.

For this project, the tools required are 

- `MonoGame.Framework.DesktopGL`
- `MonoGame.Framework.Content.Pipeline`
- `MonoGame.Content.BuilderTask`
- `mgcb-editor-linux`

For this project, the versioning is selected as `v3.8.99`, so all build versions must be specified with this versioning.

All these packages' requirements for building must be referred from the official documentation provided by MonoGame.

After all dependancies are met, running the project can be done by going to the src/GTT.GL folder and using:
```
dotnet run
```

To build application for target runtimes in windows and linux, use the following

```
dotnet publish -c Release -r win-x64 --self-contained true
dotnet publish -c Release -r linux-x64 --self-contained true
dotnet publish -c Release -r osx-x64 --self-contained true

```

This will publish the application targetting 64-bit windows, linux and mac desktop systems.
