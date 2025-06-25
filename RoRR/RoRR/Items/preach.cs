using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using ItemDisplay = RoR2.ItemDisplay;
using RoRR;
using RoRR.Utils;
using CharacterBody = RoR2.CharacterBody;



namespace preach
{
    public class preach : RoRR.Items.ItemBase<preach>

    {

        public override string ItemName => "preach";

        public override string ItemLangTokenName => "preach";

        public override string ItemPickupDesc => "jj";

        public override string ItemFullDescription => "<>";
        public override string ItemLore => "ur mom";


        public override ItemTier Tier => ItemTier.Tier2;


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
            rules.Add("preath", new RoR2.ItemDisplayRule[]
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
               
                    args.baseHealthAdd = invcount * 10;
                args.baseRegenAdd = invcount + 0.5f;            
                args.armorAdd = invcount * 0.1f;
            }
        }



    }
}
           
