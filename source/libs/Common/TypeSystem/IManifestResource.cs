using System.IO;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public interface IManifestResource : ICustomAttributeProvider
    {
        string Name { get; set; }

        int Offset { get; set; }

        bool IsPublic { get; set; }

        IModule Module { get; set; }

        Stream Data { get; set; }
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

    public interface IManifestFileCollection : IReadOnlyList<IManifestFile>
    {
        IManifestFile this[string name] { get; }
    }
}

