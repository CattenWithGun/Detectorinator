using BackPropagationHelper;
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

	  //Goofy numbers are making a random double between -1 and 1
      inputLayerBiases = RandomDoubleArray(16, random);
      hiddenLayer1Biases = RandomDoubleArray(16, random);
      hiddenLayer2Biases = RandomDoubleArray(10, random);
      name = argumentName;
    }

    public override string ToString()
    {
      return JsonConvert.SerializeObject(this);
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

    //Finds what the network thinks the image is
    public double[] FeedForward(NeuralNetwork network, byte[] inputLayerBytes)
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

      //Returns the outputLayer
      return network.outputLayer;
    }

    public byte FeedForwardAndGetGuess(NeuralNetwork network, byte[] inputLayerBytes)
    {
      double[] outputLayer = network.FeedForward(network, inputLayerBytes);
      return Convert.ToByte(network.outputLayer.ToList().IndexOf(network.outputLayer.Max()));
    }

    public double Error(NeuralNetwork network, byte[] inputLayerBytes, byte expectedValueByte)
    {
      double[] outputLayer = network.FeedForward(network, inputLayerBytes);
      double[] errors = new double[network.outputLayer.Length];
      double error = 0;
      for(int i = 0; i < errors.Length; i++)
      {
        int expectedValue = Convert.ToInt32(i == Convert.ToInt32(expectedValueByte));
        errors[i] = 0.5 * Math.Pow(expectedValue - outputLayer[i], 2);
        error += errors[i];
      }
      return error;
    }

    private void ShowDoubles(string message, double[,] doubles)
    {
      string row = "";
      Console.WriteLine(message);
      for(int height = 0; height < doubles.GetLength(0); height++)
      {
        for(int width = 0; width < doubles.GetLength(1); width++)
        {
          row += Convert.ToString(doubles[height, width]) + ", ";
        }
        Console.WriteLine(row);
        row = "";
      }
    }

    //Makes 1 gradient descent step
    public NeuralNetwork BackPropagate(NeuralNetwork network, double[] expectedValues, double learningRate)
    {
      //Backpropagates the connections between hiddenLayer2 and the outputLayer
      double[] errorWithOutputs = BackPropagation.ErrorWithRespectToOutputs(network, expectedValues);
      double[] outputsWithTanh = BackPropagation.OutputsWithRespectToTanh(network);
      double[,] newHiddenLayer2Weights = BackPropagation.NewHiddenLayer2Weights(network, learningRate, errorWithOutputs, outputsWithTanh);
      double[] newHiddenLayer2Biases = BackPropagation.NewBiases(network.outputLayer, errorWithOutputs, learningRate);

      //Backpropagates the connections between hiddenLayer1 and hiddenLayer2
      double[] errorWithHiddenLayer2 = BackPropagation.ErrorWithRespectToHiddenLayer2(network, errorWithOutputs, outputsWithTanh);
      double[] hiddenLayer2WithTanh = BackPropagation.HiddenLayer2WithRespectToTanh(network);
      double[,] newHiddenLayer1Weights = BackPropagation.NewHiddenLayer1Weights(network, learningRate, errorWithHiddenLayer2, hiddenLayer2WithTanh);
      double[] newHiddenLayer1Biases = BackPropagation.NewBiases(network.hiddenLayer2, errorWithHiddenLayer2, learningRate);

      //Backpropagates the connections between the inputLayer and hiddenLayer1
      double[] errorWithHiddenLayer1 = BackPropagation.ErrorWithRespectToHiddenLayer1(network, errorWithHiddenLayer2, hiddenLayer2WithTanh);
      double[] hiddenLayer1WithTanh = BackPropagation.HiddenLayer1WithRespectToTanh(network);
      double[,] newInputLayerWeights = BackPropagation.NewInputLayerWeights(network, learningRate, errorWithHiddenLayer1, hiddenLayer1WithTanh);
      double[] newInputLayerBiases = BackPropagation.NewBiases(network.hiddenLayer1, errorWithHiddenLayer1, learningRate);
      
      //Sets the weights to their new values, these aren't set earlier because the partial derivative needs the values to be unchanged
      network.hiddenLayer2Weights = newHiddenLayer2Weights;
      network.hiddenLayer1Weights = newHiddenLayer1Weights;
      network.inputLayerWeights = newInputLayerWeights;
      
      return network;
    }
  }
}
