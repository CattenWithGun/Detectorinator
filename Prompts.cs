using ErrorChecks;
using System;
using System.Collections.Generic;

namespace UserPrompts
{
  public static class Prompts
  {
    public static string CommandPrompt()
    {
      Console.Write("\nEnter: ");
      string option = Console.ReadLine();
      while(!Checks.IsOption(option))
      {
        Console.Write("\nNot a command, enter again: ");
        option = Console.ReadLine();
      }
      return option;
    }

    public static string NetworkNameStorePrompt(List<string> networkNames)
    {
      Console.Write("Enter a network to store: ");
      string nameToStore = Console.ReadLine();
      if(nameToStore == "exit") { return "exit"; }
      while(!Checks.NetworkListContainsName(networkNames, nameToStore))
      {
        Console.WriteLine("Network wasn't in the saved networks list");
        Console.Write("Enter again: ");
        nameToStore = Console.ReadLine();
        if(nameToStore == "exit") { return "exit"; }
      }
      return nameToStore;
    }

    public static string NetworkNameDeletePrompt(List<string> networkNames)
    {
      Console.Write("Enter a network you want to delete: ");
      string nameToDelete = Console.ReadLine();
      if(nameToDelete == "exit") { return "exit"; }
      while(!Checks.NetworkListContainsName(networkNames, nameToDelete))
      {
        Console.WriteLine("Name wasn't in the saved networks list");
        Console.Write("Enter again: ");
        nameToDelete = Console.ReadLine();
        if(nameToDelete == "exit") { return "exit"; }
      }
      return nameToDelete;
    }

    public static string NetworkNamePrompt(List<string> networkNames)
    {
      Console.Write("Enter a name for the network: ");
      string name = Console.ReadLine();
      if(name == "exit") { return "exit"; }
      while(name.Length > 20 || name.Length <= 0 || Checks.NetworkListContainsName(networkNames, name))
      {
        Console.WriteLine("Name length needs to be 1-20 characters and not already used");
        Console.Write("Enter again: ");
        name = Console.ReadLine();
        if(name == "exit") { return "exit"; }
      }
      return name;
    }

    public static string SaveDataPrompt()
    {
      string useSaveDataString;
      do
      {
        Console.Write("Do you want to use save data? (y/n): ");
        useSaveDataString = Console.ReadLine();
        if(useSaveDataString == "exit") { return "exit"; }
      }
      while(useSaveDataString != "y" && useSaveDataString != "n");
      return useSaveDataString;
    }
  }
}
