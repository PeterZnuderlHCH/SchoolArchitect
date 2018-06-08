using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public enum AreaType { Classroom, Storage, CommonRoom, Playground, Office }

    [System.Serializable]
    public class Area
    {
        string name;
        public int id;
        public AreaType type;
        public List<Tile> tiles;
        Color colour;

        public Area()
        {
            name = "Empty";
            type = AreaType.Storage;
            tiles = new List<Tile>();
            colour = new Color(1f,0f,0f);
            id = 1;
        }

        public Area(Area old)
        {
            name = old.name;
            type = old.type;
            tiles = old.tiles;
            colour = old.colour;
            id = old.id;
        }

        public Area(string _name, AreaType _type, Color _colour, int _id)
        {
            name = _name;
            type = _type;
            tiles = new List<Tile>();
            colour = _colour;
            id = _id;
        }

        public Area(string _name, AreaType _type, List<Tile> _tiles, Color _colour, int _id)
        {
            name = _name;
            type = _type;
            tiles = _tiles;
            colour = _colour;
        }

        public void AddTile(Tile tile)
        {
            if (!tiles.Contains(tile))
            {
                tiles.Add(tile);
            }
        }

        public void RemoveTile(Tile tile)
        {
            if (tiles.Contains(tile))
            {
                tiles.Remove(tile);
            }
        }

        public void UpdateArea(Tile start, Tile end)
        {
            List<Tile> temp = new List<Tile>();
            int minx = Mathf.Min(start.x, end.x);
            int miny = Mathf.Min(start.y, end.y);
            int maxx = Mathf.Max(start.x, end.x);
            int maxy = Mathf.Max(start.y, end.y);

            for (int x = minx; x < maxx; x++)
            {
                for (int y = miny; y < maxy; y++)
                {
                    if (!temp.Contains(TileGenerator.map[x][y]))
                    {
                        temp.Add(TileGenerator.map[x][y]);
                        TileGenerator.map[x][y].ChangeColourTo(colour);
                    }
                }
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (!tiles.Contains(temp[i]))
                {
                    tiles.Add(temp[i]);
                }
            }

        }

        public List<Tile> GetContents()
        {
            return tiles;
        }
    }
}
