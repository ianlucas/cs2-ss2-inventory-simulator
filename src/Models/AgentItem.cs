/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Text.Json.Serialization;

namespace InventorySimulator;

public class AgentItem
{
    [JsonPropertyName("def")]
    public ushort? Def { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; } = "";

    [JsonPropertyName("patches")]
    public List<uint> Patches { get; set; } = [];

    [JsonPropertyName("vofallback")]
    public bool VoFallback { get; set; } = false;

    [JsonPropertyName("vofemale")]
    public bool VoFemale { get; set; } = false;

    [JsonPropertyName("voprefix")]
    public string VoPrefix { get; set; } = "";

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not AgentItem other)
            return false;

        return Def == other.Def
            && Model == other.Model
            && Patches.SequenceEqual(other.Patches)
            && VoFallback == other.VoFallback
            && VoFemale == other.VoFemale
            && VoPrefix == other.VoPrefix;
    }

    public static bool operator ==(AgentItem? left, AgentItem? right)
    {
        if (ReferenceEquals(left, right))
            return true;
        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(AgentItem? left, AgentItem? right)
    {
        return !(left == right);
    }
}
