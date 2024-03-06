using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Program
{
  public static void Main()
  {
    List<NeuralNetwork> networks = new List<NeuralNetwork>();
    List<string> networkNames = new List<string>();
    NeuralNetwork network = new NeuralNetwork("initialNetwork");
    Console.Clear();
    ShowOptions();
    while(true)
    {
      string command = Prompts.CommandPrompt();

      //Finds the option and does it
      if(command == "show")
      {
        ShowNetworks(networks);
      }
      else if(command == "store")
      {
        if(networks.Count == 0)
        {
          Console.WriteLine("There are no networks to store");
          continue;
        }
        string nameToStore = Prompts.NetworkNameStorePrompt(networkNames);
        for(int i = 0; i < networks.Count; i++)
        {
          if(networks[i].name == nameToStore)
          {
            //Serialize
            string json = networks[i].ToString();

            Console.WriteLine($"Saved {nameToStore} as {json}");
            break;
          }
        }
      }
      else if(command == "make")
      {
        bool useSaveData = Prompts.SaveDataPrompt();
        if(useSaveData)
        {
          Console.Write("Enter the network save data: ");
          string neuralNetworkJson = Console.ReadLine();

          //Deserialize
          network = JsonConvert.DeserializeObject<NeuralNetwork>(neuralNetworkJson);

          if(Checks.NetworkListContainsName(networkNames, network.name))
          {
            Console.WriteLine("The save data already has a network with that name");
            network.name = Prompts.NetworkNamePrompt(networkNames);
          }
          networks.Add(network);
          Console.WriteLine($"{network.name} created from save data");
        }
        else
        {
          string name = Prompts.NetworkNamePrompt(networkNames);
          network = new NeuralNetwork(name);
          networks.Add(network);
          networkNames.Add(name);
          Console.WriteLine($"\nCreated {network.name}");
        }
      }
      else if(command == "delete")
      {
        string nameToDelete = Prompts.NetworkNameDeletePrompt(networkNames);
        for(int i = 0; i < networks.Count; i++)
        {
          if(networks[i].name == nameToDelete)
          {
            networks.RemoveAt(i);
          }
        }
        Console.WriteLine($"\nDeleted {nameToDelete}");
      }
      else if(command == "clear")
      {
        Console.Clear();
      }
      else if(command == "help")
      {
        ShowOptions();
      }
    }
  }

  private static void ShowNetworks(List<NeuralNetwork> neuralNetworks)
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

  private static void ShowOptions()
  {
    Console.WriteLine("Type commands to do stuff:");
    Console.WriteLine("Show list of neural networks:     show");
    Console.WriteLine("Train neural network:             train");
    Console.WriteLine("Store neural network:             store");
    Console.WriteLine("Make new neural network:          make");
    Console.WriteLine("Delete neural network:            delete");
    Console.WriteLine("Clear the screen:                 clear");
    Console.WriteLine("Show this dialogue again:         help");
  }
}

public static class Checks
{
  public static bool NetworkListContainsName(List<string> networkNames, string name)
  {
    for(int i = 0; i < networkNames.Count; i++)
    {
      if(networkNames[i] == name)
      {
        return true;
      }
    }
    return false;
  }

  public static bool IsOption(string option)
  {
    //Goes through the list of commands, and if the option is in there, return that it is a command
    string[] commands = { "show", "train", "store", "make", "delete", "clear", "help" };
    for(int i = 0; i < commands.Length; i++)
    {
      if(commands[i] == option)
      {
        return true;
      }
    }
    return false;
  }
}

public static class Prompts
{
  public static string CommandPrompt()
  {
    Console.Write("\nEnter: ");
    string option = Console.ReadLine();
    while(!Checks.IsOption(option))
    {
      Console.Write("\nNot a command, enter again: ");
      option = Console.ReadLine();
    }
    return option;
  }

  public static string NetworkNameStorePrompt(List<string> networkNames)
  {
    Console.Write("Enter a network to store: ");
    string nameToStore = Console.ReadLine();
    while(!Checks.NetworkListContainsName(networkNames, nameToStore))
    {
      Console.WriteLine("Network wasn't in the saved networks list");
      Console.Write("Enter again: ");
      nameToStore = Console.ReadLine();
    }
    return nameToStore;
  }

  public static string NetworkNameDeletePrompt(List<string> networkNames)
  {
    Console.Write("Enter a network you want to delete: ");
    string nameToDelete = Console.ReadLine();
    while(!Checks.NetworkListContainsName(networkNames, nameToDelete))
    {
      Console.WriteLine("Name wasn't in the saved networks list");
      Console.Write("Enter again: ");
      nameToDelete = Console.ReadLine();
    }
    return nameToDelete;
  }

  public static string NetworkNamePrompt(List<string> networkNames)
  {
    Console.Write("Enter a name for the network: ");
    string name = Console.ReadLine();
    while(name.Length > 20 || name.Length <= 0 || Checks.NetworkListContainsName(networkNames, name))
    {
      Console.WriteLine("Name length needs to be 1-20 characters and not already used");
      Console.Write("Enter again: ");
      name = Console.ReadLine();
    }
    return name;
  }

  public static bool SaveDataPrompt()
  {
    string useSaveDataString;
    do
    {
      Console.Write("Do you want to use save data? (y/n): ");
      useSaveDataString = Console.ReadLine();
    }
    while(useSaveDataString != "y" && useSaveDataString != "n");
    return useSaveDataString == "y";
  }
}

public class NeuralNetwork
{
  public string name;

  //Layers
  public double[] inputLayer;
  public double[] hiddenLayer1;
  public double[] hiddenLayer2;
  public double[] outputLayer;

  //Weights
  public double[,] inputLayerWeights;
  public double[,] hiddenLayer1Weights;
  public double[,] hiddenLayer2Weights;

  //Biases
  public double[] inputLayerBiases;
  public double[] hiddenLayer1Biases;
  public double[] hiddenLayer2Biases;

  //Creates neural network with saved data
  [JsonConstructor]
  public NeuralNetwork(double[] argumentInputLayer, double[] argumentHiddenLayer1, double[] argumentHiddenLayer2, double[] argumentOutputLayer, double[,] argumentInputLayerWeights, double[,] argumentHiddenLayer1Weights, double[,] argumentHiddenLayer2Weights, double[] argumentInputLayerBiases, double[] argumentHiddenLayer1Biases, double[] argumentHiddenLayer2Biases, string argumentName)
  {
    inputLayer = argumentInputLayer;
    hiddenLayer1 = argumentHiddenLayer1;
    hiddenLayer2 = argumentHiddenLayer2;
    outputLayer = argumentOutputLayer;
    inputLayerWeights = argumentInputLayerWeights;
    hiddenLayer1Weights = argumentHiddenLayer1Weights;
    hiddenLayer2Weights = argumentHiddenLayer2Weights;
    inputLayerBiases = argumentInputLayerBiases;
    hiddenLayer1Biases = argumentHiddenLayer1Biases;
    hiddenLayer2Biases = argumentHiddenLayer2Biases;
    name = argumentName;
  }

  //If no neuron information about the network is given, generate random weights and biases
  public NeuralNetwork(string argumentName)
  {
    Random random = new Random();
    inputLayer = RandomDoubleArray(784, random);
    hiddenLayer1 = RandomDoubleArray(16, random);
    hiddenLayer2 = RandomDoubleArray(16, random);
    outputLayer = RandomDoubleArray(10, random);
    inputLayerWeights = Random2DDoubleArray(784, 16, random);
    hiddenLayer1Weights = Random2DDoubleArray(16, 16, random);
    hiddenLayer2Weights = Random2DDoubleArray(16, 10, random);
    inputLayerBiases = RandomDoubleArray(16, random);
    hiddenLayer1Biases = RandomDoubleArray(16, random);
    hiddenLayer2Biases = RandomDoubleArray(10, random);
    name = argumentName;
  }

  public override string ToString()
  {
    return JsonConvert.SerializeObject(this);
  }

  //Makes a randomized 2D double array
  private static double[,] Random2DDoubleArray(int width, int height, Random random)
  {
    double[,] random2DDoubleArray = new double[height, width];
    for(int x = 0; x < width; x++)
    {
      for(int y = 0; y < height; y++)
      {
        random2DDoubleArray[y,x] = random.NextDouble();
      }
    }
    return random2DDoubleArray;
  }

  //Makes a randomized double array
  private static double[] RandomDoubleArray(int length, Random random)
  {
    double[] randomDoubleArray = new double[length];
    for(int i = 0; i < length; i++)
    {
      randomDoubleArray[i] = random.NextDouble();
    }
    return randomDoubleArray;
  }
}
