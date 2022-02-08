using System;

public interface ISelectable
{
    event EventHandler<UnitSelectionEvent> OnSelect;
    event EventHandler<UnitSelectionEvent> OnDeselect;
}