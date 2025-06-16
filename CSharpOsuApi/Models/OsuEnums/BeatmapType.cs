using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace CSharpOsuApi.Models.OsuEnums;

public enum BeatmapType
{
    [Description("favourite")]
    Favourite,
    [Description("favourite")]
    Favorite,
    [Description("graveyard")]
    Graveyard,
    [Description("guest")]
    Guest,
    [Description("loved")]
    Loved,
    [Description("nominated")]
    Nominated,
    [Description("pending")]
    Pending,
    [Description("ranked")]
    Ranked
}

public static class BeatmapTypeExtensions
{
    public static string Description(this BeatmapType type)
    {
        return ((DescriptionAttribute)type.GetType().GetField(type.ToString())!
            .GetCustomAttribute(typeof(DescriptionAttribute), false)!).Description;
    }
}