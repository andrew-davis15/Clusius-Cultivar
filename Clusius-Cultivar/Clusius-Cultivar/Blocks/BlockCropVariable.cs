using Vintagestory.API.Common;
using Vintagestory.GameContent;
using Vintagestory.API.MathTools;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

/*
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Clusisus Cultivar 1.0.0
 * 
 * @Author@ : Drex_ (Discord @ Drex#2967)
 * 
 * "Fine, I'll do it myself"
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 */


namespace ClusiusCultivar.Blocks
{
    /*
    * BlockCropVariable implements an override of the GetDrops method of the BlockCrop Block Type in order to allow for variable crop drops/drop rates
    * but does not change any other properties or methods of the CropBlock class. 
    * 
    * Example: Tulip
    * Add all possible drops to the drop list for the crop, and then select the probability of a random drop to be selected by adjusting the randomPosition
    * varible which is defined below. Currently this is set to 0-39, which means that there is a 1 in 40 chance of a random drop being selected.
    * 
    */
    internal class BlockCropVariable : BlockCrop
    {

        public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1)
        {
            // Check if the block is a farmland block and if it is, get the BlockEntityFarmland
            BlockEntityFarmland beFarmland = world.BlockAccessor.GetBlockEntity(pos.DownCopy()) as BlockEntityFarmland;

            // This is the integer that will be used to determine whether or not a random drop will be included in the drop list
            // By adjusting the range of the Rand() function, we can get a different probability of a random drop being selected
            // For example, if we want a 1 in 40 chance of a random drop being selected, we can set the range to 0-39
            int randomPosition = world.Rand.Next(0, 39);

            System.Diagnostics.Debug.WriteLine("Debug Random Position: " + randomPosition);

            // Get an array of drops/qty for all potential drops by calling the base method
            ItemStack[] baseDrops = base.GetDrops(world, pos, byPlayer, dropQuantityMultiplier);

            // Get the block entity for the crop on the currently selected farmland block
            Block currentCrop = world.BlockAccessor.GetBlock(pos);

            //Ensure that we have valid values for both current crop and base drops
            if (currentCrop == null || baseDrops == null)
            {
                return baseDrops;
            }

            // Get the code for the current crop and save it for comparison purposes
            string currentCropString = currentCrop.Code;
            System.Diagnostics.Debug.WriteLine("Debug Current Crop from Drops: " + currentCropString); 

            List<ItemStack> newDrops = new List<ItemStack>();

            for (int i = 0; i < baseDrops.Length; i++)
            {
                //Checking for the Variant of the crop being used/stripping stage
                string variantCode = baseDrops[i].Collectible.CodeEndWithoutParts(1);
                
                //Ensure that "base" drops that match the variant we checked for above are included in the drop table as guaranteed drops 
                // (This can be disabled by removing the following if statement)
                if (currentCropString.Contains(variantCode))
                {
                    newDrops.Add(baseDrops[i]);
                }

                //Check to see if the current index position in the full base drops list matches the randomly generated integer from above
                //If it does match, then the player "rolled" the drop and we will add the drop to the new drops list

                if (i == randomPosition)
                {
                    newDrops.Add(baseDrops[i]);
                }
            }

            // Return the new list of drops!
            return newDrops.ToArray();
        }


    }
}