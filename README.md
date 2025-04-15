![Icon](https://raw.githubusercontent.com/Cubusky/Cubusky.Core/master/Icon.png)

# Cubusky
> 

Cubusky is a .NET Core library that provides a set of tools and utilities to help you build your applications faster and easier. It is a collection of classes, methods, and extensions focussing on optimized data storage, easy-to-use save and load functionality, and developer customization.

Target towards but not limited to game development, Cubusky is designed to be lightweight, easy to use, and highly customizable. All public code is documented and tested, and the library is designed to be as user-friendly as possible. Benchmarks are also included to help you understand the performance of the library.

The whole project is open-source and free to use, and you are welcome to contribute to the project.

## Installing / Getting started

The project can be installed from NuGet using either the [NuGet Package Manager](https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio) in Visual Studio or the [NuGet CLI](https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-nuget-cli) in the command line, e.g.

```shell
nuget install Cubusky.Core
```

The package and all of its dependencies will automatically be installed.

## Features

* **Save and Load**: Cubusky provides easy-to-use save and load functionality that can be customized to fit your needs.
```csharp
public class SaverLoader
{
    public IIOStreamProvider IO = new FileIO("C://myFile.json");
    public IStreamSerializer Serializer = new JsonSerializer();
    public ICompressionStreamProvider Compression = new GZipCompression();

    public void Save<T>(T value)
    {
        IO.Save(Serializer, Compression, value);
    }

    public T? Load<T>()
    {
        return IO.Load<T>(Serializer, Compression);
    }
}
```

* **Optimized Heatmaps**: Cubusky provides a set of optimized data structures and algorithms to help you build heatmaps faster and easier.
```csharp
public class Player
{
    public SaverLoader saverLoader;
    public Heatmap3to2 heatmap;

    public void Awake()
    {
        heatmap = saverLoader.Load<Heatmap3to2>() ?? new();
    }

    public void Update()
    {
        heatmap.Add(Position);
    }

    public void OnDestroy()
    {
        saverLoader.Save(heatmap);
    }
}
```

* **Optimized Json Converters** Optimized Json Converters for common numerics often used in game development.
```csharp
public class JsonData
{
    [JsonConverter(typeof(Vector3JsonConverter))]
    public Vector3 Position { get; set; } // Will be serialized as [X,Y,Z]

    [JsonConverter(typeof(QuaternionJsonConverter))]
    public Quaternion Rotation { get; set; } // Will be serialized as [X,Y,Z,W]

    [JsonConverter(typeof(Matrix4x4JsonConverter))]
    public Matrix4x4 Transform { get; set; } // Will be serialized as [M11,M12,M13,M14,M21,M22,M23,M24,M31,M32,M33,M34,M41,M42,M43,M44]
}
```

* **Many to Many Observer Pattern**: Cubusky provides a many-to-many observer pattern that allows you to easily observe and notify multiple objects.
```csharp
// Observe as many players from as many heatmaps as you want.

public static class ObserverSets
{
    public static readonly ObserverSet<Vector3> Vector3 = new();
}

public class Heatmap : IObserver<Vector3>
{
    public void OnEnable() => ObserverSets.Vector3.Add(this);
    public void OnDisable() => ObserverSets.Vector3.Remove(this);
}

public class Player : IObservable<Vector3>
{
    public void OnEnable() => ObserverSets.Vector3.Add(this);
    public void OnDisable() => ObserverSets.Vector3.Remove(this);
}
```

## Contributing

You can contribute to Cubusky with issues and PRs. Simply filing issues for problems you encounter is a great way to contribute. Contributing implementations is greatly appreciated.

TODO: format a contributing guide in a `CONTRIBUTING.md` file. For now, follow the [dotnet contribution guide](https://raw.githubusercontent.com/dotnet/runtime/main/CONTRIBUTING.md) and the [dotnet coding style](https://raw.githubusercontent.com/dotnet/runtime/main/docs/coding-guidelines/coding-style.md).

## Links

- Project homepage: IN PROGRESS
- Repository: https://github.com/Cubusky/Cubusky.Core/
- Issue tracker: https://github.com/Cubusky/Cubusky.Core/issues
- Related projects:
  - Unity Product Page: TO COME
  - Godot Product Page: TO COME

## Licensing

The code in this project is licensed under GNU GPLv3. This license lets users do almost anything they want with the project, except distributing closed sourced versions. [Ansible](https://github.com/ansible/ansible), [Bash](https://git.savannah.gnu.org/cgit/bash.git/) and [GIMP](https://gitlab.gnome.org/GNOME/gimp) use the GNU GPLv3. For more information, see the [LICENSE](https://raw.githubusercontent.com/Cubusky/Cubusky.Core/master/LICENSE.md) file.