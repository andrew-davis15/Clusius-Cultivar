using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.API.Datastructures;

/*
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Clusius Cultivar 1.0.0
 * 
 * @Author@ : Drex_ (Discord @ Drex#2967)
 * 
 * Credit to SpearAndFang on GitHub for much of the code below currently in use in the wildfarmingrevival project
 * Updated portions of it to be more applicabe for this project, however they deserve credit as much of this is unchanged from
 * their original project
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 */


namespace ClusiusCultivar.Items

{
	public class TulipSeeds : Item
	{

        //
        //
        public override void OnHeldInteractStart(ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
    {
        //checking for nulls
        if (blockSel == null || byEntity?.World == null) { return; };

        var world = byEntity.World;
        var ground = world.BlockAccessor.GetBlock(blockSel.Position);
        var cropPos = blockSel.Position.UpCopy(1);
        var removedBlock = world.BlockAccessor.GetBlock(cropPos);

        //Determining the color of the selected TulipSeed and updating onto new block
        var seedColor = itemslot.Itemstack.Collectible.CodeEndWithoutParts(2);
            System.Diagnostics.Debug.WriteLine("Debug Seed Color: " + seedColor);
            var tulipCrop = world.BlockAccessor.GetBlock(new AssetLocation("clusiuscultivar:crop-tulip-" + seedColor + "-1"));
            System.Diagnostics.Debug.WriteLine("Debug new crop block: " + tulipCrop);

        IPlayer byPlayer = null;

        if(byEntity is EntityPlayer player) { byPlayer = byEntity.World.PlayerByUid(player.PlayerUID); }

            //Checking to see if the Tulip Crop can be placed at this location
            //Credit to ### for this section as well!
            /* if (!byEntity.World.Claims.TryAccess(byPlayer, cropPos, EnumBlockAccessFlags.BuildOrBreak))
            {
                return;
            }
            if (!ground.SideSolid[blockSel.Face.Index])
            {
                return;
            }
            if (removedBlock.Replaceable < 9501)
            {
                return;
            }
            if (ground.Fertility <= 0)
            {
                return;
            }
            */


            // Placing the plant
            world.BlockAccessor.SetBlock(tulipCrop.BlockId, cropPos);
            System.Diagnostics.Debug.WriteLine("Tried planting : " + tulipCrop.BlockId);
            byEntity.World.PlaySoundAt(new AssetLocation("game: sounds/block/plant"), cropPos.X, cropPos.Y, cropPos.Z, byPlayer);


            ((byEntity as EntityPlayer)?.Player as IClientPlayer)?.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);

            if (byPlayer?.WorldData?.CurrentGameMode != EnumGameMode.Creative)
            {
                itemslot.TakeOut(1);
                itemslot.MarkDirty();
            }
            handling = EnumHandHandling.PreventDefault;
        }

        public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
        {
            base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
            string color = inSlot.Itemstack.Collectible.LastCodePart(2);
            System.Diagnostics.Debug.WriteLine(color); // Replaced 

            Block cropBlock = world.GetBlock(new AssetLocation("Clusius-Cultivar:tulip-" + color + "-1"));
            if (cropBlock == null || cropBlock.CropProps == null) return;

            dsc.AppendLine(Lang.Get("soil-nutrition-requirement") + cropBlock.CropProps.RequiredNutrient);
            dsc.AppendLine(Lang.Get("soil-nutrition-consumption") + cropBlock.CropProps.NutrientConsumption);

            double totalDays = cropBlock.CropProps.TotalGrowthDays;
            if (totalDays > 0)
            {
                var defaultTimeInMonths = totalDays / 12;
                totalDays = defaultTimeInMonths * world.Calendar.DaysPerMonth;
            }
            else
            {
                totalDays = cropBlock.CropProps.TotalGrowthMonths * world.Calendar.DaysPerMonth;
            }

            totalDays /= api.World.Config.GetDecimal("cropGrowthRateMul", 1);

            dsc.AppendLine(Lang.Get("soil-growth-time") + " " + Lang.Get("count-days", Math.Round(totalDays, 1)));
            dsc.AppendLine(Lang.Get("crop-coldresistance", Math.Round(cropBlock.CropProps.ColdDamageBelow, 1)));
            dsc.AppendLine(Lang.Get("crop-heatresistance", Math.Round(cropBlock.CropProps.HeatDamageAbove, 1)));
        }

        public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
        {
            return new WorldInteraction[] {
                new WorldInteraction()
                {
                    ActionLangCode = "heldhelp-plant",
                    MouseButton = EnumMouseButton.Right,
                }
            }.Append(base.GetHeldInteractionHelp(inSlot));

        }

    }

}
