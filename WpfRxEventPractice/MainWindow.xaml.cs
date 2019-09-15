using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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

namespace WpfRxEventPractice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            var mouseDown = Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                    h => (s, e) => h(e),
                    h => this.MouseDown += h,
                    h => this.MouseDown -= h
                );

            var mouseMove = Observable.FromEvent<MouseEventHandler, MouseEventArgs>(
                    h => (s, e) => h(e),
                    h => this.MouseMove += h,
                    h => this.MouseMove -= h
                );

            var mouseUp = Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                    h => (s, e) => h(e),
                    h => this.MouseUp += h,
                    h => this.MouseUp -= h
                );

            var drag = mouseMove
                //マウスムーブをマウスダウンまでスキップ。
                //マウスダウン時にマウスをキャプチャ
                .SkipUntil(mouseDown.Do(_ => this.CaptureMouse()))
                //マウスアップが行われるまでTake。
                //マウスアップでマウスのキャプチャをリリース
                .TakeUntil(mouseUp.Do(_ => this.ReleaseMouseCapture()))
                //ドラッグが終了したタイミングでCompletedを表示
                .Finally(() => textBlock.Text = "Completed")
                .Repeat();

            drag.Select(e => e.GetPosition(null))
                .Select(p => $"x: {p.X}, y: {p.Y}")
                .Subscribe(s => textBlock.Text = s);

        }
    }
}
