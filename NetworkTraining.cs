using MNIST;
using NeuralNetworking;
using System;

namespace NetworkTraining
{
  public static class Training
  {
    public static NeuralNetwork TrainNetwork(NeuralNetwork network)
    {
      Console.WriteLine("Training network...");
      byte[] labels = MNISTFileHandler.GetLabels("/home/runner/NeuralNetwork/MNIST_Test_Database/labels");
      byte[,,] images = MNISTFileHandler.GetImages("/home/runner/NeuralNetwork/MNIST_Test_Database/images");
      Random random = new Random();
      while(true)
      {
        MNISTFileHandler.WriteImage(random.Next(0, 9999), images, labels);
        Console.ReadLine();
      }
      return network;
    }

    //Sigmoid function for squishing any value between 0 and 1
    private static double Sigmoid(double value)
    {
      double k = Math.Exp(value);
      return k / (1 + k);
    }
  }
}
