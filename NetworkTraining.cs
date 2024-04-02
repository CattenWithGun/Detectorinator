using MNIST;
using NeuralNetworking;
using System;
using System.Linq;

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
		double[] outputLayer = network.FeedForward(network, MNISTFileHandler.ImageToByteArray(images, imageIndex));
		//network = network.BackPropagate(network, MNISTFileHandler.LabelToExpectedValues(labels[imageIndex]));
        byte guess = Convert.ToByte(network.outputLayer.ToList().IndexOf(network.outputLayer.Max()));
		Console.WriteLine($"The network guessed {guess}");
        Console.ReadLine();
      }
      return network;
    }
  }
}
