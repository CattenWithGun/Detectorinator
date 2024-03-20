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
        int imageIndex = random.Next(0, 9999);
        MNISTFileHandler.WriteImage(imageIndex, images, labels);
        Console.WriteLine($"The network thinks that the image is a {network.FeedForward(MNISTFileHandler.ImageToByteArray(images, imageIndex), network)}");
        Console.ReadLine();
      }
      return network;
    }
  }
}
