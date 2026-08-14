using System.Collections.Generic;
using System;

[Serializable]
public class AuthTokenData
{
    public string cookie;
    public string socketURL;
    public string nameSpace;
}

[Serializable]
public class MessageData
{
    public string type;
    public Data payload = new();
}

[Serializable]
public class Data
{
    public int betIndex;
}

// InIt Data Classes

[Serializable]
public class Root
{
    public string id { get; set; }
    public GameData gameData { get; set; }
    public Features features { get; set; }
    public UiData uiData { get; set; }
    public Player player { get; set; }

    //Result
    public bool success { get; set; }
    public Payload payload { get; set; }
}

[Serializable]
public class GameData
{
    public List<List<int>> lines { get; set; }
    public List<double> bets { get; set; }
    public int totalLines { get; set; }
}

[Serializable]
public class Player
{
    public double balance { get; set; }
}

[Serializable]
public class BalanceSyncPayload
{
    public double balance;
}

[Serializable]
public class UiData
{
    public Paylines paylines { get; set; }
}

[Serializable]
public class Paylines
{
    public List<Symbol> symbols { get; set; }
}

[Serializable]
public class Symbol
{
    public int id { get; set; }
    public string name { get; set; }
    public double payout { get; set; }
    public string description { get; set; }
    public string group { get; set; }
}

[Serializable]
public class Features
{
    public AnyPayouts anyPayouts { get; set; }
    public CashSpinnerBonus cashSpinnerBonus { get; set; }
    public DoubleCashSpinnerBonus doubleCashSpinnerBonus { get; set; }
}

[Serializable]
public class AnyPayouts
{
    public double bars { get; set; }
    public double sevens { get; set; }
    public List<int> blockSymbols { get; set; }
    public bool requireAllPositionsMatch { get; set; }
}

[Serializable]
public class CashSpinnerBonus
{
    public bool enabled { get; set; }
    public List<object> wheelSlices { get; set; }
    public int requiredCount { get; set; }
    public string levelUpTrigger { get; set; }
    public int triggerSymbolId { get; set; }
    public List<double> wheelSlicesProb { get; set; }

    //Result
    public bool triggered { get; set; }
    public double award { get; set; }
    public int wheelStopIndex { get; set; }
    public bool levelUp { get; set; }
    public List<List<int>> positions { get; set; }
}

[Serializable]
public class DoubleCashSpinnerBonus
{
    public bool enabled { get; set; }
    public List<int> wheelSlices { get; set; }
    public ShortcutTrigger shortcutTrigger { get; set; }
    public List<double> wheelSlicesProb { get; set; }
    public int globalMultiplier { get; set; }

    //Result
    public bool triggered { get; set; }
    public double award { get; set; }
    public int wheelStopIndex { get; set; }
    public List<List<int>> positions { get; set; }
}

[Serializable]
public class ShortcutTrigger
{
    public string description { get; set; }
    public int simultaneousPaylinesRequired { get; set; }
}

// Result Data

[Serializable]
public class Payload
{
    public List<List<string>> reels { get; set; }
    public List<WinningLine> winningLines { get; set; }
    public double totalWin { get; set; }
    public Features features { get; set; }
}

[Serializable]
public class CashSpinner
{
    public bool triggered { get; set; }
    public double award { get; set; }
    public int wheelStopIndex { get; set; }
}

[Serializable]
public class WinningLine
{
    public int lineIndex { get; set; }
    public double payout { get; set; }
    public List<List<int>> positions { get; set; }
}

