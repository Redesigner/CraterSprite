using System.Collections.Generic;
using Godot;

namespace CraterSprite.Input;

using InputVariant = Variant<Key, JoyAxis, JoyButton>;

public class InputAction
{
    private string name;

    public List<InputAxisMappingType> mappedButtons;
}