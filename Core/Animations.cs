using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace OlibKey.Core
{
    public static class Animations
    {
        public async static Task ClosingWindowAnimation(Window w, ScaleTransform transform)
        {
            DoubleAnimation anim = new DoubleAnimation { Duration = TimeSpan.FromSeconds(0.2), From = 1, To = 0, AccelerationRatio = 1};
            DoubleAnimation anim1 = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.2),
                AccelerationRatio = 1,
                From = 1,
                To = 0.8,
            };
            Timeline.SetDesiredFrameRate(anim, 60);
            Timeline.SetDesiredFrameRate(anim1, 60);
            w.BeginAnimation(UIElement.OpacityProperty, anim);
            transform.BeginAnimation(ScaleTransform.ScaleXProperty, anim1);
            transform.BeginAnimation(ScaleTransform.ScaleYProperty, anim1);

            await Task.Delay(TimeSpan.FromSeconds(0.2));
        }
    }
}
