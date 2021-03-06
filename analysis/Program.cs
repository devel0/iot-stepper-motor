﻿using SearchAThing;
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
using Exts;
using Avalonia.Data;

namespace Exts
{

    public static class Ext
    {

        public static Entity VeryExpensiveSimplify(this Entity expr, int level)
        {
            Entity ExpensiveSimplify(Entity expr)
            {
                return expr.Replace(x => x.Simplify());
            }

            if (level <= 0)
                return expr;
            return VeryExpensiveSimplify(ExpensiveSimplify(expr), level - 1);
        }

    }

}

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

        public class GraphInput
        {
            /// <summary>
            /// if not null append to here
            /// </summary>
            public PlotView prevPlotView;
            public double t0 = 0;
            public double duration;// = 2d;
            public double initialSpeed;// = 10d;
            public double initialPos;// = 5d;
            public double speedVariation;// = 5d;            
        }

        public class GraphOutput
        {
            public GraphInput input;
            public PlotView plotView;
            public double finalPosVal;
            public double finalSpeedVal;
            public double finalAccelVal;
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
                gr.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));
                var sp = new WrapPanel() { HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center };
                gr.Children.Add(sp);

                var spSide = new StackPanel() { Orientation = Avalonia.Layout.Orientation.Horizontal, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
                Grid.SetRow(spSide, 2);
                gr.Children.Add(spSide);
                spSide.Children.Add(new TextBlock() { Text = "Show / hide:", FontWeight = Avalonia.Media.FontWeight.Bold });
                var cbAccelToggle = new CheckBox() { Content = "accel", Margin = new Thickness(10, 0, 0, 0) };
                spSide.Children.Add(cbAccelToggle);
                var cbSpeedToggle = new CheckBox() { Content = "speed", Margin = new Thickness(10, 0, 0, 0) };
                spSide.Children.Add(cbSpeedToggle);
                var cbPosToggle = new CheckBox() { Content = "pos", Margin = new Thickness(10, 0, 0, 0) };
                spSide.Children.Add(cbPosToggle);

                var cbFormulaToggle = new CheckBox() { Content = "formulas", Margin = new Thickness(10, 0, 0, 0) };
                spSide.Children.Add(cbFormulaToggle);
                cbFormulaToggle.Bind(CheckBox.IsCheckedProperty, new Binding("IsVisible", BindingMode.TwoWay)
                { Source = sp });

                var grRoot = new Grid() { Margin = new Thickness(10) };
                grRoot.Children.Add(gr);
                this.Content = grRoot;

                Func<Entity, Entity> mySimplify = (e) =>
                {
                    var q1 = e.Simplify().ToString();
                    var q2 = e.VeryExpensiveSimplify(2).ToString();

                    if (q2.Length < q1.Length)
                        return q2;
                    else
                        return q1;
                };

                Func<AngouriMath.Entity, string> toLatex = (e) =>
                {
                    var res = mySimplify(e).Latexise().Replace("\\times", "\\cdot");

                    return res;
                };

                var eqMargin = new Thickness(0, 0, 75, 25);
                var eqFontSize = 20f;

                // t_r = t - t_0
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = debugFormula("t_r=t-t_0"),
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                // s_d = s(d)
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = @"s_d=s(d)",
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                Func<string, string> debugFormula = (f) =>
                {
                    System.Diagnostics.Debug.WriteLine(f);
                    return f;
                };                

                var accelBase = "1-cos(t_r)".Substitute("t_r", "(t_r/d*(2*pi))");
                var accelCoeff = $"s_d / ({accelBase.Integrate("t_r").Substitute("t_r", "d")})";
                var accel = $"({accelCoeff}) * ({accelBase})";
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = debugFormula($"a(t_r)={toLatex(accel)}"),
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                // a_max = a(d/2)
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = debugFormula(@"a_{max}=a\left(\frac{d}{2}\right)"),
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                var aMax = $"a_max={accel}".Substitute("t_r", "d/2").Solve("d");
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = debugFormula(toLatex($"d={aMax}")),
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                var speed = $"s_0+{accel.Integrate("t_r")}";
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = debugFormula($"s(t_r)={toLatex(speed)}"),
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                var pos = $"x_0+{speed.Integrate("t_r") - speed.Integrate("t_r").Substitute("t_r", 0)}";
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = debugFormula($"x(t_r)={toLatex(pos)}"),
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                var speedPos = $"x_d={pos}".Substitute("t_r", "d").Solve("s_d");
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = debugFormula($"s(d)={toLatex(speedPos)}"),
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                var finalPos = $"{pos.Substitute("t_r", "d")}";
                sp.Children.Add(new CSharpMath.Avalonia.MathView()
                {
                    LaTeX = debugFormula($"x(d)={toLatex(finalPos)}"),
                    Margin = eqMargin,
                    FontSize = eqFontSize
                });

                GraphOutput BuildGraph(string tag, GraphInput input, int colorIdxBase, double timeTol, double timeSlices)
                {
                    var pv = input.prevPlotView != null ? input.prevPlotView : new PlotView();
                    var res = new GraphOutput() { plotView = pv, input = input };
                    if (input.prevPlotView == null)
                        pv.Model = new PlotModel();

                    var accelC = accel
                        .Substitute("d", input.duration)
                        .Substitute("s_d", input.speedVariation)
                        .Compile("t_r");
                    var accelS = new OxyPlot.Series.LineSeries()
                    {
                        Title = $"a{tag}",
                        DataFieldX = "x",
                        DataFieldY = "y",
                        ItemsSource = SciToolkit.Range(
                            tol: timeTol,
                            start: input.t0,
                            end: input.t0 + input.duration,
                            inc: input.duration / timeSlices,
                            includeEnd: true).Select(x => new PlotData(x, accelC.Call(x - input.t0).Real)),
                        Color = OxyColor.Parse(colors[(0 + colorIdxBase) % colors.Length])
                    };
                    cbAccelToggle.Bind(CheckBox.IsCheckedProperty, new Binding("IsVisible", BindingMode.TwoWay) { Source = accelS });
                    cbAccelToggle.PropertyChanged += (a, b) => { if (b.Property.Name == "IsChecked") pv.InvalidatePlot(); };
                    pv.Model.Series.Add(accelS);

                    var speedC = speed
                        .Substitute("d", input.duration)
                        .Substitute("s_d", input.speedVariation)
                        .Substitute("s_0", input.initialSpeed)
                        .Compile("t_r");
                    var speedS = new OxyPlot.Series.LineSeries()
                    {
                        Title = $"s{tag}",
                        DataFieldX = "x",
                        DataFieldY = "y",
                        ItemsSource = SciToolkit.Range(
                            tol: timeTol,
                            start: input.t0,
                            end: input.t0 + input.duration,
                            inc: input.duration / timeSlices,
                            includeEnd: true).WithIndexIsLast().Select(q =>
                            {
                                var x = q.item;
                                var y = speedC.Call(q.item - input.t0).Real;
                                var pd = new PlotData(x, y);

                                if (q.isLast) res.finalSpeedVal = y;

                                return pd;
                            }),
                        Color = OxyColor.Parse(colors[(2 + colorIdxBase) % colors.Length])
                    };
                    cbSpeedToggle.Bind(CheckBox.IsCheckedProperty, new Binding("IsVisible", BindingMode.TwoWay) { Source = speedS });
                    cbSpeedToggle.PropertyChanged += (a, b) => { if (b.Property.Name == "IsChecked") pv.InvalidatePlot(); };
                    pv.Model.Series.Add(speedS);

                    var posC = pos
                        .Substitute("d", input.duration)
                        .Substitute("s_d", input.speedVariation)
                        .Substitute("s_0", input.initialSpeed)
                        .Substitute("x_0", input.initialPos)
                        .Compile("t_r");
                    var posS = new OxyPlot.Series.LineSeries()
                    {
                        Title = $"x{tag}",
                        DataFieldX = "x",
                        DataFieldY = "y",
                        ItemsSource = SciToolkit.Range(
                            tol: timeTol,
                            start: input.t0,
                            end: input.t0 + input.duration,
                            inc: input.duration / timeSlices,
                            includeEnd: true).WithIndexIsLast().Select(q =>
                            {
                                var x = q.item;
                                var y = posC.Call(q.item - input.t0).Real;
                                var pd = new PlotData(x, y);

                                if (q.isLast) res.finalPosVal = y;

                                return pd;
                            }),
                        Color = OxyColor.Parse(colors[(4 + colorIdxBase) % colors.Length])
                    };
                    cbPosToggle.Bind(CheckBox.IsCheckedProperty, new Binding("IsVisible", BindingMode.TwoWay) { Source = posS });
                    cbPosToggle.PropertyChanged += (a, b) => { if (b.Property.Name == "IsChecked") pv.InvalidatePlot(); };
                    pv.Model.Series.Add(posS);

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

                    return res;
                };

                var motion1_d = TimeSpan.FromMilliseconds(5000);
                var motion1_rev_sec = 1d;

                var motion2_d = TimeSpan.FromMilliseconds(2150);
                var motion2_rev_sec = 0d;

                var pulse_rev = 400d;
                var pulse_width = TimeSpan.FromMilliseconds(5e-3);

                // time: [us]
                // speed: [pulse/us]
                // var time_s_coeff = 1e6;
                // var speed_rev_sec_coeff = pulse_rev * 1e-6;

                // time: [s]
                // speed: [rev/s]
                var time_s_coeff = 1d;
                var speed_rev_sec_coeff = 1d;

                var timeTol = 1e-3;
                var timeSlices = 100;

                var pv1 = BuildGraph("1", new GraphInput
                {
                    duration = motion1_d.TotalSeconds * time_s_coeff,
                    initialSpeed = 0d,
                    initialPos = 0d,
                    speedVariation = motion1_rev_sec * speed_rev_sec_coeff
                }, colorIdxBase: 0, timeTol: timeTol, timeSlices: timeSlices);

                var pv2 = BuildGraph("2", new GraphInput
                {
                    prevPlotView = pv1.plotView,
                    t0 = pv1.input.t0 + pv1.input.duration,
                    duration = motion2_d.TotalSeconds * time_s_coeff,
                    initialSpeed = pv1.finalSpeedVal,
                    initialPos = pv1.finalPosVal,
                    speedVariation = motion2_rev_sec * speed_rev_sec_coeff - pv1.finalSpeedVal
                }, colorIdxBase: 1, timeTol: timeTol, timeSlices: timeSlices);

                Grid.SetRow(pv1.plotView, 1);
                gr.Children.Add(pv1.plotView);
            }
        }

        static void Main(string[] args)
        {
            GuiToolkit.CreateGui<MainWindow>();
        }
    }
}