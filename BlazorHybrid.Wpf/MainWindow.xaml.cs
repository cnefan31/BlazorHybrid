﻿using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Web.WebView2.Core;
using System.IO;
using System.Windows;
#nullable disable 

namespace BlazorHybrid.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    protected string UploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "uploads");

    public MainWindow()
    {

        Resources.Add("services", Startup.Services);
        InitializeComponent();

        dockTop.Visibility = Visibility.Hidden;

        blazorWebView.HostPage = "wwwroot/index.html";
        blazorWebView.Services = Startup.Services;

        blazorWebView.BlazorWebViewInitialized += BlazorWebViewInitialized;

        blazorWebView.UrlLoading +=
            (sender, urlLoadingEventArgs) =>
            {
                if (urlLoadingEventArgs.Url.Host != "0.0.0.0")
                {
                    //外部链接WebView内打开,例如pdf浏览器
                    Console.WriteLine(urlLoadingEventArgs.Url);
                    urlLoadingEventArgs.UrlLoadingStrategy =
                        UrlLoadingStrategy.OpenInWebView;
                }
            };

    }

    void BlazorWebViewInitialized(object sender, EventArgs e)
    {
        //下载开始时引发 DownloadStarting，阻止默认下载
        blazorWebView.WebView.CoreWebView2.DownloadStarting += CoreWebView2_DownloadStarting;

        //指定下载保存位置
        blazorWebView.WebView.CoreWebView2.Profile.DefaultDownloadFolderPath = UploadPath;

        ////[无依赖发布webview2程序] 固定版本运行时环境的方式来实现加载网页
        ////设置web用户文件夹 
        //var browserExecutableFolder = "c:\\wb2";
        //var userData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BlazorWinFormsApp");
        //Directory.CreateDirectory(userData);
        //var creationProperties = new CoreWebView2CreationProperties()
        //{
        //    UserDataFolder = userData,
        //    BrowserExecutableFolder = browserExecutableFolder
        //};
        //mainBlazorWebView.WebView.CreationProperties = creationProperties;
    }

    private void CoreWebView2_DownloadStarting(object sender, CoreWebView2DownloadStartingEventArgs e)
    {
        var downloadOperation = e.DownloadOperation;
        string fileName = Path.GetFileName(e.ResultFilePath);
        var filePath = Path.Combine(UploadPath, fileName);

        //指定下载保存位置
        e.ResultFilePath = filePath;
        MessageBox.Show( $"下载文件完成 {fileName}", "提示");
    }
    private void ButtonShowCounter_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
          owner: this,
          messageBoxText: $"Current counter value is: {Startup._appState.Counter}",
          caption: "Counter");
    }

    private void ButtonWebviewAlert_Click(object sender, RoutedEventArgs e)
    {
        //blazorWebView.WebView.CoreWebView2.ExecuteScriptAsync("showAlert()");
        blazorWebView.WebView.CoreWebView2.ExecuteScriptAsync("alert('hello from native UI')");
    }

    private void ButtonHome_Click(object sender, RoutedEventArgs e)
    {
        blazorWebView.WebView.CoreWebView2.Navigate("https://0.0.0.0/");
    }
}
