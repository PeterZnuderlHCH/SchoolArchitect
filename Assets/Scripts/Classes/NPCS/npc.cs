using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Path;
using System.Threading;
using RPG;

[System.Serializable]
public class npc : MonoBehaviour{
    public static List<npc> allNPCs = new List<npc>();
	
    //protected => this scope allows the variable to be accessed from within this class and any sub-class
    protected int age;
    protected int income;
    protected string description;
    protected SpriteRenderer skin;
    [Range(0f, 1f)]//Creates a slider in Unity Inspector that will let you sett the value from 0 to 1
    protected float health;
    [Range(0f, 1f)]
    protected float sanity;
    [Range(0f, 1f)]
    protected float rest;
    protected bool male;
    [SerializeField]protected Task myTask;

    protected delegate void Trigger();
    Trigger taskChanged;

    private Movable movable;

    protected virtual void Awake()
	{
        allNPCs.Add(this);

        movable = GetComponent<Movable>();
        if (!movable) { movable = gameObject.AddComponent<Movable>(); }
        movable.Init(this);

        taskChanged += OnTaskChanged;
		skin = GetComponent<SpriteRenderer> (); // Get the refference for Sprite Rendere so thatwe can change the colour
    }

    public npc()
    {
        age = 30;
        income = 100;
        description = "Empty";
        health = 1f;     
        sanity = 1f;
        rest = 1f;
        male = false;
    }
    public npc(int _age, int _income, string _description, float _health, float _sanity, float _rest, bool _male)
    {
        age = _age;
        income = _income;
        description = _description;
        health = _health;
        sanity = _sanity;
        rest = _rest;
        male = _male;
    }

	public void ChangeColour(Color newCol)
	{
		skin.color = newCol;
	}

	void OnMouseDown()
	{
        UIController.selectedNPCgo = this.gameObject;
	}

    public Task GetTask()
    {
        return myTask;
    }

    public void GiveTask(Task newTask)
    {
        myTask = newTask;
        taskChanged();
    }

    void OnTaskChanged()
    {
        movable.SetTargetLocation(Mathf.RoundToInt(myTask.target.x), Mathf.RoundToInt(myTask.target.y));
    }

    public void OnTaskFinished()
    {
        myTask = null;
    }

 
}
