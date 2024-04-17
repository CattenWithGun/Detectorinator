using System.Collections.Generic;

namespace ErrorChecks
{
  public static class Checks
  {
    public static bool NetworkListContainsName(List<string> networkNames, string name)
    {
      for(int i = 0; i < networkNames.Count; i++)
      {
        if(networkNames[i] == name)
        {
          return true;
        }
      }
      return false;
    }

    public static bool IsOption(string option)
    {
      //Goes through the list of commands, and if the option is in there, return that it is a command
      string[] commands = { "show", "train", "test", "store", "make", "delete", "clear", "help" };
      for(int i = 0; i < commands.Length; i++)
      {
        if(commands[i] == option)
        {
          return true;
        }
      }
      return false;
    }
  }
}
