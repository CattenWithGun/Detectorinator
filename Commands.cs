using ErrorChecks;
using MNIST;
using NeuralNetworking;
using NetworkTraining;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UserPrompts;

namespace Actions
{
  public static class Commands
  {
    public static void Train(List<NeuralNetwork> networks, List<string> networkNames)
    {
      if(networks.Count == 0)
      {
        Console.WriteLine("There are no networks to train");
        return;
      }

      //Get the name of the network to be trained
      string nameToTrain = Prompts.NetworkNameTrainPrompt(networkNames);
      if(nameToTrain == "exit") { return; }

      //Get the learning rate
      string learningRateString = Prompts.LearningRatePrompt();
      if(learningRateString == "exit") { return; }
      double learningRate = Convert.ToDouble(learningRateString);

      //Find the network in the list and train it
      for(int i = 0; i < networks.Count; i++)
      {
        if(networks[i].name == nameToTrain)
        {
          NeuralNetwork network = Training.TrainNetwork(networks[i], learningRate);
          return;
        }
      }
    }

    public static void Test(List<NeuralNetwork> networks, List<string> networkNames)
    {
      if(networks.Count == 0)
      {
        Console.WriteLine("There are no networks to test");
        return;
      }
      
      //Get the name of the network to be tested
      string nameToTest = Prompts.NetworkNameTestPrompt(networkNames);
      if(nameToTest == "exit") { return; }

      //Find the network in the list
      NeuralNetwork networkToTest = new NeuralNetwork("placeholder");
      for(int i = 0; i < networks.Count; i++)
      {
        if(networks[i].name == nameToTest)
        {
          networkToTest = networks[i];
        }
      }

      string exit = "";
      Random random = new Random();
      byte[] labels = MNISTFileHandler.GetLabels("/home/runner/NeuralNetwork/MNIST_Test_Database/labels");
      byte[,,] images = MNISTFileHandler.GetImages("/home/runner/NeuralNetwork/MNIST_Test_Database/images");
      int imageIndex = 0;
      while(exit != "exit")
      {
        imageIndex = random.Next(0, labels.Length);
        MNISTFileHandler.WriteImage(imageIndex, images, labels);
        Console.Write($"{networkToTest.name} guessed that the image was of a {networkToTest.FeedForwardAndGetGuess(networkToTest, MNISTFileHandler.ImageToByteArray(images, imageIndex))}.\nEnter 'exit' to exit, or enter anything else to continue: ");
        exit = Console.ReadLine();
      }
    }

    public static void PrintError(List<NeuralNetwork> networks, List<string> networkNames)
    {
      if(networks.Count == 0)
      {
        Console.WriteLine("There are no networks to get the error of");
        return;
      }

      //Get the name of the network to be deleted
      string nameToGetErrorOf = Prompts.NetworkNameErrorPrompt(networkNames);
      if(nameToGetErrorOf == "exit") { return; }

      //Find the network in the list and print the error of it
      NeuralNetwork networkToGetErrorOf = new NeuralNetwork("placeholder");
      for(int i = 0; i < networks.Count; i++)
      {
        if(networks[i].name == nameToGetErrorOf)
        {
          networkToGetErrorOf = networks[i];
        }
      }

      //Finds the average error of the network
      byte[] labels = MNISTFileHandler.GetLabels("/home/runner/NeuralNetwork/MNIST_Test_Database/labels");
      byte[,,] images = MNISTFileHandler.GetImages("/home/runner/NeuralNetwork/MNIST_Test_Database/images");
      double[] errors = new double[labels.Length];
      for(int i = 0; i < labels.Length; i++)
      {
        byte[] imageBytes = MNISTFileHandler.ImageToByteArray(images, i);
        errors[i] = networkToGetErrorOf.Error(networkToGetErrorOf, imageBytes, labels[i]);
      }

      //Prints the average error
      double averageError = 0;
      for(int i = 0; i < errors.Length; i++)
      {
        averageError += errors[i];
      }
      averageError /= errors.Length;      

      //Finds the percent that the network guesses correctly
      double correctGuesses = 0;
      for(int i = 0; i < labels.Length; i++)
      {
        if(networkToGetErrorOf.FeedForwardAndGetGuess(networkToGetErrorOf, MNISTFileHandler.ImageToByteArray(images, i)) == labels[i])
        {
          correctGuesses++;
        }
      }
      double percent = correctGuesses / Convert.ToDouble(labels.Length);

      Console.WriteLine($"The percent the network guessed correctly was {percent * 100}%");
      Console.WriteLine($"The error of {networkToGetErrorOf.name} is: {averageError}");
    }

    public static void PrintOverfittedError(NeuralNetwork networkToGetErrorOf, byte[] labels, byte[,,] images)
    {
      //Finds the average error of the network
      int numberOfTests = labels.Length;
      double error = 0;
      byte[] imageBytes = MNISTFileHandler.ImageToByteArray(images, 0);
      error = networkToGetErrorOf.Error(networkToGetErrorOf, imageBytes, labels[0]);

      //Prints the average error
      double averageError = error;

      //Finds the percent that the network guesses correctly
      double correctGuesses = 0;
      if(networkToGetErrorOf.FeedForwardAndGetGuess(networkToGetErrorOf, MNISTFileHandler.ImageToByteArray(images, 0)) == labels[0])
      {
        correctGuesses = 1;
      }
      double percent = correctGuesses;

      Console.WriteLine($"The percent the network guessed correctly was {percent * 100}%");
      Console.WriteLine($"The error of {networkToGetErrorOf.name} is: {averageError}");
    }

    public static void Overfit(List<NeuralNetwork> networks, List<string> networkNames)
    {
      if(networks.Count == 0)
      {
        Console.WriteLine("There are no networks to overfit");
        return;
      }

      //Get the name of the network to be overfitted
      string nameToOverfit = Prompts.NetworkNameOverfitPrompt(networkNames);
      if(nameToOverfit == "exit") { return; }

      //Get the learning rate
      string learningRateString = Prompts.LearningRatePrompt();
      if(learningRateString == "exit") { return; }
      double learningRate = Convert.ToDouble(learningRateString);
      
      //Find the network in the list
      NeuralNetwork network = new NeuralNetwork("placeholder");
      for(int i = 0; i < networks.Count; i++)
      {
        if(networks[i].name == nameToOverfit)
        {
          network = networks[i];
        }
      }

      //Overfits the network and shows the overfitted error
      network = Training.OverfitNetwork(network, learningRate);
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
          File.WriteAllText("/home/runner/NeuralNetwork/NetworkFile.txt", json);
          Console.WriteLine($"Saved {nameToStore} in /home/runner/NeuralNetwork/NetworkFile.txt");
          return;
        }
      }
    }

    public static void ShowCommands()
    {
      Console.WriteLine("Type commands to do stuff:");
      Console.WriteLine("Show list of neural networks:     show");
      Console.WriteLine("Train neural network:             train");
      Console.WriteLine("Test the network:                 test");
      Console.WriteLine("Get the error of the network:     error");
      Console.WriteLine("Overfit the network:              overfit");
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
