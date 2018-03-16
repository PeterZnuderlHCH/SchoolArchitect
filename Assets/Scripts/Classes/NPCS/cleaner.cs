using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cleaner : npc {
    int speed;

    public cleaner(int _age, int _income, string _description, float _health, float _sanity, float _rest, bool _male,
        int _speed)
    {
        age = _age;
        income = _income;
        description = _description;
        health = _health;
        sanity = _sanity;
        rest = _rest;
        male = _male;

        speed = _speed;
    }

}
