using System;

namespace CoffeeOrder.Helpers
{
    public class OrderChecker
    {
        private readonly Random _random;
        private int _index;

        private readonly string[] _status =
            {"Ordine accettato!", "Preparazione prodotto...", "Assaggio per capire come è venuto...", "In consegna...", "Consegnato!", "In attesa"};

        public OrderChecker(Random random)
        {
            this._random = random;
        }

        public CheckResult GetUpdate()
        {
            if (_status.Length > _index)
            {
                var result = new CheckResult
                {
                    New = true,
                    Update = _status[_index],
                    Finished = _status.Length - 1 == _index
                };

                if (result.Finished)
                    _index = 0;
                else
                    _index++;

                return result;
            }

            return new CheckResult { New = false };
        }
    }

    public class CheckResult
    {
        public bool New { get; set; }
        public string Update { get; set; }
        public bool Finished { get; set; }
    }
}
