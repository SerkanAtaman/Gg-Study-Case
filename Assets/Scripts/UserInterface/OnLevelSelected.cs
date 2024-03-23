using GG.LevelSystem;
using SeroJob.BroadcastingSystem;
using UnityEngine;

namespace GG.UserInterface
{
    // Adjust the broadcast channel asset's path and name as you wish
    [CreateAssetMenu(menuName = "GG/UserInterface/OnLevelSelected")]
    public class OnLevelSelected : BroadcastChannel
    {
        // DO NOT REMOVE THIS FUNCTION. IT IS ESSENTIAL TO CHANNEL TO WORK
        protected override void OnEnable()
        {
            // -THE CONSTRUCTOR OF THE BROADCAST CHANNEL-

            // content.Construct method has to be call in load time in order to construct the channel to be used.
            // Example usage: content.Construct(this);
            //
            // Broadcast channels can be defined with up to 4 arguments in their generic definition.
            // To define the channels with an argument you just need to add the argument to the content.Construct method.
            // Example usages of content.Construct method with arguments:
            // -- content.Construct<string>(this);
            // -- content.Construct<float>(this);
            // -- content.Construct<string, int, float>(this);

            // Broadcast channels can be defined with any type of custom arguments.
            // -- content.Construct<T1, T2, T3, T4>(this);


            base.OnEnable();
            content.Construct<LevelDataHolder>(this);
        }
    }
}