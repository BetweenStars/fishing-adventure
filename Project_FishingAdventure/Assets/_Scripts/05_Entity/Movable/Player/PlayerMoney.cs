using System;
using System.Data.SqlTypes;
using UnityEngine;

public class PlayerMoney
{
    public event Action<double> OnMoneyChanged;
    public double money { get; private set; } = 0;

    public PlayerMoney(double money){ this.money = money; }

    public void AddMoney(double value)
    {
        money += value;
        OnMoneyChanged?.Invoke(money);
    }

    public bool TrySpendMoney(double value)
    {
        if (money >= value)
        {
            money -= value;
            OnMoneyChanged?.Invoke(money);
            return true;
        }
        return false;
    }
}
