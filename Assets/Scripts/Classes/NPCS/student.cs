using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class student : npc{
    string[] subjects;
    GameObject[] fashion;
    [Range(0f,1f)]
    float coolness;
    int popularityRank;

    public student(int _age, int _income, string _description, float _health, float _sanity, float _rest, bool _male,
        string[] _subjects, GameObject[] _fashion, float _coolness, int _popularityRank)
    {
        age = _age;
        income = _income;
        description = _description;
        health = _health;
        sanity = _sanity;
        rest = _rest;
        male = _male;

        subjects = _subjects;
        fashion = _fashion;
        coolness = _coolness;
        popularityRank = _popularityRank;
    }
}
