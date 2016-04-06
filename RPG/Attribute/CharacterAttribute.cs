using System;
using UnityEngine;
[System.Serializable]
public class CharacterAttribute:System.ICloneable
{
    public int HP;
    public int PhysicalPower;
    public int MagicalPower;
    public int Skill;
    public int Speed;
    public int Luck;
    public int PhysicalDefense;
    public int MagicalDefense;
    public int Movement;
    public void SetMaxium()
    {
        HP = 150;
        PhysicalPower = 60;
        MagicalPower = 60;
        Skill = 60;
        Speed = 60;
        Luck = 60;
        PhysicalDefense = 60;
        MagicalDefense = 60;
        Movement = 12;
    }
    public override string ToString()
    {
        return " HP= " + HP +
   " PhysicalPower= " + PhysicalPower +
   " MagicalPower= " + MagicalPower +
   " Skill= " + Skill +
   " Speed= " + Speed +
   " Luck= " + Luck +
   " PhysicalDefense= " + PhysicalDefense +
   " MagicalDefense= " + MagicalDefense +
   " Movement= " + Movement;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}