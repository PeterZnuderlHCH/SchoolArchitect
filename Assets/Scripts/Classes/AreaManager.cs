using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class AreaManager : MonoBehaviour
    {
        private static AreaManager _instance;
        public static AreaManager instance
        {
            get {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AreaManager>();
                    if(_instance == null)
                    {
                        _instance = new GameObject("AreaManager_AutoGen").AddComponent<AreaManager>();
                    }
                }
                return _instance;
            } set
            {
                _instance = value;
            }
        }

        public List<Area> areas = new List<Area>();

        public bool startDrawing = false;
        public bool isBeingDrawn = false;
        Area newArea;
        Tile startPosition;
        Tile endPosition;

        void Awake()
        {
            if(instance == null) { instance = this; }
            else if (instance != this) { Destroy(this); }
        }

        public bool HandleClick(Tile tile, bool down, bool up)
        {
            if(!(startDrawing || isBeingDrawn)) { return false; }
            if (startDrawing)
            {
                PrepareForDrawing();
                StartDrawing(tile);
            }
            else if (isBeingDrawn)
            {
                if (up)
                {
                    UpdateDrawing(tile);
                    StopDrawing(tile);
                }
                if(!down && !up)
                {
                    UpdateDrawing(tile);
                }
            }
            return true;
        }

        void PrepareForDrawing()
        {
            newArea = new Area("Area 51", AreaType.Office, Random.ColorHSV());
            isBeingDrawn = true;
            startDrawing = false;
        }

        void StartDrawing(Tile position)
        {
            startPosition = position;
        }

        void StopDrawing(Tile position)
        {
            endPosition = position;
            isBeingDrawn = false;
            areas.Add(newArea);//May be a problem
            GatherNPCs(newArea);
        }

        private void UpdateDrawing(Tile temporaryEnd)
        {
            newArea.UpdateArea(startPosition, temporaryEnd);
        }

        void GatherNPCs(Area area)
        {
            switch (area.type)
            {
                case AreaType.Classroom:
                    for (int i = 0; i < NPCManager.instance.NPCs.Count; i++)
                    {
                        if(NPCManager.instance.NPCs[i].GetType() == typeof(student))
                        {
                            Tile t = area.tiles[Random.Range(0, area.tiles.Count)];
                            Task task = new Task(t, typeof(student), 0);
                            NPCManager.instance.NPCs[i].GiveTask(task);
                        }
                    }
                    break;
                case AreaType.CommonRoom:
                    for (int i = 0; i < NPCManager.instance.NPCs.Count; i++)
                    {
                        if (NPCManager.instance.NPCs[i].GetType() == typeof(teacher))
                        {
                            Tile t = area.tiles[Random.Range(0, area.tiles.Count)];
                            Task task = new Task(t, typeof(teacher), 0);
                            NPCManager.instance.NPCs[i].GiveTask(task);
                        }
                    }
                    break;
                case AreaType.Office:
                    for (int i = 0; i < NPCManager.instance.NPCs.Count; i++)
                    {
                        if (NPCManager.instance.NPCs[i].GetType() == typeof(officestaff))
                        {
                            Tile t = area.tiles[Random.Range(0, area.tiles.Count)];
                            Task task = new Task(t, typeof(officestaff), 0);
                            NPCManager.instance.NPCs[i].GiveTask(task);
                        }
                    }
                    break;
                case AreaType.Playground:
                    for (int i = 0; i < NPCManager.instance.NPCs.Count; i++)
                    {
                        if (NPCManager.instance.NPCs[i].GetType() == typeof(student))
                        {
                            Tile t = area.tiles[Random.Range(0, area.tiles.Count)];
                            Task task = new Task(t, typeof(student), 0);
                            NPCManager.instance.NPCs[i].GiveTask(task);
                        }
                    }
                    break;
                case AreaType.Storage:
                    for (int i = 0; i < NPCManager.instance.NPCs.Count; i++)
                    {
                        if (NPCManager.instance.NPCs[i].GetType() == typeof(cleaner))
                        {
                            Tile t = area.tiles[Random.Range(0, area.tiles.Count)];
                            Task task = new Task(t, typeof(cleaner), 0);
                            NPCManager.instance.NPCs[i].GiveTask(task);
                        }
                    }
                    break;
            }
        }

        void GetAllTiles()
        {

        }
    }
}