using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Dotnet9WPFControls.Controls
{
    [TemplatePart(Name = PartBorrderBackground, Type = typeof(Border))]
    [TemplatePart(Name = PartCanvasHint, Type = typeof(Canvas))]
    public class GuideBase : Control
    {
        private const string PartBorrderBackground = "PART_Border_Background";
        private const string PartCanvasHint = "PART_Canvas_Hint";
    }
}
