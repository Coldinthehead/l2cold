using Core.Engine;
using Core.Game.World.Stats;

namespace Core.Game.World.Components
{
    public class EntityStats : Component
    {
        public int MAtkSpd => _mAtkSpd.Value;
        public int PAtkSpd => _pAtkSpd.Value;
        public int RunSpd => _runSpd.Value;
        public int WalkSpd => _runSpd.Value / 2;
        public int STR => _str.Value;
        public int DEX => _dex.Value;
        public int CON => _con.Value;
        public int INT => _int.Value;
        public int MEN => _men.Value;
        public int WIT => _wit.Value;
        public int PAtk => _pAtk.Value;
        public int MAtk => _mAtk.Value;
        public int PDef => _pDef.Value;
        public int MDef => _mDef.Value;
        public int Crit => _crit.Value;
        public int Evasion => _evasion.Value;
        public int Accuracy => _accuracy.Value;

        public readonly StatValue _mAtkSpd = new StatValue();
        public readonly StatValue _pAtkSpd = new StatValue();
        public readonly StatValue _runSpd = new StatValue();
        public readonly StatValue _str = new StatValue();
        public readonly StatValue _dex = new StatValue();
        public readonly StatValue _con = new StatValue();
        public readonly StatValue _int = new StatValue();
        public readonly StatValue _men = new StatValue();
        public readonly StatValue _wit = new StatValue();
        public readonly StatValue _pAtk = new StatValue();
        public readonly StatValue _mAtk = new StatValue();
        public readonly StatValue _pDef = new StatValue();
        public readonly StatValue _mDef = new StatValue();
        public readonly StatValue _crit = new StatValue();
        public readonly StatValue _evasion = new StatValue();
        public readonly StatValue _accuracy = new StatValue();

    }
}
