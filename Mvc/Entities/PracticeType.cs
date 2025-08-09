using System.ComponentModel;

namespace Mvc.Entities;

public enum PracticeType
{
    [Description("Universal")]
    Universal,
    [Description("Diagnostic")]
    Diagnostic,
    [Description("Formative")]
    Formative
}