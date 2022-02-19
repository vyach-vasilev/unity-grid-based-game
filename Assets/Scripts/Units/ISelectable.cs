using System;

public interface ISelectable
{
    bool Selected { get; set; }
    event EventHandler<UnitSelectionEvent> OnSelect;
    event EventHandler<UnitSelectionEvent> OnDeselect;
}