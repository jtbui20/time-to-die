using System;

public interface IUnitDefinition
{
    public event Action OnRebuild;
    public int Health { get; }
    public string Description { get; }

}