using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour {

    public static NPCManager instance;

	Vector2 startingpos = new Vector2 (0, 0);
	public GameObject NPCPref;
    public List<npc> NPCs = new List<npc>();

    void Awake()
    {
        if(instance == null) { instance = this; }
        if(instance != this) { Destroy(gameObject); }
    }

	IEnumerator Start()
	{
        while(TileGenerator.map == null)
        {
            Debug.Log("Waiting");
            yield return null;
        }
	}

	public GameObject SpawnAnNPC(string type, Vector2 pos = default(Vector2))
	{
		GameObject npcinst = Instantiate(NPCPref, pos, Quaternion.identity, transform);

		switch (type)
		{
		case "student":
			student std = npcinst.AddComponent<student> ();
			std.ChangeColour (Color.gray);
			break;
		case "teacher":
			teacher teach = npcinst.AddComponent<teacher> ();
			teach.ChangeColour (Color.green);
			break;
		case "officestaff":
			officestaff of = npcinst.AddComponent<officestaff> ();
			of.ChangeColour (Color.blue);
			break;
        case "cleaner":
            cleaner cln = npcinst.AddComponent<cleaner>();
            cln.ChangeColour(Color.magenta);
            break;
        default:
            npc NPC = npcinst.AddComponent<npc>();
            NPC.ChangeColour(Color.yellow);
            break;
        }
        NPCs.Add(npcinst.GetComponent<npc>());
        return npcinst;
	}
}
