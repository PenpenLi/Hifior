[System.Serializable]
public class CharacterAttribute : System.ICloneable
{
    private static CharacterAttribute max;
    public static CharacterAttribute Max
    {
        get
        {
            if (max == null)
            {
                max = new CharacterAttribute();
                max.SetMaxium();
            }
            return max;
        }
    }
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
        PhysicalPower = 999;
        MagicalPower = 999;
        Skill = 999;
        Speed = 999;
        Intel = 999;
        PhysicalDefense = 999;
        MagicalDefense = 999;
        BodySize = 999;
        Movement = 12;
    }
    public override string ToString()
    {
        return " HP = " + HP +
   " PhysicalPower = " + PhysicalPower +
   " MagicalPower = " + MagicalPower +
   " Skill = " + Skill +
   " Speed = " + Speed +
   " Intel = " + Intel +
   " PhysicalDefense = " + PhysicalDefense +
   " MagicalDefense = " + MagicalDefense +
   " Movement = " + Movement;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}