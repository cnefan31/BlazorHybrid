﻿// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using BlazorHybrid.Maui.Shared;

namespace bh001_camera_barcode
{
    public partial class MainPage : ContentPage
    {
        InitBlazorWebView initBlazorWebView;

        public MainPage()
        {
            InitializeComponent();

            initBlazorWebView = new InitBlazorWebView(blazorWebView);
        }
    }
}
