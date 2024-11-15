using System;

public interface IDeathEvent
{
    event Action<IDeathEvent> Dead;
}