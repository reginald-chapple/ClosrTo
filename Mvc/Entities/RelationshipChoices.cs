using System.ComponentModel;

namespace Mvc.Entities;

public enum RelationshipChoices
{
    [Description("God")]
    God,
    [Description("Neighbor")]
    Neighbor,
    [Description("Family")]
    Family,
    [Description("Friends")]
    Friends
}