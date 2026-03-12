using System;

namespace Economy
{
    public class CoinWallet
    {
        public int Coins { get; private set; }

        public event Action<int> CoinsChanged;

        public void AddCoins(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            Coins += amount;
            CoinsChanged?.Invoke(Coins);
        }
    }
}
