using System;
using Godot;

namespace CraterSprite.Input;

using InputVariant = Variant<Key, JoyAxis, JoyButton>;

public enum InputAxisMappingType : byte
{
    None,
    Positive,
    Negative,
    Range
}

public struct InputAxisMapping
{
    public InputVariant button;

    public InputAxisMappingType mappingType;
}