using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using ItemDisplay = RoR2.ItemDisplay;
using Util = RoR2.Util;
using CharacterBody = RoR2.CharacterBody;
using DamageInfo = RoR2.DamageInfo;
using GlobalEventManager = RoR2.GlobalEventManager;
using DamageReport = RoR2.DamageReport;
using BuffDef = RoR2.BuffDef;
using ItemDef = RoR2.ItemDef;
using RoR2.Items;
using RoRR;
using RoRR.Utils;


namespace Exhauster
{
    public class Exhauster : RoRR.Items.ItemBase<Exhauster>

    {
        public static BuffDef AccumulationDebuff;
        public static BuffDef AccumulationCooldown;

        public override string ItemName => "Exhauster";

        public override string ItemLangTokenName => "EXHAUSTER";

        public override string ItemPickupDesc => "On hit applies a<style=cIsDamage> debuff</style> when enemy <style=cIsDamage>loses</style> another <style=cIsDamage>effect</style> debuff <style=cIsDamage>deals damage</style>";

        public override string ItemFullDescription => "On hit applies an<style=cIsDamage> effect</style> with a <style=cIsDamage>10% chance</style> <style=cStack>(+10% more per stack)</style> when the <style=cIsDamage>enemy loses</style> another debuff it deals damage.Damage depends on <style=cIsDamage>missing health</style><style=cStack>(from 1% to 24%)</style> and <style=cIsDamage>debuffs count</style><style=cStack>(+100% per debuff)</style>";
        public override string ItemLore => "ur mom";


        public override ItemTier Tier => ItemTier.Tier3;

        public override GameObject ItemModel => Main.bookasset.LoadAsset<GameObject>("agonyeater.prefab");

        public static GameObject ItemBodyModelPrefab;

        public override Sprite ItemIcon => Main.bookasset.LoadAsset<Sprite>("templateicon.png");


        private void CreateBuff()
        {
            Exhauster.AccumulationDebuff = ScriptableObject.CreateInstance<BuffDef>();
            Exhauster.AccumulationDebuff.buffColor = new Color((float)byte.MaxValue, (float)byte.MaxValue, (float)byte.MaxValue);
            Exhauster.AccumulationDebuff.canStack = false;
            Exhauster.AccumulationDebuff.isDebuff = true;
            Exhauster.AccumulationDebuff.iconSprite = Main.bookasset.LoadAsset<Sprite>("templatebafficon.png");
            ContentAddition.AddBuffDef(Exhauster.AccumulationDebuff);

            Exhauster   .AccumulationCooldown = ScriptableObject.CreateInstance<BuffDef>();
            Exhauster.AccumulationCooldown.canStack = true;
            Exhauster.AccumulationCooldown.isDebuff = true;
            Exhauster.AccumulationCooldown.isHidden = true;
            ContentAddition.AddBuffDef(Exhauster.AccumulationCooldown);

        }



        public override void Init(ConfigFile config)
        {
            this.CreateItem();
            this.Hooks();
            this.CreateBuff();
            this.CreateLang();
            this.CreateItemDisplayRules();
        }


        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemBodyModelPrefab = ItemModel;
            var itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = ItemHelpers.ItemDisplaySetup(ItemBodyModelPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("Exhauster", new RoR2.ItemDisplayRule[]
            {


                new RoR2.ItemDisplayRule
                {
                    ruleType = RoR2.ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "body",
                    localPos = new Vector3(0,0,0),
                    localAngles = new Vector3(0,0,0),
                    localScale = new Vector3(1,1,1)
                }
            });
            return rules;

        }

        public override void Hooks()
        {
            GlobalEventManager.onServerDamageDealt += OnHitEnemy;
            On.RoR2.CharacterBody.OnBuffFinalStackLost += CharacterBody_OnBuffFinalStackLost;
        }



        public void OnHitEnemy(DamageReport report)
        {


            CharacterBody victimInfo = report.victimBody;
            CharacterBody attackerInfo = report.attackerBody;

            var invcount = GetCount(attackerInfo);
            if (invcount > 0)
                if (Util.CheckRoll((invcount * 10), attackerInfo.master))
                {
                    victimInfo.AddTimedBuff(AccumulationDebuff, 6.5f);
                }

        }

        private void CharacterBody_OnBuffFinalStackLost(On.RoR2.CharacterBody.orig_OnBuffFinalStackLost orig, CharacterBody self, BuffDef buffDef)
        {

            orig(self, buffDef);
            self.AddBuff(AccumulationCooldown);
            if (self.HasBuff(AccumulationDebuff))
            {
                if (!buffDef.isHidden)
                { 
                float Health = self.healthComponent.fullHealth;
                float Currenthealth = self.healthComponent.health;
                float missinghealth = Health - Currenthealth;
                float healthcap = Health * 0.24f;
                float healthpercentage = missinghealth * 0.2f;
                int accumcount = self.GetBuffCount(AccumulationCooldown);
                    {
                        DamageInfo damageinfo1 = new DamageInfo();
                        {
                            damageinfo1.damage = (float)MathHelpers.InverseHyperbolicScaling(healthpercentage, 0.012f, healthcap, accumcount);
                            damageinfo1.damageType = DamageType.Generic;
                            damageinfo1.crit = false;
                            damageinfo1.damageColorIndex = DamageColorIndex.WeakPoint;
                            damageinfo1.position = self.corePosition;
                            damageinfo1.force = Vector3.zero;
                            damageinfo1.procChainMask = new ProcChainMask();
                        }
                        self.healthComponent.TakeDamage(damageinfo1);
                    }
                }

            }
        }

    }
}







