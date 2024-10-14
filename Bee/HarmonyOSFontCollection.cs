using System;
using Avalonia.Media.Fonts;

namespace Bee;

public sealed class HarmonyOSFontCollection : EmbeddedFontCollection
{
    public HarmonyOSFontCollection() : base(
        new Uri("fonts:HarmonyOS Sans SC", UriKind.Absolute),
        new Uri("avares://Bee/Assets/Fonts", UriKind.Absolute))
    {
    }
}
