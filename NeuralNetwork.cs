using Newtonsoft.Json;
using System;
using System.Linq;

namespace NeuralNetworking
{
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

    //Sigmoid function for squishing any value between 0 and 1 using doubles
    private static double Sigmoid(double value)
    {
      double k = Math.Exp(value);
      return k / (1 + k);
    }

    //Sigmoid function for squishing any value between 0 and 1 using bytes
    private static double Sigmoid(byte valueByte)
    {
      double value = Convert.ToDouble(valueByte);
      double k = Math.Exp(value);
      return k / (1 + k);
    }

    public byte FeedForward(byte[] inputLayerBytes, NeuralNetwork network)
    {
      //Turns the inputLayerBytes into doubles and puts them in the network's inputLayer
      for(int i = 0; i < network.inputLayer.Length; i++)
      {
        //network.inputLayer[i] = Sigmoid(inputLayerBytes[i]);
        network.inputLayer[i] = Convert.ToDouble(inputLayerBytes[i]) / 255;
      }
      
      //Feed forward to hiddenLayer1
      for(int neuronIndex = 0; neuronIndex < network.hiddenLayer1.Length; neuronIndex++)
      {
        double currentStep = 0;
        for(int weightIndex = 0; weightIndex < network.inputLayer.Length; weightIndex++)
        {
          currentStep += (network.inputLayerWeights[neuronIndex, weightIndex] * inputLayer[weightIndex]);
        }
        currentStep += network.inputLayerBiases[neuronIndex];
        currentStep = Sigmoid(currentStep);
        network.hiddenLayer1[neuronIndex] = currentStep;
      }

      //Feed forward to hiddenLayer2
      for(int neuronIndex = 0; neuronIndex < network.hiddenLayer2.Length; neuronIndex++)
      {
        double currentStep = 0;
        for(int weightIndex = 0; weightIndex < network.hiddenLayer1.Length; weightIndex++)
        {
          currentStep += (network.hiddenLayer1Weights[neuronIndex, weightIndex] * hiddenLayer1[weightIndex]);
        }
        currentStep += network.hiddenLayer1Biases[neuronIndex];
        currentStep = Sigmoid(currentStep);
        network.hiddenLayer2[neuronIndex] = currentStep;
      }

      //Feed forward to outputLayer
      for(int neuronIndex = 0; neuronIndex < network.outputLayer.Length; neuronIndex++)
      {
        double currentStep = 0;
        for(int weightIndex = 0; weightIndex < network.hiddenLayer2.Length; weightIndex++)
        {
          currentStep += (network.hiddenLayer2Weights[neuronIndex, weightIndex] * hiddenLayer2[weightIndex]);
        }
        currentStep += network.hiddenLayer2Biases[neuronIndex];
        currentStep = Sigmoid(currentStep);
        network.outputLayer[neuronIndex] = currentStep;
      }
      
      //Finds the highest value in the output layer to determine the most probable number the image represents
      return Convert.ToByte(network.outputLayer.ToList().IndexOf(network.outputLayer.Max()));
    }
  }
}
