using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace 短语列表约束
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SpeechRecognizer _recognizer;
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;
            this.Unloaded += Page_UnLoaded;
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                while (true)
                {
                    SpeechRecognitionResult result = await _recognizer.RecognizeAsync();
                    if (result.Status == SpeechRecognitionResultStatus.Success)
                    {
                        //处理识别结果
                        listBox.SelectedItem = result.Text;
                        if (result.Text == "结束")
                        {
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void Page_Loaded(object sender,object e)
        {
            _recognizer = new SpeechRecognizer();
            //创建自定义短语约束
            string[] arrgy = { "足球", "排球", "跑步", "羽毛球", "橄榄球","结束" };
            SpeechRecognitionListConstraint listConstraint = new SpeechRecognitionListConstraint(arrgy);
            //添加约束实例到集合中
            _recognizer.Constraints.Add(listConstraint);
            //编译约束
            await _recognizer.CompileConstraintsAsync();
        }

        private void Page_UnLoaded(object sender, RoutedEventArgs e)
        {
            //释放资源
            _recognizer.Dispose();
        }
    }
}
