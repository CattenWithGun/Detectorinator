using ErrorChecks;
using NeuralNetworking;
using NetworkTraining;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UserPrompts;

namespace Actions
{
  public static class Commands
  {
    public static void Train(List<NeuralNetwork> networks, List<string> networkNames)
    {
      if(networks.Count == 0)
      {
        Console.WriteLine("There are no networks to delete");
        return;
      }

      //Get the name of the network to be trained
      string nameToTrain = Prompts.NetworkNameTrainPrompt(networkNames);
      if(nameToTrain == "exit") { return; }

      //Find the network in the list and train it
      for(int i = 0; i < networks.Count; i++)
      {
        if(networks[i].name == nameToTrain)
        {
          NeuralNetwork network = Training.TrainNetwork(networks[i]);
          return;
        }
      }
    }

    public static void Delete(List<NeuralNetwork> networks, List<string> networkNames)
    {
      if(networks.Count == 0)
      {
        Console.WriteLine("There are no networks to delete");
        return;
      }

      //Get the name of the network to be deleted
      string nameToDelete = Prompts.NetworkNameDeletePrompt(networkNames);
      if(nameToDelete == "exit") { return; }

      //Find the network in the list and delete it
      for(int i = 0; i < networks.Count; i++)
      {
        if(networks[i].name == nameToDelete)
        {
          networks.RemoveAt(i);
          networkNames.RemoveAt(i);
        }
      }
      Console.WriteLine($"\nDeleted {nameToDelete}");
    }

    public static void Make(List<NeuralNetwork> networks, List<string> networkNames)
    {
      if(networks.Count == 20)
      {
        Console.WriteLine("You know what? No. You don't get anymore than 20 networks because no sane human being would ever need that many.");
        return;
      }
      string useSaveDataString = Prompts.SaveDataPrompt();
      if(useSaveDataString == "exit") { return; }
      bool useSaveData = useSaveDataString == "y";

      if(useSaveData)
      {
        //Get save data from user
        Console.Write("Enter the network save data: ");
        string networkJson = Console.ReadLine();
        if(networkJson == "exit") { return; }

        //Convert to a NeuralNetwork
        NeuralNetwork network = JsonToNetwork(networkJson);
        if(network == null) { return; }

        //If there is already a network with the name, make the user make a new one
        if(Checks.NetworkListContainsName(networkNames, network.name))
        {
          Console.WriteLine("There is already a network with that name");
          string name = Prompts.NetworkNamePrompt(networkNames);
          if(name == "exit") { return; }
          network.name = name;
        }

        //Add the network to list of networks
        networkNames.Add(network.name);
        networks.Add(network);
        Console.WriteLine($"{network.name} created from save data");
      }
      else
      {
        //Gets name
        string name = Prompts.NetworkNamePrompt(networkNames);
        if(name == "exit") { return; }

        //Creates a randomized network with the name
        NeuralNetwork network = new NeuralNetwork(name);

        //Add the network to list of networks
        networks.Add(network);
        networkNames.Add(name);
        Console.WriteLine($"\nCreated {network.name}");
      }
    }  

    public static void Store(List<NeuralNetwork> networks, List<string> networkNames)
    {
      if(networks.Count == 0)
      {
        Console.WriteLine("There are no networks to store");
        return;
      }
      string nameToStore = Prompts.NetworkNameStorePrompt(networkNames);
      if(nameToStore == "exit") { return; }
      for(int i = 0; i < networks.Count; i++)
      {
        if(networks[i].name == nameToStore)
        {
          //Serialize
          string json = networks[i].ToString();
          Console.WriteLine($"Saved {nameToStore} as {json}");
          return;
        }
      }
    }

    public static void ShowCommands()
    {
      Console.WriteLine("Type commands to do stuff:");
      Console.WriteLine("Show list of neural networks:     show");
      Console.WriteLine("Train neural network:             train");
      Console.WriteLine("Store neural network:             store");
      Console.WriteLine("Make new neural network:          make");
      Console.WriteLine("Delete neural network:            delete");
      Console.WriteLine("Exits out of a prompt:            exit");
      Console.WriteLine("Clear the screen:                 clear");
      Console.WriteLine("Show this dialogue again:         help");
    }

    public static void ShowNetworks(List<NeuralNetwork> neuralNetworks)
    {
      Console.WriteLine("\nSaved Neural Networks:\n");
      if(neuralNetworks.Count == 0)
      {
        Console.WriteLine("No saved networks here");
        return;
      }
      for(int i = 0; i < neuralNetworks.Count; i++)
      {
        Console.WriteLine(neuralNetworks[i].name);
      }
    }

    private static NeuralNetwork JsonToNetwork(string networkJson)
    {
      try
      {
        return JsonConvert.DeserializeObject<NeuralNetwork>(networkJson);
      }
      catch
      {
        Console.WriteLine("Error reading save data");
        return null;
      }
    }
  }
}
