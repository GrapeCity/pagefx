using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public interface IManifestResource : ICustomAttributeProvider
    {
        string Name { get; set; }
        int Offset { get; set; }
        bool IsPublic { get; set; }
        IModule Module { get; set; }
        byte[] Data { get; set; }
    }

    public interface IManifestResourceCollection : IReadOnlyList<IManifestResource>
    {
        IManifestResource this[string name] { get; }
    }

    public interface IManifestFile : IManifestResource
    {
        string Location { get; set; }

        byte[] HashValue { get; set; }

        bool ContainsMetadata { get; set; }
    }

    public interface IManifestFileCollection : IList<IManifestFile>
    {
        IManifestFile this[string name] { get; }
    }

    public interface IUnmanagedResource
    {
        int CodePage { get; set; }

        int Language { get; set; }

        object Name { get; set; }

        object Type { get; set; }

        byte[] Value { get; set; }
    }

    public interface IUnmanagedResourceCollection : IList<IUnmanagedResource>
    {
    }
}

