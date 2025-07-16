using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using ItemDisplay = RoR2.ItemDisplay;
using RoRR;
using RoRR.Utils;
using CharacterBody = RoR2.CharacterBody;



namespace Scroll
{
    public class Scroll : RoRR.Items.ItemBase<Scroll>

    {

        public override string ItemName => "Scroll";

        public override string ItemLangTokenName => "Scroll";

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
            rules.Add("Scroll", new RoR2.ItemDisplayRule[]
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
                args.baseAttackSpeedAdd = invcount * 0.3f;
                args.damageMultAdd = invcount * 0.4f;
                args.critAdd = invcount + 20;
                {
                    if (invcount >= 2)
                    {
                        //И ещё не много приколов
                        args.attackSpeedMultAdd = invcount * 0.15f;
                        args.damageMultAdd = invcount * 0.2f;
                        args.critDamageMultAdd = invcount * 0.3f;
                    }
                    if (invcount >= 3)
                    {
                        //Нехуй много стакать )) 
                        args.baseHealthAdd = invcount / 2;
                        args.healthMultAdd = invcount / 2;
                        
                    }
                    if (invcount >= 4)
                    {
                        //Еблан ?
                        args.baseHealthAdd = invcount / 10;
                        args.healthMultAdd = invcount / 5 ;
                       
                    }
                    if ((invcount >= 5) && (invcount < 6))
                    {
                        //Ладно на приколы ))
                        args.baseAttackSpeedAdd = (invcount + 1) * 0.5f;
                        args.baseDamageAdd = (invcount + 30) * 0.3f;
                        args.critAdd = (invcount + 70) * 0.2f;
                        args.attackSpeedMultAdd = (invcount + 10) * 0.2f;
                        args.damageMultAdd = (invcount + 35) * 0.2f;
                        args.critDamageMultAdd = (invcount + 50) * 0.2f;
                        args.healthMultAdd = (invcount + 45) * 2;
                    }
                }


            }
        }
    }
}
