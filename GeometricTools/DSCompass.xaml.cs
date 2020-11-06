using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeometricTools
{
  /// <summary>
  /// Interaction logic for DSCompass.xaml
  /// </summary>
  public partial class DSCompass : UserControl
  {
    public DSCompass()
    {
      InitializeComponent();
      Loaded += DSCompass_Loaded;
      RenderTransformOrigin = new Point(.5, .5);
    }

    private void DSCompass_Loaded(object sender, RoutedEventArgs e)
    {
      MinWidth = 200;
      MaxWidth = 700;
    }

    int LogID = 0;
    public void Log(string s)
    {
      Console.WriteLine(LogID + " -> " + s);
      LogID++;
    }

    private void thumbPernaDireita_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
    {
      try
      {
        if ((this.Width + e.HorizontalChange) > MinWidth)
        {
          this.Width += (e.HorizontalChange / 2);
        }
      }
      catch (Exception ex) { Log(ex.Message); }
    }

    private void thumbPernaDireita_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
    {

    }

    private void thumbCentro_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
    {
      try
      {
        this.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) + e.HorizontalChange);
        this.SetValue(Canvas.TopProperty, (double)this.GetValue(Canvas.TopProperty) + e.VerticalChange);
      }
      catch (Exception ex) { Log(ex.Message); }
    }

    Point center;
    RotateTransform rotation;
    private void thumbCentro_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
    {
      center = new Point((double)this.GetValue(Canvas.LeftProperty) + this.Width / 2, (double)this.GetValue(Canvas.TopProperty) + this.Height / 2);
    }


    private void thumbPontaPencil_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
    {
      Point pos = Mouse.GetPosition(this);

      double deltaX = pos.X - center.X;
      double deltaY = pos.Y - center.Y;

      if (deltaY.Equals(0))
      {

        return;
      }

      double tan = deltaX / deltaY;
      double angle = Math.Atan(tan);

      // Convert to degrees.
      angle = angle * 180 / Math.PI;

      // If the mouse crosses the vertical center,
      // find the complementary angle.
      if (deltaY > 0)
      {
        angle = 180 - Math.Abs(angle);
      }

      // Rotate left if the mouse moves left and right
      // if the mouse moves right.
      if (deltaX < 0)
      {
        angle = -Math.Abs(angle);
      }
      else
      {
        angle = Math.Abs(angle);
      }

      if (Double.IsNaN(angle))
      {
        return;
      }

      // Apply the rotation to the strokes' outline.
      rotation = new RotateTransform(angle);
      RenderTransform = rotation;
    }

    private void thumbPontaPencil_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
    {

    }
  }

  public static class Math2DHelper
  {
    public static Vector RotateVector2d(double x0, double y0, double rad)
    {
      Vector result = new Vector();
      result.X = x0 * Math.Cos(rad) - y0 * Math.Sin(rad);
      result.Y = x0 * Math.Sin(rad) + y0 * Math.Cos(rad);
      return result;
    }

    public static double D2R(double degree)
    {
      return (degree % 360) * Math.PI / 180;
    }
  }

}

