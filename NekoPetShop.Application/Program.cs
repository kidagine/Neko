﻿using System;
using NekoPetShop.Application.Views;

namespace NekoPetShop.Application
{
    class Program
    {
        static void Main()
        {
            Console.Title = "NEKO-PetShop";
            StartUpView startUpView = new StartUpView();
            startUpView.Initialize();
        }
    }
}
