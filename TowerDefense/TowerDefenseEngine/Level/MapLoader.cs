using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace TowerDefenseEngine
{
    [ContentTypeWriter]
    public class MapLoaderWriter : ContentTypeWriter<MapLoader>
    {
        protected override void Write(ContentWriter output, MapLoader value)
        {
            output.WriteObject(value.MapsContentNames);
        }

        public override string GetRuntimeReader(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
        {
            return typeof(MapLoader.MapLoaderReader).AssemblyQualifiedName;
        }
    }

    public class MapLoader
    {
        public List<string> MapsContentNames
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public List<Map> Maps
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public static MapLoader Singleton;

        public MapLoader()
        {
            MapsContentNames = new List<string>();
            Maps = new List<Map>();
        }

        public class MapLoaderReader : ContentTypeReader<MapLoader>
        {
            protected override MapLoader Read(ContentReader input, MapLoader existingInstance)
            {
                MapLoader mapLoader = new MapLoader();
                mapLoader.MapsContentNames.AddRange(input.ReadObject<List<string>>());
                foreach (string s in mapLoader.MapsContentNames)
                {
                    mapLoader.Maps.Add(input.ContentManager.Load<Map>(String.Format("Maps\\{0}\\{0}", s)));
                }


                return mapLoader;
            }
        }

    }
}
