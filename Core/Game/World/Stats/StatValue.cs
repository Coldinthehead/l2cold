namespace Core.Game.World.Stats
{
    public class StatValue
    {
        public int Value => _totalValue;

        private float _baseValue;
        private float _multiplier;
        private float _unscaledValue;
        private float _baseMultiplier;
        private int _totalValue;

        public StatValue(float baseValue = 0)
        {
            _baseValue = baseValue;
            _multiplier = 1f;
            _baseMultiplier = 1f;
            _unscaledValue = 0f;
            Recalculate();
        }

        public void Add(float value)
        {
            _baseValue += value;
            Recalculate();
        }

        public void Sub(float value)
        {
            _baseValue -= value;
            Recalculate();
        }

        public void SetBaseMultipilier(float multiplier)
        {
            _baseMultiplier = multiplier;
            Recalculate();
        }


        public void AddMultiplier(float multiplier)
        {
            _multiplier += multiplier;
            Recalculate();
        }

        public void SubMultipleir(float multiplier)
        {
            _multiplier -= multiplier;
            Recalculate();
        }

        public void AddUnscaled(float value)
        {
            _unscaledValue += value;
            Recalculate();
        }

        public void SubUnscaled( float value)
        {
            _unscaledValue -= value;
            Recalculate();
        }

        private void Recalculate()
        {
            _totalValue =(int) ((_baseValue * _baseMultiplier * _multiplier) + _unscaledValue);
        }

        
    }
}
