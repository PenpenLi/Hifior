public class UDamageType
{
    /** True if this damagetype is caused by the world (falling off level, into lava, etc). */
    bool bCausedByWorld;

    /** True to scale imparted momentum by the receiving pawn's mass for pawns using character movement */
    bool bScaleMomentumByMass;

    /** The magnitude of impulse to apply to the Actors damaged by this type. */
    int DamageImpulse;

    /** When applying radial impulses, whether to treat as impulse or velocity change. */
    bool bRadialDamageVelChange;

    /** How large the impulse should be applied to destructible meshes */
    float DestructibleImpulse;

    /** How much the damage spreads on a destructible mesh */
    float DestructibleDamageSpreadScale;

    /** Damage fall-off for radius damage (exponent).  Default 1.0=linear, 2.0=square of distance, etc. */
    float DamageFalloff;
};
