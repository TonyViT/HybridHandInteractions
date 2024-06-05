
namespace HybridHandInteractions
{
    /// <summary>
    /// Interactable that can react to touch interactions. When it collides with a hand interactor
    /// it can trigger some events
    /// </summary>
    public class HandTouchInteractable: HandInteractable
    {
        //the base class already has the logic to launch events when its status change (so when the interactable
        //collides with the colliders of the interactor), so we don't need to do anything here
    }

}