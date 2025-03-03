using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MgrConsole : MgrBase<MgrConsole>
{
    public Dictionary<string, IConsole> console_dict = new Dictionary<string, IConsole>();

    public string[] command_line;

    public void WriteCommand(string command)
    {
        command_line = command.Split(' ');

        if(command_line.Length == 0)
            return;
        if(!console_dict.ContainsKey(command_line[0]))
            return;

        command_line = command_line.Skip(1).ToArray();

        console_dict[command_line[0]].HandleCommand(command_line);
    }
}