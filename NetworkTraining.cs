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
      for(int i = 0; i < labels.Length; i++)
      {
        double[] outputLayer = network.FeedForward(network, MNISTFileHandler.ImageToByteArray(images, i));
        network = network.BackPropagate(network, MNISTFileHandler.LabelToExpectedValues(labels[i]));
      }
      return network;
    }
  }
}
