[System.Serializable]
public class CharacterAttribute : System.ICloneable
{
    public int HP;
    public int PhysicalPower;
    public int MagicalPower;
    public int Skill;
    public int Speed;
    public int Intel;
    public int PhysicalDefense;
    public int MagicalDefense;
    public int BodySize;
    public int Movement;
    public void SetMaxium()
    {
        HP = 9999;
        PhysicalPower = 1000;
        MagicalPower = 1000;
        Skill = 1000;
        Speed = 1000;
        Intel = 1000;
        PhysicalDefense = 1000;
        MagicalDefense = 1000;
        BodySize = 1000;
        Movement = 12;
    }
    public override string ToString()
    {
        return " HP= " + HP +
   " PhysicalPower= " + PhysicalPower +
   " MagicalPower= " + MagicalPower +
   " Skill= " + Skill +
   " Speed= " + Speed +
   " Intel= " + Intel +
   " PhysicalDefense= " + PhysicalDefense +
   " MagicalDefense= " + MagicalDefense +
   " Movement= " + Movement;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}