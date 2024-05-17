using Actions;
using NeuralNetworking;
using System;
using System.Collections.Generic;
using UserPrompts;

public class Program
{
  public static void Main()
  {
    //Creates a list to store networks and their names
    List<NeuralNetwork> networks = new List<NeuralNetwork>();
    List<string> networkNames = new List<string>();

    //This network is used as a placeholder, and gets overwritten often
    NeuralNetwork network = new NeuralNetwork("initialNetwork");

    Console.Clear();
    Commands.ShowCommands();
    while(true)
    {
      string command = Prompts.CommandPrompt();
      //Finds the option and does it
      if(command == "show")
      {
        Commands.ShowNetworks(networks);
      }
      else if(command == "train")
      {
        Commands.Train(networks, networkNames);
      }
      else if(command == "test")
      {
        Commands.Test(networks, networkNames);
      }
      else if(command == "error")
      {
        Commands.PrintError(networks, networkNames);
      }
      else if(command == "overfit")
      {
        Commands.Overfit(networks, networkNames);
      }
      else if(command == "store")
      {
        Commands.Store(networks, networkNames);
      }
      else if(command == "make")
      {
        Commands.Make(networks, networkNames);
      }
      else if(command == "delete")
      {
        Commands.Delete(networks, networkNames);
      }
      else if(command == "clear")
      {
        Console.Clear();
      }
      else if(command == "help")
      {
        Commands.ShowCommands();
      }
    }
  }
}
