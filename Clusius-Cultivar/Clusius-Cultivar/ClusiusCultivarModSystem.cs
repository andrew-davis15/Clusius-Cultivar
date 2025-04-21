using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace ClusiusCultivar
{
    public class ClusiusCultivarModSystem : ModSystem
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         * 
         *  Clusius Cultivar v1.0.0
         * 
         * @Author@ : Drex_ (Discord @ Drex#2967)
         * 
         * Thank you so much for checking out our mod! We greatly appreciate you taking the time to play it and we hope you enjoy it! 
         * 
         * Below doesn't do anything special except to register the new block types and items for the mod. I was forced to create a new crop 
         * type in order to allow for the new tulip crops to be added to the game, and then I wanted to allow for tulip blocks to have 
         * random-ish drops. But decided to make the class more generalized so that it could be used for other crops/blocks in the future.
         * 
         * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         */


        public override void Start(ICoreAPI api)
        {
            Mod.Logger.Notification("Hello from template mod: " + api.Side);
            api.RegisterBlockClass(Mod.Info.ModID + ".BlockCropVariable", typeof(ClusiusCultivar.Blocks.BlockCropVariable));
            api.RegisterItemClass(Mod.Info.ModID + ".TulipSeeds", typeof(ClusiusCultivar.Items.TulipSeeds));
        }

    }
}
