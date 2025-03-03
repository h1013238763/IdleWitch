using System.Collections.Generic;
using UnityEngine;

public class Dialogue {
    public List<string> lines = new List<string>();
    public int current_line = 0;

    public string GetLine() {
        return lines[current_line];
    }

    public string GetNextLine() {
        current_line++;
        if(current_line >= lines.Count) {
            return null;
        }
        return lines[current_line];
    }

    public void LogAll(){
        foreach(string line in lines){
            Debug.Log(line);
        }
    }
}