using UnityEngine;
[System.Serializable]
public class CharacterAttribute
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
}