﻿using System;
using System.Collections.Generic;
using System.Linq;
using NekoPetShop.Core.DomainService;
using NekoPetShop.Core.Entity;

namespace NekoPetShop.Core.ApplicationService.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository petRepository;

        public PetService(IPetRepository petRepository)
        {
            this.petRepository = petRepository;
        }

        public void CreatePet(string name, AnimalType type, DateTime birthdate, DateTime soldDate, string color, string previousOwner, double price)
        {
            petRepository.CreatePet(name, type, birthdate, soldDate, color, previousOwner, price);
        }

        public void UpdatePet(int id, string name, AnimalType type, DateTime birthdate, DateTime soldDate, string color, string previousOwner, double price)
        {
            petRepository.UpdatePet(id, name, type, birthdate, soldDate, color, previousOwner, price);
        }

        public void DeletePet(int id)
        {
            petRepository.DeletePet(id);
        }

        public List<Pet> GetPets()
        {
            return petRepository.ReadPets().ToList();
        }

        public List<Pet> SearchPetsByType(AnimalType type)
        {
            List<Pet> filteredPetsList = new List<Pet>();
            foreach (Pet p in petRepository.ReadPets().ToList())
            {
                if (p.Type == type)
                {
                    filteredPetsList.Add(p);
                }
            }
            return filteredPetsList;
        }

        public List<Pet> SortPetsByPrice(bool isAscending)
        {
            List<Pet> allPetsList = petRepository.ReadPets().ToList();
            for (int i = 0; i < allPetsList.Count; i++)
            {
                for (int j = 0; j < allPetsList.Count; j++)
                {
                    double temp;
                    if (isAscending)
                    {
                        if (allPetsList[i].Price > allPetsList[j].Price)
                        {
                            temp = allPetsList[i].Price;
                            allPetsList[i].Price = allPetsList[j].Price;
                            allPetsList[j].Price = temp;
                        }
                    }
                    else
                    {
                        if (allPetsList[i].Price < allPetsList[j].Price)
                        {
                            temp = allPetsList[i].Price;
                            allPetsList[i].Price = allPetsList[j].Price;
                            allPetsList[j].Price = temp;
                        }
                    }
                }
            }
            return allPetsList;
        }

        public List<Pet> GetCheapestPets()
        {
            List<Pet> filteredPetsList = new List<Pet>();
            List<Pet> allPetsList = petRepository.ReadPets().ToList();
            for (int i = 0; i < allPetsList.Count; i++)
            {
                for (int j = 0; j < allPetsList.Count; j++)
                {
                    double temp;
                    if (allPetsList[i].Price < allPetsList[j].Price)
                    {
                        temp = allPetsList[i].Price;
                        allPetsList[i].Price = allPetsList[j].Price;
                        allPetsList[j].Price = temp;
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                filteredPetsList.Add(allPetsList[i]);
            }
            return filteredPetsList;
        }

        public void InitializeData()
        {
            petRepository.InitializeData();
        }
    }
}