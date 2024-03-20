using MNIST;
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
      inputLayer = new double[784];
      hiddenLayer1 = new double[16];
      hiddenLayer2 = new double[16];
      outputLayer = new double[10];

      Random random = new Random();

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
          random2DDoubleArray[y,x] = random.NextDouble() * ((1) - (-1)) + (-1);
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
        randomDoubleArray[i] = random.NextDouble() * ((1) - (-1)) + (-1);
      }
      return randomDoubleArray;
    }

    //Finds what the network thinks the image is
    public double[] FeedForward(byte[] inputLayerBytes, NeuralNetwork network)
    {
      //Turns the inputLayerBytes into doubles and puts them in the network's inputLayer
      for(int i = 0; i < network.inputLayer.Length; i++)
      {
        network.inputLayer[i] = Convert.ToDouble(inputLayerBytes[i]) / 255;
      }

      //Feed forward to hiddenLayer1
      for(int neuronIndex = 0; neuronIndex < network.hiddenLayer1.Length; neuronIndex++)
      {
        double currentStep = 0;
        for(int weightIndex = 0; weightIndex < network.inputLayer.Length; weightIndex++)
        {
          currentStep += network.inputLayerWeights[neuronIndex, weightIndex] * network.inputLayer[weightIndex];
        }
        currentStep += network.inputLayerBiases[neuronIndex];
        currentStep = Math.Tanh(currentStep);
        network.hiddenLayer1[neuronIndex] = currentStep;
      }

      //Feed forward to hiddenLayer2
      for(int neuronIndex = 0; neuronIndex < network.hiddenLayer2.Length; neuronIndex++)
      {
        double currentStep = 0;
        for(int weightIndex = 0; weightIndex < network.hiddenLayer1.Length; weightIndex++)
        {
          currentStep += network.hiddenLayer1Weights[neuronIndex, weightIndex] * network.hiddenLayer1[weightIndex];
        }
        currentStep += network.hiddenLayer1Biases[neuronIndex];
        currentStep = Math.Tanh(currentStep);
        network.hiddenLayer2[neuronIndex] = currentStep;
      }

      //Feed forward to outputLayer
      for(int neuronIndex = 0; neuronIndex < network.outputLayer.Length; neuronIndex++)
      {
        double currentStep = 0;
        for(int weightIndex = 0; weightIndex < network.hiddenLayer2.Length; weightIndex++)
        {
          currentStep += network.hiddenLayer2Weights[neuronIndex, weightIndex] * network.hiddenLayer2[weightIndex];
        }
        currentStep += network.hiddenLayer2Biases[neuronIndex];
        currentStep = Math.Tanh(currentStep);
        network.outputLayer[neuronIndex] = currentStep;
      }

      //Finds the highest value in the output layer to determine the most probable number the image represents
      return network.outputLayer;
    }
  }
}
