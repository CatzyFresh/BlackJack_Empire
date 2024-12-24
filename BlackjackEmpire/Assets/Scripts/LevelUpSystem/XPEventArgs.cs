using System;

public class XPEventArgs : EventArgs
{
    public int Amount { get; }
    public string Source { get; }

    public XPEventArgs(int amount, string source)
    {
        Amount = amount;
        Source = source;
    }
}
