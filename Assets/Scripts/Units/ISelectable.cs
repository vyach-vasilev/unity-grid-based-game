using System;

public interface ISelectable
{
    event EventHandler<UnitSelectEvent> OnSelect;
    event EventHandler<UnitSelectEvent> OnDeselect;
}