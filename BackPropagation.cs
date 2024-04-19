using NeuralNetworking;
using System;

namespace BackPropagationHelper
{
  public static class BackPropagation
  {
    public static double[] ErrorWithRespectToOutputs(NeuralNetwork network, double[] expectedValues)
    {
      double[] errorWithOutputs = new double[network.outputLayer.Length];
      for(int i = 0; i < errorWithOutputs.Length; i++)
      {
        errorWithOutputs[i] = network.outputLayer[i] - expectedValues[i];
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
      for(int outputLayerIndex = 0; outputLayerIndex < network.outputLayer.Length; outputLayerIndex++)
      {
        for(int hiddenLayer2Index = 0; hiddenLayer2Index < network.hiddenLayer2.Length; hiddenLayer2Index++)
        {
          newHiddenLayer2Weights[outputLayerIndex, hiddenLayer2Index] -= learningRate * (errorWithOutputs[outputLayerIndex] * outputsWithTanh[outputLayerIndex] * network.hiddenLayer2[hiddenLayer2Index]);
        }
      }
      return newHiddenLayer2Weights;
    }
  }
}
