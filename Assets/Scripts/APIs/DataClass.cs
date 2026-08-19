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
    public UiData uiData { get; set; }
    public Player player { get; set; }

    // result Data Classes
    public bool success { get; set; }
    public List<List<string>> matrix { get; set; }
    public Payload payload { get; set; }
    public Features features { get; set; }
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
    public List<double> multiplier { get; set; }
    public string description { get; set; }
}

// Result Data Classes

[Serializable]
public class Payload
{
    public double winAmount { get; set; }
    public List<LineWin> lineWins { get; set; }
    public double heatEmUpWin { get; set; }
    public object superWheelBonus { get; set; }
    public int activeLines { get; set; }
    public int freeSpinsRemaining { get; set; }
    public bool isFreeSpinActive { get; set; }
    public bool isFreeSpinsTriggered { get; set; }
    public bool isWheelTriggered { get; set; }
}

[Serializable]
public class LineWin
{
    public int lineIndex { get; set; }
    public List<Position> positions { get; set; }
    public string symbolId { get; set; }
    public string symbolName { get; set; }
    public double payout { get; set; }
    public int matchCount { get; set; }
}

[Serializable]
public class Position
{
    public List<int> position { get; set; }
}

[Serializable]
public class Features
{
}