using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using ItemDisplay = RoR2.ItemDisplay;
using RoRR;
using RoRR.Utils;
using CharacterBody = RoR2.CharacterBody;



namespace roll
{
    public class roll : RoRR.Items.ItemBase<roll>

    {

        public override string ItemName => "roll";

        public override string ItemLangTokenName => "roll";

        public override string ItemPickupDesc => "jj";

        public override string ItemFullDescription => "<>";
        public override string ItemLore => "ur mom";


        public override ItemTier Tier => ItemTier.Tier3;


        public static GameObject ItemBodyModelPrefab;

        public override GameObject ItemModel => Main.bookasset.LoadAsset<GameObject>("agonyeater.prefab");

        public override Sprite ItemIcon => Main.bookasset.LoadAsset<Sprite>("templateicon.png");

        public override void Init(ConfigFile config)
        {
            this.CreateItem();
            this.Hooks();
            this.CreateLang();
            this.CreateItemDisplayRules();

        }


        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemBodyModelPrefab = ItemModel;
            var itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = ItemHelpers.ItemDisplaySetup(ItemBodyModelPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("roll", new RoR2.ItemDisplayRule[]
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
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }
        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody self, RecalculateStatsAPI.StatHookEventArgs args)
        {
            var invcount = GetCount(self);
            if (invcount >= 1)
            {
                //Лови приколы
                args.baseAttackSpeedAdd = self.attackSpeed + 35;
                args.baseDamageAdd = self.damage + 40;
                args.critAdd = self.crit + 30;
            if (invcount >= 2)
                //И ещё не много приколов
                args.attackSpeedMultAdd =self.attackSpeed + 48;
                args.damageMultAdd =self.damage + 69;
                args.critDamageMultAdd =self.critMultiplier + 14;
            if (invcount >=3 )
               //Нехуй много стакать )) 
                args.baseHealthAdd = self.baseMaxHealth * 0.5f;
                args.healthMultAdd = self.baseMaxHealth * 0.5f;
                args.moveSpeedMultAdd = self.moveSpeed * 0.5f;
            if (invcount >= 4 )
                //Еблан ?
                args.baseHealthAdd = self.baseMaxHealth * 0.25f;
                args.healthMultAdd = self.baseMaxHealth * 0.25f;
                args.moveSpeedMultAdd = self.moveSpeed * 0.25f;
            if ((invcount >= 5) && (invcount < 6))
                //Ладно на приколы ))
                args.baseAttackSpeedAdd = (self.attackSpeed + 35) * 70f;
                args.baseDamageAdd = (self.damage + 80) * 70f;
                args.critAdd = (self.crit + 65) * 70f ;
                args.attackSpeedMultAdd = (self.attackSpeed + 70) * 70f;
                args.damageMultAdd = (self.damage + 100) * 50f;
                args.critDamageMultAdd = (self.critMultiplier + 50) * 70f;
            }
                    
                    
      }
    }
}

