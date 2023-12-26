using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public enum WeaponType
    {
        PyromancyCaster,
        FaithCaster,
        SpellCaster,
        Unarmed,
        StraightSword,
        SmallShield,
        Shield,
        Bow,
        _Spear,
        _GreatSword
    }

    public enum AmmoType
    {
        Arrow,
        Bolt
    }

    public enum AttackType
    {
        light,
        heavy,
        charge
        //light Attack 01
        //light Attack 02
        //heavy Attack 01
        //heavy Attack 02
        //etc, etc, ...
    }

    public enum AICombatStyle
    {
        swordAndShield,
        archer
    }

    public enum AIAttackActionType
    {
        meleeAttackAction,
        magicAttackAction,
        rangedAttackAction
    }

    public enum DamageType
    {
        Physical,
        Fire,
        _Lightning,
        _Dark,
        _Magic,
        _Bleed
    }

    public enum BuffClass
    {
        Physical,
        Fire,
        _Lightning,
        _Dark,
        _Magic,
        _Bleed
    }

    public enum EffectParticleType
    {
        Poison,
        _Bleed,
        _Curse
    }

    public enum _EffectWeaponDamageType
    {
        _BleedWeaponType,
        _CurseWeaponType,
        _NormalWeaponType
    }

    public enum EncumbranceLevel
    {
        Light, //LIGHT ROLL
        Medium, //MEDIUM ROLL
        Heavy, //HARD ROLL
        Overloaded //WALK SPEED ONLY
    }

    public class Enums : MonoBehaviour
    {

    }
}