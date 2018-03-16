using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class teacher : npc {

    string subject;
    int level;
    int max_students;
    int experience;
    float integrity;

    public teacher(int _age, int _income, string _description, float _health, float _sanity, float _rest, bool _male,
        string _subject, int _level, int _max_students, int _experience, float _integrity)
    {
        age = _age;
        income = _income;
        description = _description;
        health = _health;
        sanity = _sanity;
        rest = _rest;
        male = _male;

        subject = _subject;
        level = _level;
        max_students = _max_students;
        experience = _experience;
        integrity = _integrity;
    }
}
