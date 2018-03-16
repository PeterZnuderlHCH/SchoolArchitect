using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;
using System;

namespace RPG
{
    [Serializable]
    public class Task
    {
        public Tile target; //{ get; private set; }
        public Type actorType; //{ get; private set; }
        public int priority; //{ get; private set; }

        //TODO: Add Action to the task

        public Task()
        {
            target = null;
            actorType = typeof(npc);
            priority = 0;
        }

        public Task(Tile _target, Type _type, int _priority)
        {
            target = _target;
            actorType = _type;
            priority = _priority;
        }

    }
}

