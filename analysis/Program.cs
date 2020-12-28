using SearchAThing;
using System.Linq;
using SearchAThing.Gui;
using Avalonia.Controls;
using Avalonia;
using OxyPlot;
using OxyPlot.Avalonia;
using static AngouriMath.Extensions.AngouriMathExtensions;
using System.Collections.Generic;
using System;
using AngouriMath;

namespace analysis
{
    class Program
    {
        public class PlotData
        {
            public PlotData(double x, double y) { this.x = x; this.y = y; }
            public double x { get; set; }
            public double y { get; set; }
        }

        public class MainWindow : Win
        {
            public MainWindow() : base(new[]
            {
                "resm:OxyPlot.Avalonia.Themes.Default.xaml?assembly=OxyPlot.Avalonia"
            })
            {
                // Width = 600;
                // Height = 700;

                var colors = new[] { "#9ccc65", "#6b9b37", "#42a5f5", "#0077c2", "#ef5350", "#b61827" };

                var gr = new Grid();
                gr.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));
                gr.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
                var sp = new WrapPanel() { HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center };
                gr.Children.Add(sp);

                var grRoot = new Grid() { Margin = new Thickness(10) };
                grRoot.Children.Add(gr);
                this.Content = grRoot;

                Func<Entity, Entity> mySimplify = (e) =>
                {
                    return e
                        .Substitute("sin(d/d*2*pi)", "0")
                        .Substitute("-0/((1/d)*2*pi)", "0")                        
                        .Simplify()
                        .ToString()
                        .Replace("-1/4 * s / pi ^ 2 + 1/4 * s / pi ^ 2", "0");
                };

                Func<AngouriMath.Entity, string> toLatex = (e) =>
                {
                    var res = mySimplify(e.Simplify())
                        .Simplify()
                        .Latexise().Replace("\\times", "\\cdot");

                    return res;
                };
                var eqMargin = new Thickness(0, 0, 100, 15);
                var eqFontSize = 20f;

                var duration = 2d;
                var initialSpeed = 10d;
                var initialPos = 5d;
                var speedVal = 5d;

                var accelBase = "1-cos(t)".Substitute("t", "(t/d*(2*pi))");
                //var accelCoeff = $"s / abs({accelBase.Integrate("t").Substitute("t", "d")})";
                var accelCoeff = $"s / ({accelBase.Integrate("t").Substitute("t", "d")})";
                var accel = $"({accelCoeff}) * ({accelBase})";
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = $"accel(t)={toLatex(accel)}",
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                var accelGiven = $"a=({mySimplify(accel)})".Substitute("t", "d/2").Solve("d");
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = $"d(a)={toLatex(accelGiven)}",
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                var speed = $"s_0+{accel.Integrate("t")}";
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = $"speed(t)={toLatex(speed)}",
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                var pos = $"p_0+{speed.Integrate("t") - speed.Integrate("t").Substitute("t", 0)}";
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = $"pos(t)={toLatex(pos)}",
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                var finalPos = $"{pos.Substitute("t", "d")}";
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = $"pos(d)={toLatex(finalPos)}",
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                Func<PlotView> BuildGraph = () =>
                {
                    var pv = new PlotView();
                    pv.Model = new PlotModel();

                    var accelC = accel.Substitute("d", duration).Substitute("s", speedVal).Compile("t");
                    var accelS = new OxyPlot.Series.LineSeries()
                    {
                        Title = "accel",
                        DataFieldX = "x",
                        DataFieldY = "y",
                        ItemsSource = SciToolkit.Range(
                            tol: 1e-3,
                            start: 0,
                            end: duration,
                            inc: duration / 100,
                            includeEnd: true).Select(x => new PlotData(x, accelC.Call(x).Real)),
                        Color = OxyColor.Parse(colors[0])
                    };
                    pv.Model.Series.Add(accelS);

                    var speedC = speed.Substitute("d", duration).Substitute("s", speedVal).Substitute("s_0", initialSpeed).Compile("t");
                    var speedS = new OxyPlot.Series.LineSeries()
                    {
                        Title = "speed",
                        DataFieldX = "x",
                        DataFieldY = "y",
                        ItemsSource = SciToolkit.Range(
                           tol: 1e-3,
                           start: 0,
                           end: duration,
                           inc: duration / 100,
                           includeEnd: true).Select(x => new PlotData(x, speedC.Call(x).Real)),
                        Color = OxyColor.Parse(colors[2])
                    };
                    pv.Model.Series.Add(speedS);
                    pv.Model.Annotations.Add(new OxyPlot.Annotations.TextAnnotation()
                    {
                        TextPosition = new DataPoint(0, initialSpeed),
                        Text = $"s0={initialSpeed}",
                        TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Right,
                        TextVerticalAlignment = OxyPlot.VerticalAlignment.Bottom,
                        StrokeThickness = 0
                    });
                    pv.Model.Annotations.Add(new OxyPlot.Annotations.TextAnnotation()
                    {
                        TextPosition = new DataPoint(duration, initialSpeed + speedVal),
                        Text = $"s=s0+{speedVal}",
                        TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Right,
                        TextVerticalAlignment = OxyPlot.VerticalAlignment.Bottom,
                        StrokeThickness = 0
                    });

                    var posC = pos.Substitute("d", duration).Substitute("s", speedVal).Substitute("s_0", initialSpeed).Substitute("p_0", initialPos).Compile("t");
                    var posS = new OxyPlot.Series.LineSeries()
                    {
                        Title = "pos",
                        DataFieldX = "x",
                        DataFieldY = "y",
                        ItemsSource = SciToolkit.Range(
                            tol: 1e-3,
                            start: 0,
                            end: duration,
                            inc: duration / 100,
                            includeEnd: true).Select(x => new PlotData(x, posC.Call(x).Real)),
                        Color = OxyColor.Parse(colors[4])
                    };
                    pv.Model.Series.Add(posS);
                    pv.Model.Annotations.Add(new OxyPlot.Annotations.TextAnnotation()
                    {
                        TextPosition = new DataPoint(0, initialPos),
                        Text = $"p0={initialPos}",
                        TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Right,
                        TextVerticalAlignment = OxyPlot.VerticalAlignment.Bottom,
                        StrokeThickness = 0
                    });

                    pv.Model.Annotations.Add(new OxyPlot.Annotations.TextAnnotation()
                    {
                        TextPosition = new DataPoint(duration, 0),
                        Text = $"d={duration}",
                        TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Center,
                        TextVerticalAlignment = OxyPlot.VerticalAlignment.Top,
                        StrokeThickness = 0
                    });

                    pv.ResetAllAxes();
                    foreach (var x in pv.Model.Axes)
                    {
                        x.MajorGridlineStyle = LineStyle.Dot;
                    }
                    foreach (var ax in pv.Model.Axes)
                    {
                        ax.MinimumPadding = 0.1;
                        ax.MaximumPadding = 0.1;
                    }
                    pv.InvalidatePlot();

                    return pv;
                };

                var tabs = new List<TabItem>();

                PlotView pv1 = BuildGraph();
                var ti1 = new TabItem() { Header = "accel", Content = pv1 };
                tabs.Add(ti1);

                speedVal = -20;
                PlotView pv2 = BuildGraph();
                var ti2 = new TabItem() { Header = "decel", Content = pv2 };
                tabs.Add(ti2);

                var tc = new TabControl();
                tc.Items = tabs;
                Grid.SetRow(tc, 1);
                gr.Children.Add(tc);
            }
        }

        static void Main(string[] args)
        {
            GuiToolkit.CreateGui<MainWindow>();
        }
    }
}