using UnityEngine;
public class UController : UActor
{
    /** Pawn currently being controlled by this controller.  Use Pawn.Possess() to take control of a pawn */
    private UPawn Pawn;

    /**
	 * Used to track when pawn changes during OnRep_Pawn. 
	 * It's possible to use a OnRep parameter here, but I'm not sure what happens with pointers yet so playing it safe.
	 */
    private UPawn OldPawn;

    /** PlayerState containing replicated information about the player using this controller (only exists for players, not NPCs). */
    UPlayerController PlayerState;

    public virtual void SetInitialLocationAndRotation(Vector3 NewLocation, Quaternion NewRotation) { }

    /**
	 * If true, the controller location will match the possessed Pawn's location. If false, it will not be updated. Rotation will match ControlRotation in either case.
	 * Since a Controller's location is normally inaccessible, this is intended mainly for purposes of being able to attach
	 * an Actor that follows the possessed Pawn location, but that still has the full aim rotation (since a Pawn might
	 * update only some components of the rotation).
	 */
    bool bAttachToPawn;

    /**
	  * Physically attach the Controller to the specified Pawn, so that our position reflects theirs.
	  * The attachment persists during possession of the pawn. The Controller's rotation continues to match the ControlRotation.
	  * Attempting to attach to a NULL Pawn will call DetachFromPawn() instead.
	  */
    public virtual void AttachToPawn(UPawn InPawn) { }

    /** Detach the RootComponent from its parent, but only if bAttachToPawn is true and it was attached to a Pawn.	 */
    public virtual void DetachFromPawn() { }

    /** Add dependency that makes us tick before the given Pawn. This minimizes latency between input processing and Pawn movement.	 */
    public virtual void AddPawnTickDependency(UPawn NewPawn) { }

    /** Remove dependency that makes us tick before the given Pawn.	 */
    public virtual void RemovePawnTickDependency(UPawn InOldPawn) { }


    /** Actor marking where this controller spawned in. */
    UActor StartSpot;

    //=============================================================================
    // CONTROLLER STATE PROPERTIES
    string StateName;

    /** Change the current state to named state */
    public virtual void ChangeState(string NewState) { }

    /** 
	 * States (uses FNames for replication, correlated to state flags) 
	 * @param StateName the name of the state to test against
	 * @return true if current state is StateName
	 */
    public bool IsInState(string InStateName) { return true; }

    /** @return the name of the current state */
    public string GetStateName() { return StateName; }
    public virtual void GetActorEyesViewPoint(out Vector3 out_Location, out Quaternion out_Rotation)
    {
        out_Location = Vector3.zero;
        out_Rotation = Quaternion.identity;
    }

    /* Overridden to create the player replication info and perform other mundane initialization tasks. */
    public virtual void PostInitializeComponents() { }
    public virtual void Reset() { }
    public virtual void Destroyed() { }
    // End AActor interface

    /** Getter for Pawn */
    public UPawn GetPawn() { return Pawn; }

    /** Setter for Pawn. Normally should only be used internally when possessing/unpossessing a Pawn. */
    public virtual void SetPawn(UPawn InPawn) { Pawn = InPawn; }

    /** Get the actor the controller is looking at */
    public virtual UActor GetViewTarget() { return null; }

    /** Called from Destroyed().  Cleans up PlayerState. */
    public virtual void CleanupPlayerState() { }

    /**
	 * Handles attaching this controller to the specified pawn.
	 * Only runs on the network authority (where HasAuthority() returns true).
	 * @param InPawn The Pawn to be possessed.
	 * @see HasAuthority()
	 */
    public virtual void Possess(UPawn InPawn)
    {
        if (InPawn == null)
        {
            Debug.LogError("The Pawn you try to possess is null");
            return;
        }
        OldPawn = Pawn;
        Pawn = InPawn;
        if (OldPawn != null)
            OldPawn.GetComponent<UPawn>().enabled = false;
        Pawn.GetComponent<UPawn>().enabled = true;
    }
    public void PossessOld()
    {
        Possess(OldPawn);
    }
    /**
	 * Called to unpossess our pawn for any reason that is not the pawn being destroyed (destruction handled by PawnDestroyed()).
	 */
    public virtual void UnPossess()
    {
        Destroy(Pawn);
    }

    /**
     * Called to unpossess our pawn because it is going to be destroyed.
     * (other unpossession handled by UnPossess())
     */
    public virtual void PawnPendingDestroy(UPawn inPawn) { }

    public virtual void NotifyKilled(UController Killer, UController KilledPlayer, UPawn KilledPawn, UDamageType DamageType) { }

    /** Called when this controller instigates ANY damage */
    public virtual void InstigatedAnyDamage(float Damage, UDamageType DamageType, UActor DamagedActor, UActor DamageCauser) { }

    /** spawns and initializes the PlayerState for this Controller */
    public virtual void InitPlayerState() { }

    /**
     * Called from game mode upon end of the game, used to transition to proper state. 
     * @param EndGameFocus Actor to set as the view target on end game
     * @param bIsWinner true if this controller is on winning team
     */
    public virtual void GameHasEnded(UActor EndGameFocus = null, bool bIsWinner = false) { }

    /**
	 * Returns Player's Point of View
	 * For the AI this means the Pawn's 'Eyes' ViewPoint
	 * For a Human player, this means the Camera's ViewPoint
	 *
	 * @output	out_Location, view location of player
	 * @output	out_rotation, view rotation of player
	*/
    public virtual void GetPlayerViewPoint(Vector3 Location, Quaternion Rotation) { }


    /** If controller has any navigation-related components then this function 
     *	makes them update their cached data */
    public virtual void UpdateNavigationComponents() { }

    /** Aborts the move the controller is currently performing */

    public virtual void StopMovement() { }

    /** State entered when inactive (no possessed pawn, not spectating, etc). */
    protected virtual void BeginInactiveState() { }

    /** State entered when inactive (no possessed pawn, not spectating, etc). */
    protected virtual void EndInactiveState() { }
    /** Event when this controller instigates ANY damage */
    protected void ReceiveInstigatedAnyDamage(float Damage, UDamageType DamageType, UActor DamagedActor, UActor DamageCauser) { }
}

