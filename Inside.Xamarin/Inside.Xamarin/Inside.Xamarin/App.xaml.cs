﻿using System;
using Inside.Xamarin.Views.Home;
using Inside.Xamarin.Views.Login;
using Inside.Xamarin.Views.Menu;
using Inside.Xamarin.Views.Tabs;
using Inside.Xamarin.Views.Parking;
using Inside.Xamarin.Views.ParkingInfo;
using Xamarin.Forms;
using Inside.Xamarin.Helpers;
using Inside.Xamarin.Views.Master;
using Inside.Xamarin.ViewModels;

namespace Inside.Xamarin
{
    public partial class App : Application
    {
        
        #region Properties
        public static NavigationPage Navigator { get; internal set; }
        public static MasterView Master { get; internal set; }
        #endregion

        public App()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(Settings.Token))
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else {
                MainPage = new MasterView();
            }
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //TODO Esto es una prueba para gestionar aqui cualquier tipo de exepcion que ocurra en la app. 
            Application.Current.MainPage.DisplayAlert("Error General Application Exeption",
                e.ExceptionObject.ToString(),
                "Accept.");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}