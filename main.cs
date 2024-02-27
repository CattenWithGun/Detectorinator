using System;

public class Program
{
  public static void Main()
  {
    NeuralNetwork network = new NeuralNetwork();
    Console.WriteLine("Hello World");
  }
}

public class NeuralNetwork
{
  //Layers
  byte[] inputLayer = new byte[784];
  byte[] hiddenLayer1 = new byte[16];
  byte[] hiddenLayer2 = new byte[16];
  byte[] outputLayer = new byte[10];

  //Weights
  byte[,] inputLayerWeights = RandomStartingWeights(784,16);
  byte[,] hiddenLayer1Weights = RandomStartingWeights(16,16);
  byte[,] hiddenLayer2Weights = RandomStartingWeights(16,10);

  //Biases
  byte[] inputLayerBiases = new byte[16];
  byte[] hiddenLayer1Biases = new byte[16];
  byte[] hiddenLayer2Biases = new byte[10];

  //Makes a randomized byte array
  private static byte[,] RandomStartingWeights(int width, int height)
  {
    byte[,] randomWeights = new byte[width, height];
    Random random = new Random();
    for(int y = 0; y < height; y++)
    {
      for(int x = 0; x < width; x++)
      {
        randomWeights[x,y] = Convert.ToByte(random.Next(0, 255));
      }
    }
    return randomWeights;
  }
}
