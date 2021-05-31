using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace 自定义语音识别规则
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void tbn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            btn.IsEnabled = false;
            using (SpeechRecognizer recognizer = new SpeechRecognizer())
            {
                try
                {
                    //加载语法文件
                    var sgrsFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///srgs.xml"));
                    //创建识别约束
                    SpeechRecognitionGrammarFileConstraint grammarFileConstraint = new SpeechRecognitionGrammarFileConstraint(sgrsFile);
                    //加入到约束集合中
                    recognizer.Constraints.Add(grammarFileConstraint);
                    //编译约束
                    SpeechRecognitionCompilationResult compilationResult = await recognizer.CompileConstraintsAsync();
                    if (compilationResult.Status == SpeechRecognitionResultStatus.Success)
                    {
                        //开始识别
                        SpeechRecognitionResult result = await recognizer.RecognizeAsync();
                        if (result.Status == SpeechRecognitionResultStatus.Success)
                        {
                            tbRes.Text = $"识别结果：{result.Text}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            btn.IsEnabled = true;
        }
    }
}
