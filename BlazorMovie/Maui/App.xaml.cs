﻿using Application = Microsoft.Maui.Controls.Application;

namespace Maui;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }

