﻿using System;
using System.Threading;
using System.Collections.Generic;
using NekoPetShop.Core.Entity;
using NekoPetShop.Application.Util;
using NekoPetShop.Core.DomainService;
using NekoPetShop.Core.ApplicationService;
using NekoPetShop.Infrastructure.Repositories;
using NekoPetShop.Core.ApplicationService.Services;
using Microsoft.Extensions.DependencyInjection;

namespace NekoPetShop.Application.Views
{
    class OwnerView : IView
    {
        private readonly IOwnerService ownerService;
        private readonly string FILEPATHOWNERLAYOUT = AppContext.BaseDirectory + "\\TxtFiles\\OwnerViewLayout.txt";


        public OwnerView(IOwnerService ownerService)
        {
            this.ownerService = ownerService;
        }

        public void Initialize()
        {
            Console.Clear();
            ShowMenuLayoutASCII();
            ShowOwnerListLayout();
            ShowOwnerListData(ownerService.GetOwners());
            Console.ReadLine();
        }

        private void ShowMenuLayoutASCII()
        {
            ASCIIAnimator.Instance.CreateASCIIAnimation(FILEPATHOWNERLAYOUT, 0, 0);
        }

        private void ShowOwnerListLayout()
        {
            string listOfOwnersText = "LIST OF OWNERS";
            Console.SetCursorPosition((Console.WindowWidth - listOfOwnersText.Length) / 2, 4);
            Console.WriteLine(listOfOwnersText);
            string listTabs = "|  #      FIRST NAME    LAST NAME    ADDRESS               PHONE NUMBER      EMAIL";
            Console.SetCursorPosition(18, 7);
            Console.WriteLine(listTabs);
            string spacingTabText = "|----------------------------------------------------------------------------------------------------";
            Console.SetCursorPosition(18, 8);
            Console.WriteLine(spacingTabText);
        }

        private void ShowOwnerListData(List<Owner> ownersList)
        {
            int countTop = 9;
            foreach (Owner o in ownersList)
            {
                string ownerListing = ($"|  {o.Id} {GetFixedSpacing(o.Id.ToString().Length, 5)} {o.FirstName} {GetFixedSpacing(o.FirstName.Length, 12)} {o.LastName} {GetFixedSpacing(o.LastName.Length, 11)} {o.Address} {GetFixedSpacing(o.Address.Length, 20)} {o.PhoneNumber} {GetFixedSpacing(o.PhoneNumber.Length, 16)} {o.Email}");
                Console.SetCursorPosition(18, countTop);
                Console.WriteLine(ownerListing);
                countTop++;

                string spacingText = "|----------------------------------------------------------------------------------------------------";
                Console.SetCursorPosition(18, countTop);
                Console.WriteLine(spacingText);
                countTop++;
            }
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("");
            UserMenu();
        }

        private string GetFixedSpacing(int propertyLength, int maxSpacing)
        {
            string fixedSpacing = "";
            int spacingToAdd = maxSpacing - propertyLength;
            for (int i = 0; i < spacingToAdd; i++)
            {
                fixedSpacing += " ";
            }
            return fixedSpacing;
        }

        private void UserMenu()
        {
            bool allowOptionChoosing = true;
            Console.SetCursorPosition(0, 9);
            ConsoleKeyInfo cki;
            do
            {
                while (Console.KeyAvailable == false)
                    Thread.Sleep(250);
                cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.D0)
                {
                    allowOptionChoosing = false;
                    SwitchToPetView();
                }
                else if (cki.Key == ConsoleKey.D1)
                {
                    allowOptionChoosing = false;
                    CreateOwner();
                }
                else if (cki.Key == ConsoleKey.D2)
                {
                    allowOptionChoosing = false;
                    DeleteOwner();
                }
                else if (cki.Key == ConsoleKey.D3)
                {
                    allowOptionChoosing = false;
                    UpdateOwner();
                }
            } while (allowOptionChoosing);
        }

        private void SwitchToPetView()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IPetRepository, PetRepository>();
            serviceCollection.AddScoped<IPetService, PetService>();
            serviceCollection.AddScoped<IView, PetView>();
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            IOwnerService ownerService = serviceProvider.GetRequiredService<IOwnerService>();
            IPetService petService = serviceProvider.GetRequiredService<IPetService>();
            IView petView = serviceProvider.GetRequiredService<IView>();
            petView.Initialize();
        }

        private void CreateOwner()
        {
            Console.WriteLine("-Add-");
            Console.WriteLine("First name:");
            string firstName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Last name:");
            string lastName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Address:");
            string address = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Phone number:");
            string phoneNumber = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Email:");
            string email = Console.ReadLine();
            Console.WriteLine();
            ownerService.CreateOwner(firstName, lastName, address, phoneNumber, email);
            ClearOwnerList();
            ShowOwnerListData(ownerService.GetOwners());
        }

        private void DeleteOwner()
        {
            Console.WriteLine("-Delete-");
            Console.WriteLine("Choose id: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine(GetRandomizedInsult());
            }
            ownerService.DeleteOwner(id);
            ClearOwnerList();
            ShowOwnerListData(ownerService.GetOwners());
        }

        private void UpdateOwner()
        {
            Console.WriteLine("-Update-");
            Console.WriteLine("Choose id: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine(GetRandomizedInsult());
            }
            Console.WriteLine("First name:");
            string firstName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Last name:");
            string lastName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Address:");
            string address = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Phone number:");
            string phoneNumber = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Email:");
            string email = Console.ReadLine();
            Console.WriteLine();
            ownerService.UpdateOwner(id, firstName, lastName, address, phoneNumber, email);
            ClearOwnerList();
            ShowOwnerListData(ownerService.GetOwners());
        }

        private void ClearOwnerList()
        {
            int topCount = 9;
            int lengthToDelete = Console.CursorTop;
            for (int i = 0; i < Console.CursorTop; i++)
            {
                Console.SetCursorPosition(0, topCount);
                Console.Write(new string(' ', 500));
                topCount++;
            }

            topCount = 9;
            int listDefaultLength = 21;
            for (int i = 0; i < listDefaultLength; i++)
            {
                Console.SetCursorPosition(18, topCount);
                Console.Write('|');
                topCount++;
            }
        }

        private string GetRandomizedInsult()
        {
            string insultToReturn = "";
            Random random = new Random();
            int insultNumber = random.Next(1, 10);
            switch (insultNumber)
            {
                case 1:
                    insultToReturn = "wat is this dumville";
                    break;
                case 2:
                    insultToReturn = "nuh uh man";
                    break;
                case 3:
                    insultToReturn = "sucks.";
                    break;
                case 4:
                    insultToReturn = "i dun even man";
                    break;
                case 5:
                    insultToReturn = "is ur head typing";
                    break;
                case 6:
                    insultToReturn = "ur mom";
                    break;
                case 7:
                    insultToReturn = "thats just stoopid";
                    break;
                case 8:
                    insultToReturn = "try again u dum";
                    break;
                case 9:
                    insultToReturn = "u dun deserve a pet";
                    break;
                case 10:
                    insultToReturn = ":/";
                    break;
            }
            return insultToReturn;
        }
    }
}