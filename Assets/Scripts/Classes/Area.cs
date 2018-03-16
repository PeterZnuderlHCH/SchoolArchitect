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
        public AreaType type;
        public List<Tile> tiles;
        Color colour;

        public Area()
        {
            name = "Empty";
            type = AreaType.Storage;
            tiles = new List<Tile>();
            colour = new Color(1f,0f,0f);
        }

        public Area(string _name, AreaType _type, Color _colour)
        {
            name = _name;
            type = _type;
            tiles = new List<Tile>();
            colour = _colour;
        }

        public Area(string _name, AreaType _type, List<Tile> _tiles, Color _colour)
        {
            name = _name;
            type = _type;
            tiles = _tiles;
            colour = _colour;
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
                    temp.Add(TileGenerator.map[x][y]);
                    TileGenerator.map[x][y].ChangeColourTo(colour);
                }
            }

            tiles = temp;
        }
    }
}
