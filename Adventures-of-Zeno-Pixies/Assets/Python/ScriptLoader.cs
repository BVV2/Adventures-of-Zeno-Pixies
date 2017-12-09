using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEditor;
using IronPython;

public class ScriptLoader {

	[MenuItem("Python/LoadFromFile")]
    public static void ReadFile()
    {
        var ScriptEngine = IronPython.Hosting.Python.CreateEngine();
        var ScriptScope = ScriptEngine.CreateScope();
        ScriptEngine.Runtime.LoadAssembly(typeof(GameObject).Assembly);
        ScriptEngine.Runtime.LoadAssembly(typeof(Editor).Assembly);
        StringBuilder example = new StringBuilder();
        FileInfo pySourceFile = new FileInfo("Assets/Python/test.py");
        StreamReader reader = pySourceFile.OpenText();
        while(!reader.EndOfStream)
        {
            example.AppendLine(reader.ReadLine());
          //  Debug.Log(reader.ReadLine());
        }

        reader.Close();
        var ScriptSource = ScriptEngine.CreateScriptSourceFromString(example.ToString());
        ScriptSource.Execute(ScriptScope);
    }
    public void Runner()
    {
        ReadFile();
    }
}
