using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace TowerDefenseEngine
{
    public enum MapState
    {
        WaveDelay,
        Active,
        Finished
    }
    public class Map
    {
        #region Writer
        [ContentTypeWriter]
        public class MapWriter : ContentTypeWriter<Map>
        {
            protected override void Write(ContentWriter output, Map value)
            {
                output.Write(value.Name);
                output.WriteObject(value.Dimensions);
                output.WriteObject(value.TileDimensions);
                output.Write(value.Description);
                output.Write(value.ThumbnailAsset);
                output.Write(value.TileSheetAsset);
                output.WriteObject(value.GroundTiles);
                output.WriteObject(value.PathTiles);
                output.WriteObject(value.SpawnLocation);
                output.Write(value.SpawnAssetName);
                output.WriteObject(value.CastleLocation);
                output.Write(value.CastleAssetName);
                output.Write(value.Endless);
                output.WriteObject(value.TowersAvailable);
                output.WriteObject(value.WavesAvailable);
                output.Write(value.WaveDelay);
                output.Write(value.Difficulty);
                output.Write(value.Money);
                output.WriteObject(value.ForeColorArray);
                output.WriteObject(value.ErrorColorArray);
                output.Write(value.InfoBarBackgroundAsset);
                output.WriteObject(value.BorderColorArray);
                output.Write(value.BorderTextureAsset);
                output.Write(value.MouseTextureAsset);
                output.Write(value.SmallErrorButtonTextureAsset);
                output.Write(value.SmallNormalButtonTextureAsset);
                output.Write(value.LargeErrorButtonTextureAsset);
                output.Write(value.LargeNormalButtonTextureAsset);
                output.Write(value.SongCueName);

            }

            public override string GetRuntimeReader(Microsoft.Xna.Framework.Content.Pipeline.TargetPlatform targetPlatform)
            {
                return typeof(Map.MapReader).AssemblyQualifiedName;
            }
        }
        #endregion

        [ContentSerializerIgnore]
        public MapState State
        {
            get;
            private set;
        }

        [ContentSerializer]
        public string Name
        {
            get;
            private set;
        }

        [ContentSerializer]
        public Point Dimensions
        {
            get;
            private set;
        }

        [ContentSerializer]
        public Point TileDimensions
        {
            get;
            private set;
        }

        [ContentSerializer]
        public string Description
        {
            get;
            private set;
        }

        [ContentSerializer(Optional = true)]
        private string ThumbnailAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D Thumbnail
        {
            get;
            private set;
        }

        [ContentSerializer(Optional = true)]
        private string TileSheetAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D TileSheet
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public Point NumberOfTilesInSheet
        {
            get;
            private set;
        }

        [ContentSerializer]
        public int[] GroundTiles
        {
            get;
            private set;
        }

        [ContentSerializer(Optional = true)]
        public int[] PathTiles
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public Path ActivePath
        {
            get;
            set;
        }

        [ContentSerializer]
        public Point SpawnLocation
        {
            get;
            private set;
        }

        [ContentSerializer]
        private string SpawnAssetName
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public GameplayObject Spawn
        {
            get;
            private set;
        }

        [ContentSerializer]
        public Point CastleLocation
        {
            get;
            private set;
        }

        [ContentSerializer]
        private string CastleAssetName
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public GameplayObject Castle
        {
            get;
            private set;
        }

        [ContentSerializer]
        public bool Endless
        {
            get;
            private set;
        }

        [ContentSerializer]
        private List<string> TowersAvailable
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public List<Tower> TowerList
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public string TowersInfo
        {
            get;
            private set;
        }

        [ContentSerializer]
        private List<string> WavesAvailable
        {
            get;
            set;
        }
        [ContentSerializer]
        private float WaveDelay
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public float Timer
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public List<Wave> WaveList
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public int WaveIndex
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public Wave ActiveWave
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public string WavesInfo
        {
            get;
            private set;
        }

        [ContentSerializer]
        public int Difficulty
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        public string DifficultyInfo
        {
            get;
            private set;
        }

        int money;

        [ContentSerializer]
        public int Money
        {
            get { return money; }
            private set
            {
                money = value;
                MoneyInfo = String.Format("Money: {0}", money);
            }
        }

        [ContentSerializerIgnore]
        public string MoneyInfo
        {
            get;
            private set;
        }

        [ContentSerializer]
        private int[] ForeColorArray
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Color ForeColor
        {
            get;
            private set;
        }

        [ContentSerializer]
        private int[] ErrorColorArray
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Color ErrorColor
        {
            get;
            private set;
        }

        [ContentSerializer]
        private string InfoBarBackgroundAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D InfoBarBackground
        {
            get;
            private set;
        }

        [ContentSerializer]
        private int[] BorderColorArray
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Color BorderColor
        {
            get;
            private set;
        }

        [ContentSerializer]
        private string BorderTextureAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D BorderTexture
        {
            get;
            private set;
        }

        [ContentSerializer]
        private string MouseTextureAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D MouseTexture
        {
            get;
            private set;
        }

        [ContentSerializerIgnore]
        private int[,] PlaceableArray
        {
            get;
            set;
        }

        [ContentSerializer]
        private string SmallErrorButtonTextureAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D SmallErrorButtonTexture
        {
            get;
            private set;
        }

        [ContentSerializer]
        private string SmallNormalButtonTextureAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D SmallNormalButtonTexture
        {
            get;
            private set;
        }

        [ContentSerializer]
        private string LargeErrorButtonTextureAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D LargeErrorButtonTexture
        {
            get;
            private set;
        }

        [ContentSerializer]
        private string LargeNormalButtonTextureAsset
        {
            get;
            set;
        }

        [ContentSerializerIgnore]
        public Texture2D LargeNormalButtonTexture
        {
            get;
            private set;
        }

        [ContentSerializer]
        public string SongCueName
        {
            get;
            private set;
        }

        public event EventHandler NewWave;

        public Map()
        {
            TowersAvailable = new List<string>();
            WavesAvailable = new List<string>();
            TowersInfo = string.Empty;
            WavesInfo = string.Empty;
            DifficultyInfo = string.Empty;
            WaveIndex = 0;
        }

        public Point ToWorldCoordinates(Tile t)
        {
            Point result = new Point(-1, -1);

            result.X = (t.MapLocation.X * TileDimensions.X) + (TileDimensions.X / 2);
            result.Y = (t.MapLocation.Y * TileDimensions.Y) + (TileDimensions.Y / 2);

            return result;
        }

        public Point ToWorldCoordinates(int x, int y)
        {
            Point result = new Point(-1, -1);

            result.X = (x * TileDimensions.X);
            result.Y = (y * TileDimensions.Y);

            return result;
        }

        public Point ToMapCoordinates(int x, int y)
        {
            Point result = new Point(-1, -1);

            result.X = x / TileDimensions.X;
            result.Y = y / TileDimensions.Y;

            return result;
        }

        public void Update(GameTime gameTime)
        {
            if (ActiveWave.IsDone)
            {
                if (WaveIndex >= WaveList.Count - 1)
                {
                    if (Endless) WaveIndex = 0;
                    else { State = MapState.Finished; }
                }

                if (State != MapState.Finished)
                {
                    if (WaveDelay > 0)
                    {
                        Timer = WaveDelay;
                        State = MapState.WaveDelay;
                    }
                    WaveIndex++;
                    ActiveWave = WaveList[WaveIndex];

                    if (NewWave != null)
                        NewWave(this, EventArgs.Empty);
                }
            }
            else
            {
                if (Timer > 0)
                {
                    Timer -= (float)gameTime.ElapsedGameTime.TotalSeconds * Session.singleton.Speed;
                }
                else
                {
                    if (State == MapState.WaveDelay) State = MapState.Active;

                    ActiveWave.Update(gameTime);
                }
            }
        }

        public void StartNextWaveNow()
        {
            Timer = 0;
            State = MapState.Active;
        }

        public void Reset()
        {
            WaveIndex = 0;

            foreach (Wave w in WaveList)
                w.Reset();

            ActiveWave = WaveList[WaveIndex];
            State = MapState.WaveDelay;

            GeneratePlaceableArray();
        }

        public bool IsValidPlacement(int x, int y)
        {
            if (y < PlaceableArray.GetLength(0) && x < PlaceableArray.GetLength(1)
                && y >= 0 && x >= 0)
                return PlaceableArray[y, x] == 0;
            else
                return false;
        }

        public void SetValidPlacement(int x, int y, bool b)
        {
            PlaceableArray[y, x] = b ? 0 : 1;
        }

        private void GeneratePlaceableArray()
        {
            int[,] arr = new int[Dimensions.Y, Dimensions.X];

            for (int y = 0; y < Dimensions.Y; y++)
            {
                for (int x = 0; x < Dimensions.X; x++)
                {
                    int gtc = GroundTiles[(y * Dimensions.X) + x];
                    int ptc = PathTiles[(y * Dimensions.X) + x];

                    if (gtc == 0 && ptc == 0 && (SpawnLocation.X != x || SpawnLocation.Y != y) && (CastleLocation.X != x || CastleLocation.Y != y))
                    {
                        arr[y, x] = 0;
                    }
                    else
                    {
                        arr[y, x] = 1;
                    }
                }
            }

            PlaceableArray = arr;
        }

        #region Content Reader
        public class MapReader : ContentTypeReader<Map>
        {
            protected override Map Read(ContentReader input, Map existingInstance)
            {
                Map map = new Map();

                map.Name = input.ReadString();
                map.Dimensions = input.ReadObject<Point>();
                map.TileDimensions = input.ReadObject<Point>();
                map.Description = input.ReadString();
                map.ThumbnailAsset = input.ReadString();
                if (string.IsNullOrEmpty(map.ThumbnailAsset))
                {
                    map.ThumbnailAsset = "nothumb";
                    map.Thumbnail = input.ContentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}", map.ThumbnailAsset));
                }
                else
                {
                    map.Thumbnail = input.ContentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, map.ThumbnailAsset));
                }

                map.TileSheetAsset = input.ReadString();
                if (string.IsNullOrEmpty(map.TileSheetAsset)) throw new Exception("You need a tile sheet in order to create a map.");
                map.TileSheet = input.ContentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, map.TileSheetAsset));
                map.NumberOfTilesInSheet = new Point(map.TileSheet.Width / map.TileDimensions.X, map.TileSheet.Height / map.TileDimensions.Y);

                map.GroundTiles = input.ReadObject<int[]>();
                map.PathTiles = input.ReadObject<int[]>();

                map.SpawnLocation = input.ReadObject<Point>();
                map.SpawnAssetName = input.ReadString();
                if (string.IsNullOrEmpty(map.SpawnAssetName)) throw new Exception("You need a spawn asset name in the XML.");
                map.Spawn = new GameplayObject();
                map.Spawn.Texture = input.ContentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, map.SpawnAssetName));
                map.Spawn.Position = new Vector2((map.SpawnLocation.X * map.TileDimensions.X) + map.Spawn.Origin.X, (map.SpawnLocation.Y * map.TileDimensions.Y) + map.Spawn.Origin.Y);
                


                map.CastleLocation = input.ReadObject<Point>();
                map.CastleAssetName = input.ReadString();
                if (string.IsNullOrEmpty(map.CastleAssetName)) throw new Exception("You need a castle asset name in the XML.");
                map.Castle = new GameplayObject();
                map.Castle.Texture = input.ContentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, map.CastleAssetName));
                map.Castle.Position = new Vector2((map.CastleLocation.X * map.TileDimensions.X) + map.Castle.Origin.X, (map.CastleLocation.Y * map.TileDimensions.Y) + map.Castle.Origin.Y);
                

                GenerateActiveTilePath(map);
                map.Spawn.Rotation = GetAngleBetweenTwoMapLocations(map.SpawnLocation, GetAdjacentTileLocation(map, map.SpawnLocation));
                map.Castle.Rotation = GetAngleBetweenTwoMapLocations(map.CastleLocation, GetAdjacentTileLocation(map, map.CastleLocation));

                map.Endless = input.ReadBoolean();

                map.TowersAvailable.AddRange(input.ReadObject<List<string>>());
                map.TowersInfo = String.Format("Available Towers: {0}", map.TowersAvailable.Count);

                map.TowerList = new List<Tower>(map.TowersAvailable.Count);
                foreach (string s in map.TowersAvailable)
                {
                    Tower t = input.ContentManager.Load<Tower>(String.Format("Towers\\{0}", s)).Clone();
                    t.Initialize(map, input.ContentManager);
                    map.TowerList.Add(t);
                }

                map.WavesAvailable.AddRange(input.ReadObject<List<string>>());
                map.WavesInfo = String.Format("Waves: {0}", map.WavesAvailable.Count);

                map.WaveList = new List<Wave>(map.WavesAvailable.Count);
                foreach (string s in map.WavesAvailable)
                {
                    Wave w = input.ContentManager.Load<Wave>(String.Format("Wave\\{0}", s)).Clone();
                    w.Initialize(map);
                    map.WaveList.Add(w);
                }

                map.ActiveWave = map.WaveList[0];
                map.WaveIndex = 0;

                map.WaveDelay = input.ReadSingle();

                map.Difficulty = input.ReadInt32();
                map.DifficultyInfo = String.Format("Difficulty: {0}", map.Difficulty);

                map.Money = input.ReadInt32();
                map.ForeColorArray = input.ReadObject<int[]>();
                if (map.ForeColorArray.Length == 4)
                {
                    map.ForeColor = new Color(map.ForeColorArray[0], map.ForeColorArray[1], map.ForeColorArray[2], map.ForeColorArray[3]);
                }
                else
                {
                    throw new Exception(input.AssetName + ".xml must have 4 integers to represent RGBA color data in the ForeColorArray tag.");
                }

                map.ErrorColorArray = input.ReadObject<int[]>();
                if (map.ErrorColorArray.Length == 4)
                {
                    map.ErrorColor = new Color(map.ErrorColorArray[0], map.ErrorColorArray[1], map.ErrorColorArray[2], map.ErrorColorArray[3]);
                }
                else
                {
                    throw new Exception(input.AssetName + ".xml must have 4 integers to represent RGBA color data in the ErrorColorArray tag.");
                }

                map.InfoBarBackgroundAsset = input.ReadString();
                if (!string.IsNullOrEmpty(map.InfoBarBackgroundAsset)) map.InfoBarBackground = input.ContentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, map.InfoBarBackgroundAsset));

                map.BorderColorArray = input.ReadObject<int[]>();
                if (map.BorderColorArray.Length == 4)
                {
                    map.BorderColor = new Color(map.BorderColorArray[0], map.BorderColorArray[1], map.BorderColorArray[2], map.BorderColorArray[3]);
                }

                map.BorderTextureAsset = input.ReadString();
                if (!string.IsNullOrEmpty(map.BorderTextureAsset)) map.BorderTexture = input.ContentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, map.BorderTextureAsset));

                map.MouseTextureAsset = input.ReadString();
                if (!string.IsNullOrEmpty(map.MouseTextureAsset)) map.MouseTexture = input.ContentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, map.MouseTextureAsset));

                map.PlaceableArray = GeneratePlaceableArray(map);

                map.SmallErrorButtonTextureAsset = input.ReadString();
                if (!string.IsNullOrEmpty(map.SmallErrorButtonTextureAsset)) map.SmallErrorButtonTexture = input.ContentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, map.SmallErrorButtonTextureAsset));

                map.SmallNormalButtonTextureAsset = input.ReadString();
                if (!string.IsNullOrEmpty(map.SmallNormalButtonTextureAsset)) map.SmallNormalButtonTexture = input.ContentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, map.SmallNormalButtonTextureAsset));

                map.LargeErrorButtonTextureAsset = input.ReadString();
                if (!string.IsNullOrEmpty(map.LargeErrorButtonTextureAsset)) map.LargeErrorButtonTexture = input.ContentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, map.LargeErrorButtonTextureAsset));

                map.LargeNormalButtonTextureAsset = input.ReadString();
                if (!string.IsNullOrEmpty(map.LargeNormalButtonTextureAsset)) map.LargeNormalButtonTexture = input.ContentManager.Load<Texture2D>(String.Format("Textures\\Maps\\{0}\\{1}", map.Name, map.LargeNormalButtonTextureAsset));

                if (map.SmallNormalButtonTexture.Bounds != map.SmallErrorButtonTexture.Bounds) throw new Exception("The SmallNormalButtonTexture and the SmallErrorButtonTexture must be the same size");
                if (map.LargeNormalButtonTexture.Bounds != map.LargeErrorButtonTexture.Bounds) throw new Exception("The LargeNormalButtonTexture and the LargeErrorButtonTexture must be the same size");

                if (map.WaveDelay > 0)
                {
                    map.State = MapState.WaveDelay;
                    map.Timer = map.WaveDelay;
                }

                map.SongCueName = input.ReadString();

                return map;
            }

            private int[,] GeneratePlaceableArray(Map map)
            {
                int[,] arr = new int[map.Dimensions.Y, map.Dimensions.X];

                for (int y = 0; y < map.Dimensions.Y; y++)
                {
                    for (int x = 0; x < map.Dimensions.X; x++)
                    {
                        int gtc = map.GroundTiles[(y * map.Dimensions.X) + x];
                        int ptc = map.PathTiles[(y * map.Dimensions.X) + x];

                        if (gtc == 0 && ptc == 0 && (map.SpawnLocation.X != x || map.SpawnLocation.Y != y) && (map.CastleLocation.X != x || map.CastleLocation.Y != y))
                        {
                            arr[y, x] = 0;
                        }
                        else
                        {
                            arr[y, x] = 1;
                        }
                    }
                }

                return arr;
            }

            private float GetAngleBetweenTwoMapLocations(Point source, Point target)
            {
                if (target.X == -1) return 0.0f;
                else
                {
                    return (float)Math.Atan2(target.Y - source.Y, target.X - source.X);
                }
            }

            private Point GetAdjacentTileLocation(Map map, Point point)
            {
                for (int x = point.X - 1; x <= point.X + 1; x++)
                {
                    if (x >= 0 && x < map.ActivePath.Length.X && x != point.X)
                    {
                        if (map.ActivePath[x, point.Y].Type == TileType.Passable) return map.ActivePath[x, point.Y].MapLocation;
                    }
                }
                for (int y = point.Y - 1; y <= point.Y + 1; y++)
                {
                    if (y >= 0 && y < map.ActivePath.Length.Y && y != point.Y)
                    {
                        if (map.ActivePath[point.X, y].Type == TileType.Passable) return map.ActivePath[point.X, y].MapLocation;
                    }
                }
                return new Point(-1, -1);
            }

            private void GenerateActiveTilePath(Map map)
            {
                Tile[,] ap = new Tile[map.Dimensions.X, map.Dimensions.Y];
                for (int y = 0; y < map.Dimensions.Y; y++)
                {
                    for (int x = 0; x < map.Dimensions.X; x++)
                    {
                        int code = map.PathTiles[(y * map.Dimensions.X) + x];
                        Tile t = new Tile(new Point(x, y), code);
                        if (code != 0) t.Type = TileType.Passable;
                        else t.Type = TileType.NotPassable;
                        ap[x, y] = t;
                    }
                }

                map.ActivePath = new Path(ap, ap[map.SpawnLocation.X, map.SpawnLocation.Y], ap[map.CastleLocation.X, map.CastleLocation.Y]);
                map.ActivePath.Start.Type = TileType.Passable;
                map.ActivePath.End.Type = TileType.Passable;

                GenerateAdjacencyList(map);
                map.ActivePath.CalculateShortestPath();
            }

            private void GenerateAdjacencyList(Map map)
            {
                for (int j = 0; j < map.Dimensions.Y; j++)
                {
                    for (int i = 0; i < map.Dimensions.X; i++)
                    {
                        Tile t = map.ActivePath[i, j];
                        if (t.TileCode == 0 || t.TileCode == 6 || t.TileCode == 8 || t.TileCode == 9) t.AddToAdjacencyList(GetTile(map, i - 1, j, t.TileCode));
                        if (t.TileCode == 0 || t.TileCode == 7 || t.TileCode == 8 || t.TileCode == 10) t.AddToAdjacencyList(GetTile(map, i, j - 1, t.TileCode));
                        if (t.TileCode == 0 || t.TileCode == 7 || t.TileCode == 9 || t.TileCode == 11) t.AddToAdjacencyList(GetTile(map, i, j + 1, t.TileCode));
                        if (t.TileCode == 0 || t.TileCode == 6 || t.TileCode == 10 || t.TileCode == 11) t.AddToAdjacencyList(GetTile(map, i + 1, j, t.TileCode));
                    }
                }
            }

            Tile GetTile(Map map, int row, int column, int code)
            {
                if (row < 0 || row >= map.Dimensions.X ||
                    column < 0 || column >= map.Dimensions.Y)
                {
                    return null;
                }
                else
                {
                    Tile t = map.ActivePath[row, column];
                    if (t.Type == TileType.Passable)
                    {
                        
                        return t;
                    }
                    else return null;
                }
            }
        }
        #endregion
    }
}
