using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class officestaff : npc {

    int ability;

    public officestaff(int _age, int _income, string _description, float _health, float _sanity, float _rest, bool _male,
        int _ability)
    {
        age = _age;
        income = _income;
        description = _description;
        health = _health;
        sanity = _sanity;
        rest = _rest;
        male = _male;

        ability = _ability;
    }
}
