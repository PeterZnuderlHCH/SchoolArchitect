using System.Collections;
using System.Collections.Generic;
using RPG;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public InputField inputX, inputY;
    public InputField inputTargetX, inputTargetY;
    public NPCManager npcman;
    public static GameObject selectedNPCgo;
    TaskManager tm;

    private void Awake()
    {
        tm = FindObjectOfType<TaskManager>();
    }

    public void OnSpawnNPCButtonClicked()
    {
        int x, y;

        int.TryParse(inputX.text,out x);
        int.TryParse(inputY.text, out y);

        selectedNPCgo = npcman.SpawnAnNPC("student", new Vector2(x, y));
    }

    public void OnMoveToButtonClicked()
    {
        int x, y;

        int.TryParse(inputTargetX.text, out x);
        int.TryParse(inputTargetY.text, out y);
        Vector2 target = new Vector2(x, y);
        Tile t = TileGenerator.map[x][y];

        Task task = new Task(t,typeof(npc), 0);
        TaskManager.instance.AddTaskToQueue(task);

        //selectedNPCgo.GetComponent<npc>().SetTargetLocation(x,y);
        
    }

    public void OnSpawnNPCXButtonClicked(int num)
    {
        string[] types = new string[] {"student", "teacher", "officestaff", "cleaner", "" };
        string type;
        for (int i = 0; i < num; i++)
        {
            type = types[Random.Range(0, types.Length)];
            Vector2 position = new Vector2(Random.Range(0, 100), Random.Range(0, 100));

            selectedNPCgo = npcman.SpawnAnNPC(type, position);

        }

    }

    public void OnMoveX100ButtonClicked()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector2 target = new Vector2((int)Random.Range(0,100), Random.Range(0, 100));
            Tile t = TileGenerator.map[(int)target.x][(int)target.y];

            Task task = new Task(t, typeof(npc), 0);
            TaskManager.instance.AddTaskToQueue(task);

        }
 
    }
}
