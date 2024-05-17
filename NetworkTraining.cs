using Actions;
using MNIST;
using NeuralNetworking;
using System;
using System.Diagnostics;

namespace NetworkTraining
{
  public static class Training
  {
    public static NeuralNetwork TrainNetwork(NeuralNetwork network, double learningRate)
    {
      Console.WriteLine("Training network...");
      byte[] labels = MNISTFileHandler.GetLabels("/home/runner/NeuralNetwork/MNIST_Test_Database/labels");
      byte[,,] images = MNISTFileHandler.GetImages("/home/runner/NeuralNetwork/MNIST_Test_Database/images");
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      for(int j = 0; j < 100; j++)
      {
        for(int i = 0; i < labels.Length; i++)
        {
          double[] outputLayer = network.FeedForward(network, MNISTFileHandler.ImageToByteArray(images, i));
          network = network.BackPropagate(network, MNISTFileHandler.LabelToExpectedValues(labels[i]), learningRate);
        }
      }
      stopwatch.Stop();
      Console.WriteLine($"Training finished, took about {stopwatch.ElapsedMilliseconds / 1000} seconds");
      return network;
    }

    public static NeuralNetwork OverfitNetwork(NeuralNetwork network, double learningRate)
    {
      Console.WriteLine("Overfitting network...");
      byte[] labels = MNISTFileHandler.GetLabels("/home/runner/NeuralNetwork/MNIST_Test_Database/labels");
      byte[,,] images = MNISTFileHandler.GetImages("/home/runner/NeuralNetwork/MNIST_Test_Database/images");
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      for(int j = 0; j < 1000; j++)
      {
        double[] outputLayer = network.FeedForward(network, MNISTFileHandler.ImageToByteArray(images, 0));
        network = network.BackPropagate(network, MNISTFileHandler.LabelToExpectedValues(labels[0]), learningRate);
      }
      stopwatch.Stop();
      Console.WriteLine($"Overfitting finished, took about {stopwatch.ElapsedMilliseconds / 1000} seconds");
      Commands.PrintOverfittedError(network, labels, images);
      return network;
    }
  }
}
