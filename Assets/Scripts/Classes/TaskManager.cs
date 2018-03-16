using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace RPG
{
    public class TaskManager : MonoBehaviour
    {
        #region Singleton 
        public static TaskManager instance;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        #endregion

        float updateFrequency = 0.05f;
        [SerializeField] List<Task> tasks = new List<Task>();
        [SerializeField] List<npc> freeNpcs = new List<npc>();

        // Use this for initialization
        void Start()
        {
            StartCoroutine(CheckLazyNPCs());
            StartCoroutine(AssignTasks());
        }

        IEnumerator AssignTasks()
        {
            while (true)
            {
                if (tasks.Count <= 0 || freeNpcs.Count <= 0) { yield return new WaitForSecondsRealtime(updateFrequency); } //If we have no avaliable NPCs or no tasks needing completion do nothing -> prevent's wasted CPU time

                for (int i = 0; i < tasks.Count; i++)
                {
                    for (int j = 0; j < freeNpcs.Count; j++)
                    {
                        if (freeNpcs[j].GetType() == tasks[i].actorType || tasks[i].actorType == typeof(npc)) // If our NPC is the right type for the job give it to him.
                        {
                            DistributeTask(tasks[i], freeNpcs[j]);
                            break;
                        }
                    }
                }
                yield return new WaitForSecondsRealtime(updateFrequency);
            }
        }

        IEnumerator CheckLazyNPCs()
        {
            while (true)
            {
                for (int i = 0; i < npc.allNPCs.Count; i++)
                {

                    if (npc.allNPCs[i].GetTask() == null && !freeNpcs.Contains(npc.allNPCs[i]))
                    {
                        freeNpcs.Add(npc.allNPCs[i]);
                        //Debug.Log("Found " + npc.allNPCs[i] + " just sitting around. Disgraceful!");
                    }
                }
                yield return new WaitForSecondsRealtime(0.5f);
            }
        }

        public void AddNPCToQueue(npc newNpc)
        {
            freeNpcs.Add(newNpc);
        }

        public void AddTaskToQueue(Task newTask)
        {
            tasks.Add(newTask);
        }


        void DistributeTask(Task task, npc NPC)
        {
            Debug.Log("Giving a task to: " + NPC);
            NPC.GiveTask(task);
            freeNpcs.Remove(NPC);
            tasks.Remove(task);
        }
    }
}

