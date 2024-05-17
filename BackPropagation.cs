using NeuralNetworking;
using System;

namespace BackPropagationHelper
{
  public static class BackPropagation
  {
    public static double[] ErrorWithRespectToOutputs(NeuralNetwork network, double[] expectedValues)
    {
      double[] errorWithOutputs = new double[network.outputLayer.Length];
      for(int i = 0; i < network.outputLayer.Length; i++)
      {
        errorWithOutputs[i] = -(expectedValues[i] - network.outputLayer[i]);
      }
      return errorWithOutputs;
    }

    public static double[] OutputsWithRespectToTanh(NeuralNetwork network)
    {
      double[] outputsWithTanh = new double[network.outputLayer.Length];
      for(int i = 0; i < outputsWithTanh.Length; i++)
      {
        outputsWithTanh[i] = 1 - (Math.Pow(Math.Tanh(network.outputLayer[i]), 2));
      }
      return outputsWithTanh;
    }

    public static double[,] NewHiddenLayer2Weights(NeuralNetwork network, double learningRate, double[] errorWithOutputs, double[] outputsWithTanh)
    {
      double[,] newHiddenLayer2Weights = network.hiddenLayer2Weights;
      //Iterates over each connection between hiddenLayer2 and outputLayer
      for(int outputLayerIndex = 0; outputLayerIndex < network.outputLayer.Length; outputLayerIndex++)
      {
        for(int hiddenLayer2Index = 0; hiddenLayer2Index < network.hiddenLayer2.Length; hiddenLayer2Index++)
        {
          newHiddenLayer2Weights[outputLayerIndex, hiddenLayer2Index] -= learningRate * (errorWithOutputs[outputLayerIndex] * outputsWithTanh[outputLayerIndex] * network.hiddenLayer2[hiddenLayer2Index]);
        }
      }
      return newHiddenLayer2Weights;
    }

    public static double[] NewBiases(double[] biases, double[] errorWithLayer, double learningRate)
    {
      double[] newBiases = biases;
      for(int i = 0; i < biases.Length; i++)
      {
        newBiases[i] -= learningRate * errorWithLayer[i];
      }
      return newBiases;
    }

    public static double[] ErrorWithRespectToHiddenLayer2(NeuralNetwork network, double[] errorWithOutputs, double[] outputsWithTanh)
    {
      //Contains all the quells after addition
      double[] errorWithHiddenLayer2 = new double[network.hiddenLayer2.Length];
      //Contains all the quells before addition
      double[,] partialErrorsWithHiddenLayer2 = new double[network.hiddenLayer2.Length, network.outputLayer.Length];
      for(int hiddenLayer2Index = 0; hiddenLayer2Index < network.hiddenLayer2.Length; hiddenLayer2Index++)
      {
        for(int outputLayerIndex = 0; outputLayerIndex < network.outputLayer.Length; outputLayerIndex++)
        {
          double hiddenLayer2Weight = network.hiddenLayer2Weights[outputLayerIndex, hiddenLayer2Index];
          partialErrorsWithHiddenLayer2[hiddenLayer2Index, outputLayerIndex] = errorWithOutputs[outputLayerIndex] * outputsWithTanh[outputLayerIndex] * hiddenLayer2Weight;
        }
      }
      //Adds the partialErrors across the rows
      double error = 0;
      for(int row = 0; row < partialErrorsWithHiddenLayer2.GetLength(0); row++)
      {
        for(int index = 0; index < partialErrorsWithHiddenLayer2.GetLength(1); index++)
        {
          error += partialErrorsWithHiddenLayer2[row, index];
        }
        errorWithHiddenLayer2[row] = error;
        error = 0;
      }
      
      return errorWithHiddenLayer2;
    }

    public static double[] HiddenLayer2WithRespectToTanh(NeuralNetwork network)
    {
      double[] hiddenLayer2WithTanh = new double[network.hiddenLayer2.Length];
      for(int i = 0; i < hiddenLayer2WithTanh.Length; i++)
      {
        hiddenLayer2WithTanh[i] = 1 - (Math.Pow(Math.Tanh(network.hiddenLayer2[i]), 2));
      }
      return hiddenLayer2WithTanh;
    }

    public static double[,] NewHiddenLayer1Weights(NeuralNetwork network, double learningRate, double[] errorWithHiddenLayer2, 
double[] hiddenLayer2WithTanh)
    {
      double[,] newHiddenLayer1Weights = network.hiddenLayer1Weights;
      //Iterates over each connection between hiddenLayer1 and hiddenLayer2
      for(int hiddenLayer2Index = 0; hiddenLayer2Index < network.hiddenLayer2.Length; hiddenLayer2Index++)
      {
        for(int hiddenLayer1Index = 0; hiddenLayer1Index < network.hiddenLayer1.Length; hiddenLayer1Index++)
        {
          newHiddenLayer1Weights[hiddenLayer2Index, hiddenLayer1Index] -= learningRate * (errorWithHiddenLayer2[hiddenLayer2Index] * hiddenLayer2WithTanh[hiddenLayer2Index] * network.hiddenLayer1[hiddenLayer1Index]);
        }
      }
      return newHiddenLayer1Weights;
    }

    public static double[] ErrorWithRespectToHiddenLayer1(NeuralNetwork network, double[] errorWithHiddenLayer2, double[] hiddenLayer2WithTanh)
    {
      //Contains all the quells after addition
      double[] errorWithHiddenLayer1 = new double[network.hiddenLayer1.Length];
      //Contains all the quells before addition
      double[,] partialErrorsWithHiddenLayer1 = new double[network.hiddenLayer1.Length, network.hiddenLayer2.Length];
      for(int hiddenLayer1Index = 0; hiddenLayer1Index < network.hiddenLayer1.Length; hiddenLayer1Index++)
      {
        for(int hiddenLayer2Index = 0; hiddenLayer2Index < network.hiddenLayer2.Length; hiddenLayer2Index++)
        {
          double hiddenLayer1Weight = network.hiddenLayer1Weights[hiddenLayer2Index, hiddenLayer1Index];
          partialErrorsWithHiddenLayer1[hiddenLayer1Index, hiddenLayer2Index] = errorWithHiddenLayer2[hiddenLayer2Index] * hiddenLayer2WithTanh[hiddenLayer2Index] * hiddenLayer1Weight;
        }
      }
      //Adds the partialErrors across the rows
      double error = 0;
      for(int row = 0; row < partialErrorsWithHiddenLayer1.GetLength(0); row++)
      {
        for(int index = 0; index < partialErrorsWithHiddenLayer1.GetLength(1); index++)
        {
          error += partialErrorsWithHiddenLayer1[row, index];
        }
        errorWithHiddenLayer1[row] = error;
        error = 0;
      }

      return errorWithHiddenLayer1;
    }

    public static double[] HiddenLayer1WithRespectToTanh(NeuralNetwork network)
    {
      double[] hiddenLayer1WithTanh = new double[network.hiddenLayer1.Length];
      for(int i = 0; i < hiddenLayer1WithTanh.Length; i++)
      {
        hiddenLayer1WithTanh[i] = 1 - (Math.Pow(Math.Tanh(network.hiddenLayer1[i]), 2));
      }
      return hiddenLayer1WithTanh;
    }

    public static double[,] NewInputLayerWeights(NeuralNetwork network, double learningRate, double[] errorWithHiddenLayer1, double[] hiddenLayer1WithTanh)
    {
      double[,] newInputLayerWeights = network.inputLayerWeights;
      //Iterates over each connection between the inputLayer and hiddenLayer1
      for(int hiddenLayer1Index = 0; hiddenLayer1Index < network.hiddenLayer1.Length; hiddenLayer1Index++)
      {
        for(int inputLayerIndex = 0; inputLayerIndex < network.inputLayer.Length; inputLayerIndex++)
        {
          newInputLayerWeights[hiddenLayer1Index, inputLayerIndex] -= learningRate * (errorWithHiddenLayer1[hiddenLayer1Index] * hiddenLayer1WithTanh[hiddenLayer1Index] * network.inputLayer[inputLayerIndex]);
        }
      }
      return newInputLayerWeights;
    }
  }
}
