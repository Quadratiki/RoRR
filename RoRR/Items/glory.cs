using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using RoR2.Items;
using RoRR;
using RoRR.Utils;
using System;
using System.Linq;
using UnityEngine;
using static RoR2.OverlapAttack;
using BuffDef = RoR2.BuffDef;
using CharacterBody = RoR2.CharacterBody;
using DamageInfo = RoR2.DamageInfo;
using DamageReport = RoR2.DamageReport;
using GlobalEventManager = RoR2.GlobalEventManager;
using ItemDef = RoR2.ItemDef;
using ItemDisplay = RoR2.ItemDisplay;
using Util = RoR2.Util;



namespace flex
{
    public class glory : RoRR.Items.ItemBase<glory>

    {
        public static BuffDef gloru;


        public override string ItemName => "glory";

        public override string ItemLangTokenName => "glory";

        public override string ItemPickupDesc => "jj";

        public override string ItemFullDescription => "<>";
        public override string ItemLore => "ur mom";


        public override ItemTier Tier => ItemTier.Boss;


        public static GameObject ItemBodyModelPrefab;

        public override GameObject ItemModel => Main.bookasset.LoadAsset<GameObject>("agonyeater.prefab");

        public override Sprite ItemIcon => Main.bookasset.LoadAsset<Sprite>("templateicon.png");

        private void CreateBuff()
        {
            glory.gloru = ScriptableObject.CreateInstance<BuffDef>();
            glory.gloru.buffColor = new Color((float)byte.MaxValue, (float)byte.MaxValue, (float)byte.MaxValue);
            glory.gloru.canStack = true;
            glory.gloru.isDebuff = false;
            glory.gloru.iconSprite = Main.bookasset.LoadAsset<Sprite>("templatebafficon.png");
            ContentAddition.AddBuffDef(glory.gloru);

        }
        public override void Init(ConfigFile config)
        {
            this.CreateItem();
            this.Hooks();
            this.CreateBuff();
            this.CreateLang();
            this.CreateItemDisplayRules();

        }

        public int GetCount(CharacterBody body)
        {
            if (!body || !body.inventory) { return 0; }

            return body.inventory.GetItemCount(ItemDef);
        }
        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemBodyModelPrefab = ItemModel;
            var itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = ItemHelpers.ItemDisplaySetup(ItemBodyModelPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("glory", new RoR2.ItemDisplayRule[]
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
            On.RoR2.CharacterBody.OnTakeDamageServer += CharacterBody_OnTakeDamageServer;
        }



        private void CharacterBody_OnTakeDamageServer(On.RoR2.CharacterBody.orig_OnTakeDamageServer orig, CharacterBody self, DamageReport damageReport)
        {
            var invcount = GetCount(self);
            if (invcount > 0)
            {

                self.SetBuffCount(gloru.buffIndex, 0);
                int buff = self.GetBuffCount(gloru);

                self.attackSpeed = self.attackSpeed * 0.7f + (0.7f * buff / 35f);
                self.baseDamage = self.baseDamage * 0.7f + (1.5f * buff / 5f);  
            }
        }

        public void OnHitEnemy(DamageReport report)
        {

            CharacterBody victimInfo = report.victimBody;
            CharacterBody attackerInfo = report.attackerBody;
            var invcount = GetCount(attackerInfo);
            if (invcount > 0)
            {
                int buff = attackerInfo.GetBuffCount(gloru);

                attackerInfo.attackSpeed = attackerInfo.attackSpeed * 0.7f + (0.7f * buff / 35f);
                attackerInfo.baseDamage = attackerInfo.baseDamage * 0.7f + (1.5f * buff / 5f);
                
                if (buff < invcount * 50)
                {
                    attackerInfo.AddBuff(gloru);
                }
            }
        }
    }
}



