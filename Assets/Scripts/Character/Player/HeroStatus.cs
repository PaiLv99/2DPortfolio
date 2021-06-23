using UnityEngine;

[System.Serializable]
public class HeroStatus : Status
{
    public int HP { get; set; }
    public int AP { get; set; }
    public int EXP { get; set; }
    public int DP { get; set; }
    public int LEVEL { get; protected set; }
    public int MAXHP { get; protected set; }
    public int InstanceID { get; protected set; }


    public int MAXEXP { get; private set; }

    public Sprite SPRITE { get; private set; }
    public Sprite STATUSSPRITE { get; private set; }

    public Data.ItemData Weapon { get; set; }
    public Data.ItemData Armor { get; set; }
    public Data.ItemData RangeWeapon { get; set; }

    public Data.HeroData Data;

    public HeroStatus(Data.HeroData data)
    {
        HP = data._maxHp;
        MAXHP = data._maxHp;
        AP = data._ap;
        DP = data._dp;
        EXP = 0;
        MAXEXP = data._exp;
        LEVEL = data._level;
        Data = data;
        SPRITE = data._sprite;
        STATUSSPRITE = data._statusSprite;
    }

    public void StartEquip()
    {
        Weapon = DB.Instance.GetItemData(Data._startWeapon);
        Armor = DB.Instance.GetItemData(Data._startArmor);
        RangeWeapon = DB.Instance.GetItemData(Data._startProjectile);
        UIMng.Instance.CallEvent(UIList.Inventory, "EquipStart", Weapon);
        UIMng.Instance.CallEvent(UIList.Inventory, "EquipStart", Armor);

        // test
        UIMng.Instance.CallEvent(UIList.Inventory, "AddSlot", DB.Instance.GetItemData("PotionOfHealing"));
        UIMng.Instance.CallEvent(UIList.Inventory, "AddSlot", DB.Instance.GetItemData("PotionOfFlame"));
        UIMng.Instance.CallEvent(UIList.Inventory, "AddSlot", DB.Instance.GetItemData("PotionOfFreeze"));
        UIMng.Instance.CallEvent(UIList.Inventory, "AddSlot", DB.Instance.GetItemData("PotionOfInvisibility"));
        UIMng.Instance.CallEvent(UIList.Inventory, "AddSlot", DB.Instance.GetItemData("PotionOfVision"));
        UIMng.Instance.CallEvent(UIList.Inventory, "AddSlot", DB.Instance.GetItemData("ScrollOfEnchant"));
        UIMng.Instance.CallEvent(UIList.Inventory, "AddSlot", DB.Instance.GetItemData("ScrollOfMapping"));
        UIMng.Instance.CallEvent(UIList.Inventory, "AddSlot", DB.Instance.GetItemData("ScrollOfTeleportation"));
        UIMng.Instance.CallEvent(UIList.Inventory, "AddSlot", DB.Instance.GetItemData("ScrollOfIdentify"));


        if (RangeWeapon != null)
        {
            UIMng.Instance.CallEvent(UIList.Inventory, "AddSlot", RangeWeapon);
            Inventory inventory = UIMng.Instance.Get<Inventory>(UIList.Inventory);
            UIMng.Instance.CallEvent(UIList.HUD, "AddQuickSlot", inventory.GetSlot(RangeWeapon));
        }
    }

    public override void Damage(int value) 
    {
        TextEffect effect = EffectMng.Instance.Pop("TextEffect") as TextEffect;

        // 주사위 굴리기
        int min = Armor._enchant;
        int max = Armor._tear * (2 * Armor._enchant) + 1;
        int armor = Random.Range(min, max);
        value -= armor;

        if (value <= 0)
        {
            value = 0;
            effect.SetText("막음");
            effect.CallEvent(GameMng.CharMng.GetHero().transform.position);

            //_charPopUp.BlockPop();
            return;
        }

        float prevHP = (float)HP / MAXHP;
        //UIMng.Instance.CallEvent(UIList.HUD, "DamageUpdateHP", prevHP);

        HP -= value;
        UIMng.Instance.CallEvent(UIList.HUD, "UpdateHP");
        UIMng.Instance.CallEvent(UIList.HUD, "HPCut", prevHP);
        effect.SetText(value.ToString());
        effect.CallEvent(GameMng.CharMng.GetHero().transform.position + new Vector3(0.5f, 0.5f));
        //_charPopUp.DamagePop(value);

        if (HP <= 0)
            Die();
    }

    public override void MagicDamage(int value)
    {
        float prevHP = (float)HP / MAXHP;
        UIMng.Instance.CallEvent(UIList.HUD, "DamageUpdateHP", prevHP);

        HP -= value;
        UIMng.Instance.CallEvent(UIList.HUD, "UpdateHP");
        //_charPopUp.DamagePop(value);

        TextEffect effect = EffectMng.Instance.Pop("TextEffect") as TextEffect;
        effect.SetText(value.ToString());
        effect.CallEvent(GameMng.CharMng.GetHero().transform.position);


        if (HP <= 0)
            Die();
    }

    public override int GetAp()
    {
        // 무기가 없을 때 사용할 기본 공격력
        int baseValue = Random.Range(0, AP + 1);

        // 기본 힘 다이스 + 무기 공격력 다이스 굴림
        int min = 1 + Weapon._tear + Weapon._enchant;
        int max = Weapon._value + (1 + Weapon._tear) * Weapon._enchant + 1;
        int value = Random.Range(min, max);

        if (Weapon != null)
            return value;

        return baseValue;
    }

    public void GetEXP(int exp) 
    { 
        EXP += exp;
        if (EXP >= MAXEXP)
            LevelUp();

        TextEffect effect = EffectMng.Instance.Pop("TextEffect") as TextEffect;
        effect.SetText("경험치 +" + exp.ToString());
        effect.CallEvent(GameMng.CharMng.GetHero().transform.position);
        UIMng.Instance.CallEvent(UIList.HUD, "UpdateEXP");
    }

    public void LevelUpPotion()
    {
        LevelUp();
    }

    private void LevelUp()
    {
        LEVEL++;
        MAXHP += 5;
        HP = MAXHP;
        AP += 1;
        DP += 1;
        EXP = 0;
        MAXEXP += 5;
        TextEffect effect = EffectMng.Instance.Pop("TextEffect") as TextEffect;
        effect.SetText("레벨업!");
        effect.CallEvent(GameMng.CharMng.GetHero().transform.position);

        UIMng.Instance.CallEvent(UIList.HUD, "UpdateLevel");
    }

    private void Die()
    {
        GameMng.CharMng.GetHero().Death();

        //GameMng.Instance.GameOver();
        UIMng.Instance.SetActive(UIList.GameOver, true);
        UIMng.Instance.CallEvent(UIList.GameOver, "Open");
    }

    public void Healing(int value)
    {
        HP += value;

        if (HP >= MAXHP)
            HP = MAXHP;

        UIMng.Instance.CallEvent(UIList.HUD, "UpdateHP");
        UIMng.Instance.CallEvent(UIList.HUD, "TextBox", "당신은 치유되기 시작했다.");
        //EffectMng.Instance.Pop("Healing").CallEvent()
        //GameMng.Instance.HeroTurnOut();
    }

    private bool Dodge()
    {
        int rand = Random.Range(0, 20);

        if (rand == 0)
            return true;

        if (rand == 19)
            return false;

        return false;
    }
}