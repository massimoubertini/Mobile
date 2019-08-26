using System;
using System.Linq;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName("XamarinDocs")]
[assembly: ExportEffect(typeof(TouchTracking.UWP.TouchEffect), "TouchEffect")]

namespace TouchTracking.UWP
{
    public class TouchEffect : PlatformEffect
    {
        private FrameworkElement frameworkElement;
        private TouchTracking.TouchEffect effect;
        private Action<Element, TouchActionEventArgs> onTouchAction;

        protected override void OnAttached()
        {
            // ottiene Windows FrameworkElement corrispondente all'elemento che l'effetto è collegato a
            frameworkElement = Control == null ? Container : Control;
            // ottiene l'accesso alla classe TouchEffect nella libreria .NET Standard
            effect = (TouchTracking.TouchEffect)Element.Effects.FirstOrDefault(e => e is TouchTracking.TouchEffect);
            if (effect != null && frameworkElement != null)
            {
                // salva il metodo per chiamare gli eventi di tocco
                onTouchAction = effect.OnTouchAction;
                // imposta i gestori eventi in FrameworkElement
                frameworkElement.PointerEntered += OnPointerEntered;
                frameworkElement.PointerPressed += OnPointerPressed;
                frameworkElement.PointerMoved += OnPointerMoved;
                frameworkElement.PointerReleased += OnPointerReleased;
                frameworkElement.PointerExited += OnPointerExited;
                frameworkElement.PointerCanceled += OnPointerCancelled;
            }
        }

        protected override void OnDetached()
        {
            if (onTouchAction != null)
            {
                // rilascia i gestori eventi in FrameworkElement
                frameworkElement.PointerEntered -= OnPointerEntered;
                frameworkElement.PointerPressed -= OnPointerPressed;
                frameworkElement.PointerMoved -= OnPointerMoved;
                frameworkElement.PointerReleased -= OnPointerReleased;
                frameworkElement.PointerExited -= OnPointerEntered;
                frameworkElement.PointerCanceled -= OnPointerCancelled;
            }
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, TouchActionType.Entered, args);
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, TouchActionType.Pressed, args);
            // controlla l'impostazione della proprietà Capture
            if (effect.Capture)
                (sender as FrameworkElement).CapturePointer(args.Pointer);
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, TouchActionType.Moved, args);
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, TouchActionType.Released, args);
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, TouchActionType.Exited, args);
        }

        private void OnPointerCancelled(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, TouchActionType.Cancelled, args);
        }

        private void CommonHandler(object sender, TouchActionType touchActionType, PointerRoutedEventArgs args)
        {
            PointerPoint pointerPoint = args.GetCurrentPoint(sender as UIElement);
            Windows.Foundation.Point windowsPoint = pointerPoint.Position;
            onTouchAction(Element, new TouchActionEventArgs(args.Pointer.PointerId, touchActionType, new Point(windowsPoint.X, windowsPoint.Y), args.Pointer.IsInContact));
        }
    }
}