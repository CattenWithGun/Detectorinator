using System;
using System.Collections.Generic;

public class Program
{
  public static void Main()
  {
    List<NeuralNetwork> neuralNetworks = new List<NeuralNetwork>();
    NeuralNetwork network = new NeuralNetwork("initialNetwork");
    while(true)
    {
      ShowOptions();
      
      //Gets an option
      Console.Write("Enter: ");
      string option = Console.ReadLine();
      while(!IsOption(option))
      {
        Console.Write("Not a command, enter again: ");
        option = Console.ReadLine();
      }

      //Finds the option and does it
      if(option == "show")
      {
        ShowNetworks(neuralNetworks);
      }
      else if(option == "make")
      {
        string name = GetNetworkName();
        bool useSaveData = UseSaveData();
        if(useSaveData)
        {
          Console.WriteLine("unfinished");
        }
        else
        {
          //might need to make sure this doesn't do a memory leak
          network = new NeuralNetwork(name);
          neuralNetworks.Add(network);
          Console.WriteLine($"Created {network.name}");
        }
      }
      else if(option == "help")
      {
        ShowOptions();
      }
    }
  }

  private static string GetNetworkName()
  {
    Console.Write("Enter a name for the network: ");
    string name = Console.ReadLine();
    while(name.Length > 20 || name.Length <= 0)
    {
      Console.WriteLine("Name length needs to be 1-20 characters");
      Console.Write("Enter again: ");
      name = Console.ReadLine();
    }
    return name;
  }

  private static bool UseSaveData()
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

  private static void ShowNetworks(List<NeuralNetwork> neuralNetworks)
  {
    Console.WriteLine("Saved Neural Networks:\n");
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
    Console.WriteLine("Show this dialogue again:         help");
  }

  private static bool IsOption(string option)
  {
    //Goes through the list of commands, and if the option is in there, return that it is a command
    string[] commands = { "show", "train", "store", "make", "delete", "help" };
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
  public NeuralNetwork(double[] argumentInputLayer, double[] argumentHiddenLayer1, double[] argumentHiddenLayer2, double[] argumentOutputLayer, double[,] argumentInputLayerWeights, double[,] argumentHiddenLayer1Weights, double[,] argumentHiddenLayer2Weights, double[] argumentInputLayerBiases, double[] argumentHiddenLayer1Biases, double[] argumentHiddenLayer2Biases, string constructName)
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
    name = constructName;
  }

  //If no neuron information about the network is given, generate random weights and biases
  public NeuralNetwork(string constructName)
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
    name = constructName;
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
