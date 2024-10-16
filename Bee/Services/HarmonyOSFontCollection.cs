using System;
using Avalonia.Media.Fonts;

namespace Bee.Services;

public sealed class HarmonyOSFontCollection : EmbeddedFontCollection
{
    public HarmonyOSFontCollection() : base(
        new Uri("fonts:HarmonyOS Sans", UriKind.Absolute),
        new Uri("avares://Bee/Assets/Fonts", UriKind.Absolute))
    {
    }
}
