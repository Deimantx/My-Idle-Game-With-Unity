using System.Collections.Generic;
using UnityEngine;

namespace IdleGame.UI.Tooltips
{
    public enum TooltipValueTone
    {
        Normal,
        Positive,
        Negative,
        Muted
    }

    public readonly struct TooltipStatLine
    {
        public TooltipStatLine(string label, string value, TooltipValueTone tone = TooltipValueTone.Normal)
        {
            Label = label ?? string.Empty;
            Value = value ?? string.Empty;
            Tone = tone;
        }

        public string Label { get; }
        public string Value { get; }
        public TooltipValueTone Tone { get; }
        public bool IsValid => !string.IsNullOrWhiteSpace(Label) && !string.IsNullOrWhiteSpace(Value);
    }

    public sealed class TooltipData
    {
        public string SourceId = string.Empty;
        public string DisplayName = string.Empty;
        public string Category = string.Empty;
        public string Description = string.Empty;
        public Sprite Icon;
        public readonly List<TooltipStatLine> PrimaryStats = new();
        public readonly List<TooltipStatLine> SecondaryStats = new();
        public readonly List<TooltipStatLine> Effects = new();
        public readonly List<TooltipStatLine> Requirements = new();
        public readonly List<TooltipStatLine> Comparison = new();
        public readonly List<string> Footer = new();

        public bool HasUsefulContent =>
            !string.IsNullOrWhiteSpace(DisplayName) ||
            !string.IsNullOrWhiteSpace(Description) ||
            PrimaryStats.Count > 0 ||
            SecondaryStats.Count > 0 ||
            Effects.Count > 0 ||
            Requirements.Count > 0 ||
            Comparison.Count > 0 ||
            Footer.Count > 0;
    }
}
