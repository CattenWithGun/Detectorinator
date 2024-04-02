using Newtonsoft.Json;
using System;

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
    public double inputLayerBiase;
    public double hiddenLayer1Biase;
    public double hiddenLayer2Biase;

    //Creates neural network with saved data
    [JsonConstructor]
    public NeuralNetwork(double[] argumentInputLayer, double[] argumentHiddenLayer1, double[] argumentHiddenLayer2, double[] argumentOutputLayer, double[,] argumentInputLayerWeights, double[,] argumentHiddenLayer1Weights, double[,] argumentHiddenLayer2Weights, double argumentInputLayerBiase, double argumentHiddenLayer1Biase, double argumentHiddenLayer2Biase, string argumentName)
    {
      inputLayer = argumentInputLayer;
      hiddenLayer1 = argumentHiddenLayer1;
      hiddenLayer2 = argumentHiddenLayer2;
      outputLayer = argumentOutputLayer;
      inputLayerWeights = argumentInputLayerWeights;
      hiddenLayer1Weights = argumentHiddenLayer1Weights;
      hiddenLayer2Weights = argumentHiddenLayer2Weights;
      inputLayerBiase = argumentInputLayerBiase;
      hiddenLayer1Biase = argumentHiddenLayer1Biase;
      hiddenLayer2Biase = argumentHiddenLayer2Biase;
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
      inputLayerBiase = random.NextDouble() * ((1) - (-1)) + (-1);
      hiddenLayer1Biase = random.NextDouble() * ((1) - (-1)) + (-1);
      hiddenLayer2Biase = random.NextDouble() * ((1) - (-1)) + (-1);
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
        currentStep += network.inputLayerBiase;
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
        currentStep += network.hiddenLayer1Biase;
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
        currentStep += network.hiddenLayer2Biase;
        currentStep = Math.Tanh(currentStep);
        network.outputLayer[neuronIndex] = currentStep;
      }

      //Returns the outputLayer
      return network.outputLayer;
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

    public NeuralNetwork BackPropagate(NeuralNetwork network, double[] expectedValues)
    {
      ShowDoubles("Original Weights:", network.hiddenLayer2Weights);

      //Finds how much the total error changes with respect to the outputs
      double[] errorWithOutputs = new double[network.outputLayer.Length];
      for(int i = 0; i < errorWithOutputs.Length; i++)
	  {
		  errorWithOutputs[i] = network.outputLayer[i] - expectedValues[i];
	  }

	  //Finds how much the outputs change with respect to the tanh function
	  double[] outputsWithTanh = new double[network.outputLayer.Length];
	  for(int i = 0; i < outputsWithTanh.Length; i++)
	  {
		outputsWithTanh[i] = 1 - (Math.Pow(Math.Tanh(network.outputLayer[i]), 2));
	  }

	  //Finds how much input to tanh changes with respect to the weights
	  //double[]
	  
      ShowDoubles("New Weights:", network.hiddenLayer2Weights);
      return network;
    }
  }
}
