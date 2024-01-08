using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using DATA_CONVERTER;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp;
using Jace.Execution;
using Jace.Operations;
using Jace.Tokenizer;
using Jace.Util;
using Jace;
using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis.CSharp.Syntax;
// In order for us to do functions i am going to create a list of int and when we go into a function we add a new int wich is the line number and when we want to return we use the last line number, jump to that line, and then removed the last line number from the list.
namespace jumpE_basic
{
    class jumpE_basic
    {
        static void Main(string[] args)
        {
            DATA_CONVERTER.Data data = new DATA_CONVERTER.Data();
            bool run = true;
            bool clear_lock = false;
            double floatingvar =0;
            data.setI("LNT", 0);
            while (run)
            {
                data.setI("LN", 0);
                string hell = Console.ReadLine();
                if (hell == "end")
                {
                    break;
                }
                else if (hell == "clear lock")
                {
                    if (clear_lock)
                    {
                        clear_lock = false;
                        Console.WriteLine(clear_lock);
                    }
                    else
                    {
                        clear_lock = true;
                            Console.WriteLine(clear_lock);
                    }
                        
                }
                else if (hell == "clear")
                {
                    if (clear_lock == false) { 
                    DATA_CONVERTER.Data datas = new DATA_CONVERTER.Data();
                    data = datas;
                    Console.WriteLine("CLEAR");
                    }
                }
                else if(hell == "run")
                {
                    try
                    {
                        string hells = Console.ReadLine();
                        string fileName = @"" + hells;
                        using (StreamReader streamReader = File.OpenText(fileName))
                        {
                            string text = streamReader.ReadToEnd();
                            base_runner bases = new base_runner(text, data);
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else if(hell == "setD")
                {
                    Console.WriteLine("name of variable");
                    string varname = Console.ReadLine();
                    Console.WriteLine("variable value");
                    double varval = Double.Parse(Console.ReadLine());
                    data.setD(varname, varval);
                }
                else if(hell == "help")
                {
                    Console.WriteLine("_-----------_ \n setD \n setS \n run \n clear \n refD \n refS \n add \n save \n clear lock \n ref \n folder \n_-----------_ ");
                }
                else if (hell == "setS")
                {
                    Console.WriteLine("name of variable");
                    string varname = Console.ReadLine();
                    Console.WriteLine("variable value");
                    string varval = Console.ReadLine();
                    data.setS(varname, varval);
                }
                else if(hell == "refD")
                {
                    
                    Console.WriteLine("name of variable");
                    string varname = Console.ReadLine();
                    if (data.indouble(varname))
                    {
                        floatingvar = data.referenceD(varname);
                        Console.WriteLine(floatingvar);
                    }
                    else
                    {
                        Console.WriteLine("not a double");
                    }
                        
                }
                else if (hell == "refS")
                {
                    Console.WriteLine("name of variable");
                    string varname = Console.ReadLine();
                    if (data.instring(varname))
                    {
                        Console.WriteLine(data.referenceS(varname));
                    }
                    else
                    {
                        Console.WriteLine("not a string");
                    }
                }
                else if(hell == "ref")
                {
                    Console.WriteLine("name of variable");
                    string varname = Console.ReadLine();
                    if (data.isvar(varname))
                    {
                        Console.WriteLine(data.referenceVar(varname));
                    }
                    else
                    {
                        Console.WriteLine("not an initiallized variable");
                    }
                }
                else if(hell == "add")
                {
                    Console.WriteLine("name of variable");
                    string varname = Console.ReadLine();
                    double fla = data.referenceD(varname);
                    data.setD(varname, fla + floatingvar);
                }
                else if(hell == "folder")
                {
                    string folderPath = @"";
                    folderPath = Console.ReadLine();
                    if (Directory.Exists(folderPath))
                    {
                        string[] files = Directory.GetFiles(folderPath);
                        foreach (string filePath in files)
                        {
                            Console.WriteLine("File Name: " + Path.GetFileName(filePath));
                            Console.WriteLine("File Path: " + filePath);
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("The specified folder does not exist.");
                    }
                }
                else if(hell == "save")
                {
                    Console.WriteLine("name of file");
                    string filename = Console.ReadLine();
                    filename += ".txt";
                    data.SaveToFile(filename);
                }
                else
                {
                    Console.WriteLine("NOT A COMMAND");
                }
                
            }

        }
    }
    public class base_runner
    {
        public List<string> code = new List<string>();
        public List<string> lines = new List<string>();
        public string taken_in_string;
        public List<int> positions = new List<int>();
        SimpleTokenizer ourstuff = new SimpleTokenizer();
        CommandRegistry commandRegistry = new CommandRegistry();
        DATA_CONVERTER.Data data;
        DATA_CONVERTER.command_centralls interorouter = new DATA_CONVERTER.command_centralls();
        public int position;
        public bool run;
        public string data_storage = "@";

        public base_runner(string taken, DATA_CONVERTER.Data data)
        {
            this.taken_in_string = taken;
            this.lines = ourstuff.Linizer(this.taken_in_string);
            this.position = 0;
            this.run = true;
            this.data = data;
            data.setI("LNT", 0);

            while (this.run)
            {
                this.code = ourstuff.Tokenizer(this.lines[this.position]);
                data.setI("LNC", this.position);
                data.setI("LNT", data.referenceI("LNT")+1);
                if (commandRegistry.ContainsCommand(this.code[0]))
                {

                    interorouter = commandRegistry.GetCommandDefinition(this.code[0]);
                    if (interorouter is command_centrall)
                    {
                        ((command_centrall)(interorouter)).Execute(this.code, data, this);
                    }
                    if (interorouter is outer_commands)
                    {
                        ((outer_commands)(interorouter)).Execute(this.code, data);
                    }

                }
                else
                {
                    //Console.WriteLine("Unknown command: " + this.code[0]);
                }
                if (this.run == false)
                {
                    break;
                }
                //Debug.WriteLine(this.lines[this.position]);
                if (this.lines.Count >= this.position + 1)
                {
                    this.position++;
                }
                else
                {
                    this.position = 0;
                }



            }
        }

        public void changePosition(int a)
        {
            this.position = a;
        }
        public int get_position()
        {
            return this.position;
        }
        public void STOP()
        {
            this.run = false;
        }
        public class SimpleTokenizer
        {
            public List<string> Linizer(string input)
            {
                List<string> words = new List<string>();
                string[] lines = input.Split(new char[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    words.Add(line);
                }

                return words;
            }
            public List<string> Tokenizer(string input)
            {
                List<string> words = new List<string>();
                string[] tokens = input.Split(new char[] { ' ', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in tokens)
                {
                    words.Add(line);
                }

                return words;
            }
            public List<string> comandizer(string input)
            {
                List<string> words = new List<string>();
                string[] tokens = input.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in tokens)
                {
                    words.Add(line);
                }

                return words;
            }
        }
        public class CommandRegistry
        {
            Dictionary<string, DATA_CONVERTER.command_centralls> commands = new Dictionary<string, DATA_CONVERTER.command_centralls>();
            public CommandRegistry()
            {
                print print = new print();
                //whenD whend = new whenD();
                //whenS whens = new whenS();
                setS setS = new setS();
                use use = new use(this);
                //setD setD = new setD();
                //add add = new add();
                end end = new end();
                //subtract subtract = new subtract();
                //multiply multiply = new multiply();
                //divide divide = new divide();
                jump jump = new jump();
                inputD inputD = new inputD();
                comment comment = new comment();
                inputS inputS = new inputS();
                inputI inputI = new inputI();
                useC useC = new useC(this);
                line_number line_number = new line_number();
                pre_defined_variable Math_equation = new pre_defined_variable();
                double_func double_func = new double_func(Math_equation,this);
                string_func string_Func = new string_func(Math_equation, this);
                int_func int_func = new int_func(Math_equation,this);
                when when = new when(Math_equation, this);
                return_func return_Func = new return_func();
                commands.Add("return", return_Func); commands.Add("Return", return_Func); commands.Add("RETURN", return_Func); commands.Add("<<", return_Func);
                commands.Add("when", when); commands.Add("When", when);commands.Add("if", when);
                commands.Add("useC", useC); commands.Add("usec", useC);
                commands.Add("print", print); commands.Add("Print", print);
                commands.Add("inputI", inputI); commands.Add("inputi", inputI);commands.Add("InputI", inputI);
                //commands.Add("whenD", whend); commands.Add("WhenD", whend);
                commands.Add("inputS", inputS);commands.Add("inputs", inputS);commands.Add("InputS", inputS);
                //commands.Add("setS", setS); commands.Add("SetS", setS);
                commands.Add("string", string_Func);commands.Add("String", string_Func);commands.Add("STRING", string_Func);
                commands.Add("int", int_func); commands.Add("INT", int_func);
                //commands.Add("whenS", whens); commands.Add("WhenS", whens);
                commands.Add("jump", jump); commands.Add("jp", jump); commands.Add("JP", jump); commands.Add("JUMP", jump);
                commands.Add("double", double_func); commands.Add("DOUBLE", double_func); commands.Add("Double", double_func);
                /*commands.Add("subtract", subtract); commands.Add("sub", subtract);
                commands.Add("multiply", multiply); commands.Add("mult", multiply);
                commands.Add("divide", divide); commands.Add("div", divide);*/
                commands.Add("end", end); commands.Add("stop", end); commands.Add("END", end);
                commands.Add("inputD", inputD); commands.Add("inputd", inputD); commands.Add("InputD", inputD);
                commands.Add("use", use);
                commands.Add("line_number", line_number); commands.Add("ln", line_number); commands.Add("LN", line_number);
                commands.Add("comment", comment); commands.Add("//", comment); commands.Add("#", comment);
            }
            public void add_command(string name, command_centralls type)
            {
                commands.Add(name, type);
            }
            public bool ContainsCommand(string command)
            {
                if (commands.ContainsKey(command))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public command_centralls GetCommandDefinition(string commandName)
            {
                if (ContainsCommand(commandName))
                {
                    return commands[commandName];
                }
                else
                {
                    return null;
                }
            }
        }
        public class command_centrall : command_centralls
        {
            public virtual void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                Debug.WriteLine("eh");
            }
        }
        public class comment : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
            }
        }
        public class print : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    //int color = 15;
                    //string Message = "";
                    
                    for (int i = 1; i < code.Count; i++)
                    {
                        if (code[i] == "\\Clear ")
                        {
                            Console.Clear();
                        }
                        else if (code[i] == "\\n")
                        {
                            Console.WriteLine();
                        }
                        /*else if (code[i] == "\\Color")
                        {
                            Console.Writeint.Parse(code[i + 1]));
                            
                            i++;
                        }*/
                        else if (code[i] == "\""&&code[i+2] == "\"")
                        {
                            Console.Write(D.referenceVar(code[i + 1]) + " ");

                            i += 2;
                        }
                        else
                        {
                            Console.Write(code[i] + " ");
                        }

                    }
                    Console.WriteLine();
                }
                catch(Exception e){
                    Console.WriteLine(e + " Line: " + Base.get_position());
                }
                


            }
        }
        public class when : command_centrall
        {
            pre_defined_variable Math_equation;
            CommandRegistry commands;
            IDictionary<string, double> drict = new Dictionary<string, double>();
            public when(pre_defined_variable j, CommandRegistry c)
            {
                this.Math_equation = j;
                this.commands = c;

            }
            public override void Execute(List<string> code, Data D, base_runner Base)
            {

                try
                {
                    string equation = "";
                    /*if (code.Count() == 2)
                    {
                        D.setI(code[1], 0);
                        this.commands.add_command(code[1], this.Math_equation);

                    }*/ // this will be for booleans when i get around to 
                    //else { }
                        for (int i = 1; i < code.Count(); i++)
                        {
                            double j;
                            if (Double.TryParse(code[i], out j))
                            {
                                equation += j + " ";
                            }
                            else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                            code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " "||code[i]== "=="||code[i]=="!="||code[i]==">"|| code[i] == "<"||
                            code[i] == "=>"|| code[i] == "=<"|| code[i] == "!")
                            {
                                equation += code[i] + " ";
                            }
                            else if (D.isnumvar(code[i]))
                            {
                                equation += D.referenceVar(code[i]) + " ";
                            }
                        else
                        {
                            equation += code[i] + " ";
                            Debug.WriteLine("not recognized when statement");
                        }
                        }
                        CalculationEngine engine = new CalculationEngine();
                    bool result = Convert.ToBoolean(engine.Calculate(equation));
                    if (!result)
                    {
                        int w = 0;
                        int q = Base.position+1;
                        while (true)
                        {


                            if (Base.lines[q] == "{")
                            {
                                w++;
                            }
                            if (Base.lines[q] == "}")
                            {
                                if (w == 1)
                                {
                                    Base.changePosition(q);
                                    break;
                                }
                                else
                                {
                                    w--;
                                }
                            }
                            q++;
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Initialization error " + e);
                }
            }
        }
        /*public class whenD : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    if (code[1] == "ReFd")
                    {
                        if (code[4] == "ReFd")
                        {

                            if (code[3] == "=")
                            {
                                if (D.referenceD(code[2]) == D.referenceD(code[5]))
                                {

                                }
                                else
                                {
                                    if (code[6] == "ReFd")
                                    {
                                        Base.changePosition((int)D.referenceD(code[7]));
                                    }
                                    else
                                    {
                                        Base.changePosition((int)D.referenceD(code[6]));
                                    }
                                }
                            }
                            else if (code[3] == "<")
                            {
                                if (D.referenceD(code[2]) < D.referenceD(code[5]))
                                {

                                }
                                else
                                {
                                    if (code[6] == "ReFd")
                                    {
                                        Base.changePosition((int)D.referenceD(code[7]));
                                    }
                                    else
                                    {
                                        Base.changePosition((int)D.referenceD(code[6]));
                                    }
                                }
                            }
                            else if (code[3] == ">")
                            {
                                if (D.referenceD(code[2]) > D.referenceD(code[5]))
                                {

                                }
                                else
                                {
                                    if (code[6] == "ReFd")
                                    {
                                        Base.changePosition((int)D.referenceD(code[7]));
                                    }
                                    else
                                    {
                                        Base.changePosition((int)D.referenceD(code[6]));
                                    }
                                }
                            }
                            else if (code[3] == "<=")
                            {
                                if (D.referenceD(code[2]) <= D.referenceD(code[5]))
                                {

                                }
                                else
                                {
                                    if (code[6] == "ReFd")
                                    {
                                        Base.changePosition((int)D.referenceD(code[7]));
                                    }
                                    else
                                    {
                                        Base.changePosition((int)D.referenceD(code[6]));
                                    }
                                }
                            }
                            else if (code[3] == ">=")
                            {
                                if (D.referenceD(code[2]) >= D.referenceD(code[5]))
                                {

                                }
                                else
                                {
                                    if (code[6] == "ReFd")
                                    {
                                        Base.changePosition((int)D.referenceD(code[7]));
                                    }
                                    else
                                    {
                                        Base.changePosition((int)D.referenceD(code[6]));
                                    }
                                }
                            }
                        }
                        else
                        {

                            if (code[3] == "=")
                            {
                                if (D.referenceD(code[2]) == Double.Parse(code[4]))
                                {
                                    Debug.WriteLine("weird");
                                }
                                else
                                {
                                    if (code[5] == "ReFd")
                                    {
                                        Base.changePosition((int)D.referenceD(code[6]));
                                    }
                                    else
                                    {
                                        Base.changePosition((int)D.referenceD(code[5]));
                                    }
                                }
                            }
                            else if (code[3] == "<")
                            {
                                if (D.referenceD(code[2]) < Double.Parse(code[4]))
                                {

                                }
                                else
                                {
                                    if (code[5] == "ReFd")
                                    {
                                        Base.changePosition((int)D.referenceD(code[6]));
                                    }
                                    else
                                    {
                                        Base.changePosition((int)D.referenceD(code[5]));
                                    }
                                }
                            }
                            else if (code[3] == ">")
                            {
                                if (D.referenceD(code[2]) > Double.Parse(code[4]))
                                {

                                }
                                else
                                {
                                    if (code[5] == "ReFd")
                                    {
                                        Base.changePosition((int)D.referenceD(code[6]));
                                    }
                                    else
                                    {
                                        Base.changePosition((int)D.referenceD(code[5]));
                                    }
                                }
                            }
                            else if (code[3] == "<=")
                            {
                                if (D.referenceD(code[2]) <= Double.Parse(code[4]))
                                {

                                }
                                else
                                {
                                    if (code[5] == "ReFd")
                                    {
                                        Base.changePosition((int)D.referenceD(code[6]));
                                    }
                                    else
                                    {
                                        Base.changePosition((int)D.referenceD(code[5]));
                                    }
                                }
                            }
                            else if (code[3] == ">=")
                            {
                                if (D.referenceD(code[2]) >= Double.Parse(code[4]))
                                {

                                }
                                else
                                {
                                    if (code[5] == "ReFd")
                                    {
                                        Base.changePosition((int)D.referenceD(code[6]));
                                    }
                                    else
                                    {
                                        Base.changePosition((int)D.referenceD(code[5]));
                                    }
                                }
                            }
                        }
                    }
                    else if (code[3] == "ReFd")
                    {
                        if (code[2] == "=")
                        {
                            if (Double.Parse(code[1]) == D.referenceD(code[4]))
                            {

                            }
                            else
                            {
                                if (code[5] == "ReFd")
                                {
                                    Base.changePosition((int)D.referenceD(code[6]));
                                }
                                else
                                {
                                    Base.changePosition((int)D.referenceD(code[5]));
                                }
                            }
                        }
                        if (code[2] == "<")
                        {
                            if (Double.Parse(code[1]) < D.referenceD(code[4]))
                            {

                            }
                            else
                            {
                                if (code[5] == "ReFd")
                                {
                                    Base.changePosition((int)D.referenceD(code[6]));
                                }
                                else
                                {
                                    Base.changePosition((int)D.referenceD(code[5]));
                                }
                            }
                        }
                        if (code[2] == ">")
                        {
                            if (Double.Parse(code[1]) > D.referenceD(code[4]))
                            {

                            }
                            else
                            {
                                if (code[5] == "ReFd")
                                {
                                    Base.changePosition((int)D.referenceD(code[6]));
                                }
                                else
                                {
                                    Base.changePosition((int)D.referenceD(code[5]));
                                }
                            }
                        }
                        if (code[2] == ">=")
                        {
                            if (Double.Parse(code[1]) >= D.referenceD(code[4]))
                            {

                            }
                            else
                            {
                                if (code[5] == "ReFd")
                                {
                                    Base.changePosition((int)D.referenceD(code[6]));
                                }
                                else
                                {
                                    Base.changePosition((int)D.referenceD(code[5]));
                                }
                            }
                        }
                        if (code[2] == "<=")
                        {
                            if (Double.Parse(code[1]) <= D.referenceD(code[4]))
                            {

                            }
                            else
                            {
                                if (code[5] == "ReFd")
                                {
                                    Base.changePosition((int)D.referenceD(code[6]));
                                }
                                else
                                {
                                    Base.changePosition((int)D.referenceD(code[5]));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (code[2] == "=")
                        {
                            if (Double.Parse(code[1]) == Double.Parse(code[3]))
                            {

                            }
                            else
                            {
                                if (code[4] == "ReFd")
                                {
                                    Base.changePosition((int)D.referenceD(code[5]));
                                }
                                else
                                {
                                    Base.changePosition((int)D.referenceD(code[4]));
                                }
                            }
                        }
                        if (code[2] == "<")
                        {
                            if (Double.Parse(code[1]) < Double.Parse(code[3]))
                            {

                            }
                            else
                            {
                                if (code[4] == "ReFd")
                                {
                                    Base.changePosition((int)D.referenceD(code[5]));
                                }
                                else
                                {
                                    Base.changePosition((int)D.referenceD(code[4]));
                                }
                            }
                        }
                        if (code[2] == ">")
                        {
                            if (Double.Parse(code[1]) > Double.Parse(code[3]))
                            {

                            }
                            else
                            {
                                if (code[4] == "ReFd")
                                {
                                    Base.changePosition((int)D.referenceD(code[5]));
                                }
                                else
                                {
                                    Base.changePosition((int)D.referenceD(code[4]));
                                }
                            }
                        }
                        if (code[2] == ">=")
                        {
                            if (Double.Parse(code[1]) >= Double.Parse(code[3]))
                            {

                            }
                            else
                            {
                                if (code[4] == "ReFd")
                                {
                                    Base.changePosition((int)D.referenceD(code[5]));
                                }
                                else
                                {
                                    Base.changePosition((int)D.referenceD(code[4]));
                                }
                            }
                        }
                        if (code[2] == "<=")
                        {
                            if (Double.Parse(code[1]) <= Double.Parse(code[3]))
                            {

                            }
                            else
                            {
                                if (code[4] == "ReFd")
                                {
                                    Base.changePosition((int)D.referenceD(code[5]));
                                }
                                else
                                {
                                    Base.changePosition((int)D.referenceD(code[4]));
                                }
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e + " Line: " + Base.get_position());
                }
                

            }
        }
        public class whenS : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    if (code[2] == "=")
                    {
                        if (code[1] == "ReFd")
                        {
                            if (code[4] == "ReFd")
                            {
                                if (D.referenceS(code[3]) == D.referenceS(code[5]))
                                {

                                }
                                else
                                {
                                    int x = Base.get_position();
                                    if (code[6] == "ReFd")
                                    {
                                        x += (int)D.referenceD(code[7]);
                                        Base.changePosition(x);
                                    }
                                    else
                                    {
                                        x += (int)Double.Parse(code[6]);
                                        Base.changePosition(x);
                                    }
                                }
                            }
                            else
                            {
                                if (D.referenceS(code[2]) == (code[4]))
                                {

                                }
                                else
                                {
                                    int x = Base.get_position();
                                    if (code[5] == "ReFd")
                                    {
                                        x += (int)D.referenceD(code[6]);
                                        Base.changePosition(x);
                                    }
                                    else
                                    {
                                        x += (int)Double.Parse(code[5]);
                                        Base.changePosition(x);
                                    }
                                }
                            }
                        }
                        else if (code[3] == "ReFd")
                        {
                            if ((code[2]) == D.referenceS(code[4]))
                            {

                            }
                            else
                            {
                                int x = Base.get_position();
                                if (code[5] == "ReFd")
                                {
                                    x += (int)D.referenceD(code[6]);
                                    Base.changePosition(x);
                                }
                                else
                                {
                                    x += (int)Double.Parse(code[5]);
                                    Base.changePosition(x);
                                }
                            }
                        }
                        else
                        {
                            if (code[1] == code[3])
                            {

                            }
                            else
                            {
                                int x = Base.get_position();
                                if (code[4] == "ReFd")
                                {
                                    x += (int)D.referenceD(code[5]);
                                    Base.changePosition(x);
                                }
                                else
                                {
                                    x += (int)Double.Parse(code[4]);
                                    Base.changePosition(x);
                                }
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e + " Line: " + Base.get_position());
                }
                
            }
        }*/

        public class useC : command_centrall
        {
            private CommandRegistry commandRegistry;

            public useC(CommandRegistry a)
            {
                this.commandRegistry = a;
            }

            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (code.Count >= 3)
                {
                    string filePath = code[1];

                    // Read the contents of the .cs file
                    string fileContent = File.ReadAllText(filePath);

                    // Dynamic code generation with namespace
                    string generatedCode = $@"
using DATA_CONVERTER;
using System;
using Jace.Execution;
using Jace.Operations;
using Jace.Tokenizer;
using Jace.Util;
using Jace;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Collections.Generic;
namespace Imported_commands 
{{ 
            {fileContent}
}}";
                    //Debug.WriteLine(generatedCode);
                    try
                    {
                        // Compile the code using CodeDom
                        CompilerResults compilerResults = CompileCode(generatedCode);

                        if (compilerResults.Errors.HasErrors)
                        {

                            foreach (CompilerError error in compilerResults.Errors)
                            {
                                Debug.WriteLine(error.ErrorText);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to initialize imports: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid number of parameters for the 'use' command.");
                }
            }

            private CompilerResults CompileCode(string code)
            {
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                CompilerParameters compilerParameters = new CompilerParameters
                {
                    GenerateInMemory = true,
                    GenerateExecutable = false
                };
                string assemblyNameD = "DATA_CONVERTER";

                // Get the assembly by name
                Assembly assemblyD = Assembly.Load(assemblyNameD);

                // Get the location (path) of the assembly
                string assemblyPathD = assemblyD.Location;




                compilerParameters.ReferencedAssemblies.Add(assemblyPathD);
                compilerParameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\PresentationFramework.dll");
                return codeProvider.CompileAssemblyFromSource(compilerParameters, code);

            }
        }


        public class use : command_centrall
    {
        private CommandRegistry commandRegistry;

        public use(CommandRegistry a)
        {
            this.commandRegistry = a;
        }

        public override void Execute(List<string> code, Data D, base_runner Base)
        {
            if (code.Count >= 3)
            {
                string filePath = code[1];
                string className = code[2];

                // Read the contents of the .cs file
                string fileContent = File.ReadAllText(filePath);

                    // Dynamic code generation with namespace
                    string generatedCode = $@"
using DATA_CONVERTER;
using System;
using System.Linq;
using System.Text;
using System.IO;
using Jace.Execution;
using Jace.Operations;
using Jace.Tokenizer;
using Jace.Util;
using Jace;
using System.Windows;
using System.Collections.Generic;
namespace Imported_commands 
{{ 
    public class {className} : DATA_CONVERTER.outer_commands 
    {{ 
        public override void Execute(List<string> code, Data D)
        {{
            {fileContent}
        }}
    }} 
}}";
                    //Debug.WriteLine(generatedCode);
                try
                {
                    // Compile the code using CodeDom
                    CompilerResults compilerResults = CompileCode(generatedCode);

                    if (compilerResults.Errors.HasErrors)
                    {
                            
                            foreach (CompilerError error in compilerResults.Errors)
                        {
                            Debug.WriteLine(error.ErrorText);
                            }
                    }
                    else
                    {
                        // Get the compiled type
                        Type compiledType = compilerResults.CompiledAssembly.GetType($"Imported_commands.{className}");

                        // Create an instance of the compiled class
                        object initializedObject = Activator.CreateInstance(compiledType);

                        // Check if the result is an instance of DATA_CONVERTER.outer_commands
                        if (initializedObject is DATA_CONVERTER.outer_commands outerCommandsObject)
                        {
                            // Add the command to the commandRegistry
                            this.commandRegistry.add_command(className, outerCommandsObject);
                                Debug.WriteLine(className);
                                
                        }
                        else
                        {
                            Console.WriteLine($"Failed to create an instance of {className}.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create an instance of {className}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid number of parameters for the 'use' command.");
            }
        }

        private CompilerResults CompileCode(string code)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CompilerParameters compilerParameters = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false
            };
                string assemblyNameD = "DATA_CONVERTER";

                // Get the assembly by name
                Assembly assemblyD = Assembly.Load(assemblyNameD);

                // Get the location (path) of the assembly
                string assemblyPathD = assemblyD.Location;




                compilerParameters.ReferencedAssemblies.Add(assemblyPathD);
                compilerParameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\PresentationFramework.dll");
                return codeProvider.CompileAssemblyFromSource(compilerParameters, code);

        }
    }

        public class inputD : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                if (D.indouble(code[1]))
                {
                    bool inputSuccess = false;

                    do
                    {
                        try
                        {
                            Console.Write("Enter a double value: ");
                            string rans = Console.ReadLine();

                            if (double.TryParse(rans, out double ran))
                            {
                                D.setD(code[1], ran);
                                inputSuccess = true;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid double.");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e + " Line: " + Base.get_position());
                        }
                    } while (!inputSuccess);
                }
            }
        }

        public class inputI : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                if (D.inint(code[1]))
                {
                    bool inputSuccess = false;

                    do
                    {
                        try
                        {
                            Console.Write("Enter an integer value: ");
                            string rans = Console.ReadLine();

                            if (int.TryParse(rans, out int ran))
                            {
                                D.setI(code[1], ran);
                                inputSuccess = true;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid integer.");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e + " Line: " + Base.get_position());
                        }
                    } while (!inputSuccess);
                }
            }
        }

        public class inputS : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                if (D.instring(code[1]))
                {
                    bool inputSuccess = false;

                    do
                    {
                        try
                        {
                            Console.Write("Enter a string: ");
                            string rans = Console.ReadLine();
                                D.setS(code[1], rans);
                                inputSuccess = true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e + " Line: " + Base.get_position());
                        }
                    } while (!inputSuccess);
                }
            }
        }

        public class setS : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    string Message = "";

                    for (int i = 1; i < code.Count; i++)
                    {
                        if (code[i] == "ReFs")
                        {
                            string Messagee = D.referenceS(code[i + 1]);
                            Message += Messagee + " ";
                            i += 1;
                        }
                        else if (code[i] == "ReFd")
                        {
                            double Messagee = D.referenceD(code[i + 1]);
                            Message += Messagee + " ";
                            i += 1;
                        }
                        else
                        {
                            Message += code[i] + " ";
                        }

                    }
                    D.setS(code[1], Message);
                }
                catch
                {
                    Console.WriteLine("Setting error: line " + Base.get_position());
                }
                
            }
        }
        /*public class setD : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    if(code[2] == "ReFd")
                    {
                        D.setD(code[1], D.referenceD(code[3]));
                    }
                    else
                    {
                        D.setD(code[1], Double.Parse(code[2]));
                    }
                }
                catch
                {
                    Console.WriteLine("Setting error: line " + Base.get_position());
                }
                
            }
        }
        public class setI : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    if (code[2] == "ReFd")
                    {
                        D.setI(code[1], D.referenceI(code[3]));
                    }
                    else
                    {
                        D.setI(code[1], int.Parse(code[2]));
                    }
                }
                catch
                {
                    Console.WriteLine("Setting error: line " + Base.get_position());
                }

            }
        }*/
        public class int_func : command_centrall
        {
            pre_defined_variable Math_equation;
            CommandRegistry commands;
            IDictionary<string, double> drict = new Dictionary<string, double>();
            public int_func(pre_defined_variable j, CommandRegistry c)
            {
                this.Math_equation = j;
                this.commands = c;

            }
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                
                try
                {
                    string equation = "";
                    if (code.Count() == 2)
                    {
                        D.setI(code[1], 0);
                        this.commands.add_command(code[1], this.Math_equation);

                    }
                    else if (code[2] == "=")
                    {
                        for (int i = 3; i < code.Count(); i++)
                        {
                            double j;
                            if (Double.TryParse(code[i],out j))
                            {
                                equation += j + " ";
                            }
                            else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                            code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                            {
                                equation += code[i] + " ";
                            }
                            else if (D.isnumvar(code[i]))
                            {
                                equation += D.referenceVar(code[i]) + " ";
                            }
                        }
                        CalculationEngine engine = new CalculationEngine();
                        D.setI(code[1], (int)(engine.Calculate(equation, drict)));
                        this.commands.add_command(code[1], this.Math_equation);

                    }
                    
                }
                catch(Exception e)
                {
                    Console.WriteLine("Initialization error " + e);
                }
            }
        }
        public class pre_defined_variable : command_centrall
        {
            IDictionary<string, double> drict = new Dictionary<string, double>();
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (D.inint(code[0]))
                {
                    try
                    {
                        string equation = "";
                        if (code[1] == "=")
                        {
                            for (int i = 2; i < code.Count(); i++)
                            {
                                double j;
                                if (Double.TryParse(code[i], out j))
                                {
                                    equation += j + " ";
                                }
                                else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                                {
                                    equation += code[i] + " ";
                                }
                                else if (D.isnumvar(code[i]))
                                {
                                    equation += D.referenceVar(code[i]) + " ";
                                }
                            }
                            CalculationEngine engine = new CalculationEngine();
                            D.setI(code[0], (int)(engine.Calculate(equation, drict)));

                        }
                        else if (code[1] == "+=")
                        {
                            for (int i = 2; i < code.Count(); i++)
                            {
                                double j;
                                if (Double.TryParse(code[i], out j))
                                {
                                    equation += j + " ";
                                }
                                else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                                {
                                    equation += code[i] + " ";
                                }
                                else if (D.isnumvar(code[i]))
                                {
                                    equation += D.referenceVar(code[i]) + " ";
                                }
                            }
                            CalculationEngine engine = new CalculationEngine();
                            D.setI(code[0], D.referenceI(code[0])+(int)(engine.Calculate(equation, drict)));

                        }
                        else if (code[1] == "-=")
                        {
                            for (int i = 2; i < code.Count(); i++)
                            {
                                double j;
                                if (Double.TryParse(code[i], out j))
                                {
                                    equation += j + " ";
                                }
                                else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                                {
                                    equation += code[i] + " ";
                                }
                                else if (D.isnumvar(code[i]))
                                {
                                    equation += D.referenceVar(code[i]) + " ";
                                }
                            }
                            CalculationEngine engine = new CalculationEngine();
                            D.setI(code[0], D.referenceI(code[0]) - (int)(engine.Calculate(equation, drict)));

                        }
                        else if (code[1] == "*=")
                        {
                            for (int i = 2; i < code.Count(); i++)
                            {
                                double j;
                                if (Double.TryParse(code[i], out j))
                                {
                                    equation += j + " ";
                                }
                                else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                                {
                                    equation += code[i] + " ";
                                }
                                else if (D.isnumvar(code[i]))
                                {
                                    equation += D.referenceVar(code[i]) + " ";
                                }
                            }
                            CalculationEngine engine = new CalculationEngine();
                            D.setI(code[0], D.referenceI(code[0]) * (int)(engine.Calculate(equation, drict)));

                        }
                        else if (code[1] == "/=")
                        {
                            for (int i = 2; i < code.Count(); i++)
                            {
                                double j;
                                if (Double.TryParse(code[i], out j))
                                {
                                    equation += j + " ";
                                }
                                else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                                {
                                    equation += code[i] + " ";
                                }
                                else if (D.isnumvar(code[i]))
                                {
                                    equation += D.referenceVar(code[i]) + " ";
                                }
                            }
                            CalculationEngine engine = new CalculationEngine();
                            D.setI(code[0], D.referenceI(code[0]) / (int)(engine.Calculate(equation, drict)));

                        }
                        else if(code[1] == "++")
                        {
                            D.setI(code[0], D.referenceI(code[0]) + 1);
                        }
                        else if (code[1] == "--")
                        {
                            D.setI(code[0], D.referenceI(code[0]) - 1);
                        }
                        else if (code[1] == "**")
                        {
                            D.setI(code[0], D.referenceI(code[0]) * D.referenceI(code[0]));
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Initialization error");
                    }
                }
                if (D.indouble(code[0]))
                {

                    {
                        try
                        {
                            string equation = "";
                            if (code[1] == "=")
                            {
                                for (int i = 2; i < code.Count(); i++)
                                {
                                    double j;
                                    if (Double.TryParse(code[i], out j))
                                    {
                                        equation += j + " ";
                                    }
                                    else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                    code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                                    {
                                        equation += code[i] + " ";
                                    }
                                    else if (D.isnumvar(code[i]))
                                    {
                                        equation += D.referenceVar(code[i]) + " ";
                                    }
                                }
                                CalculationEngine engine = new CalculationEngine();
                                D.setD(code[0], (engine.Calculate(equation, null)));

                            }
                            else if (code[1] == "+=")
                            {
                                for (int i = 2; i < code.Count(); i++)
                                {
                                    double j;
                                    if (Double.TryParse(code[i], out j))
                                    {
                                        equation += j + " ";
                                    }
                                    else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                    code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                                    {
                                        equation += code[i] + " ";
                                    }
                                    else if (D.isnumvar(code[i]))
                                    {
                                        equation += D.referenceVar(code[i]) + " ";
                                    }
                                }
                                CalculationEngine engine = new CalculationEngine();
                                D.setD(code[0], D.referenceD(code[0]) + (engine.Calculate(equation, drict)));

                            }
                            else if (code[1] == "-=")
                            {
                                for (int i = 2; i < code.Count(); i++)
                                {
                                    double j;
                                    if (Double.TryParse(code[i], out j))
                                    {
                                        equation += j + " ";
                                    }
                                    else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                    code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                                    {
                                        equation += code[i] + " ";
                                    }
                                    else if (D.isnumvar(code[i]))
                                    {
                                        equation += D.referenceVar(code[i]) + " ";
                                    }
                                }
                                CalculationEngine engine = new CalculationEngine();
                                D.setD(code[0], D.referenceD(code[0]) - (engine.Calculate(equation, drict)));

                            }
                            else if (code[1] == "*=")
                            {
                                for (int i = 2; i < code.Count(); i++)
                                {
                                    double j;
                                    if (Double.TryParse(code[i], out j))
                                    {
                                        equation += j + " ";
                                    }
                                    else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                    code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                                    {
                                        equation += code[i] + " ";
                                    }
                                    else if (D.isnumvar(code[i]))
                                    {
                                        equation += D.referenceVar(code[i]) + " ";
                                    }
                                }
                                CalculationEngine engine = new CalculationEngine();
                                D.setD(code[0], D.referenceD(code[0]) * (engine.Calculate(equation, drict)));

                            }
                            else if (code[1] == "/=")
                            {
                                for (int i = 2; i < code.Count(); i++)
                                {
                                    double j;
                                    if (Double.TryParse(code[i], out j))
                                    {
                                        equation += j + " ";
                                    }
                                    else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                    code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                                    {
                                        equation += code[i] + " ";
                                    }
                                    else if (D.isnumvar(code[i]))
                                    {
                                        equation += D.referenceVar(code[i]) + " ";
                                    }
                                }
                                CalculationEngine engine = new CalculationEngine();
                                D.setD(code[0], D.referenceD(code[0]) / (engine.Calculate(equation, drict)));

                            }
                            else if (code[1] == "++")
                            {
                                D.setD(code[0], D.referenceD(code[0]) + 1);
                            }
                            else if (code[1] == "--")
                            {
                                D.setD(code[0], D.referenceD(code[0]) - 1);
                            }
                            else if (code[1] == "**")
                            {
                                D.setD(code[0], D.referenceD(code[0]) * D.referenceD(code[0]));
                            }
                            else if (code[1] == "/^")
                            {
                                D.setD(code[0], Math.Sqrt(D.referenceD(code[0])));
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Initialization error");
                        }
                    }
                }
                if (D.instring(code[0]))
                {
                    string mesage = D.referenceS(code[1]);
                    if(code[1] == "=")
                    {
                        mesage = "";
                        for (int i = 2; i < code.Count(); i++)
                        {
                            if (code[i] == "\"" && code[i + 2] == "\"")
                            {
                                mesage += D.referenceVar(code[i + 1]) + " ";
                                i += 2;
                            }
                            else
                            {
                                mesage += code[i] + " ";
                                i++;
                            }
                        }
                    }
                    else if (code[1] == "+=")
                    {
                        for (int i = 2; i < code.Count(); i++)
                        {
                            if (code[i] == "\"" && code[i + 2] == "\"")
                            {
                                mesage += D.referenceVar(code[i + 1]) + " ";
                                i += 2;
                            }
                            else
                            {
                                mesage += code[i] + " ";
                                i++;
                            }
                        }
                    }
                    D.setS(code[0], mesage);
                }

            }
        }
        public class string_func : command_centrall
        {
            pre_defined_variable varlee;
            CommandRegistry commands;
            public string_func(pre_defined_variable j, CommandRegistry c)
            {
                this.varlee = j;
                this.commands = c;
            }
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                try
                {
                    string mesage = "";
                    if (code.Count() == 2)
                    {
                        D.setS(code[1], mesage);
                        this.commands.add_command(code[1], this.varlee);
                    }
                    else if (code[1] == "=")
                    {
                        mesage = "";
                        for (int i = 2; i < code.Count(); i++)
                        {
                            if (code[i] == "\"" && code[i + 2] == "\"")
                            {
                                mesage += D.referenceVar(code[i + 1]) + " ";
                                i += 2;
                            }
                            else
                            {
                                mesage += code[i] + " ";
                                i++;
                            }
                        }
                        D.setS(code[1], mesage);
                        this.commands.add_command(code[1], this.varlee);

                    }
                }
                catch
                {
                    Console.WriteLine("Initialization error");
                }
            }
        }
        public class double_func : command_centrall
        {
            pre_defined_variable Math_equation;
            CommandRegistry commands;
            IDictionary<string, double> drict = new Dictionary<string, double>();
            public double_func(pre_defined_variable j, CommandRegistry c)
            {
                this.Math_equation = j;
                this.commands = c;
            }
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                try
                {
                    string equation = "";
                    if (code.Count() == 2)
                    {
                        D.setD(code[1], 0);
                        this.commands.add_command(code[1], this.Math_equation);
                    }
                    else if (code[2] == "=")
                    {
                        for (int i = 3; i < code.Count(); i++)
                        {
                            double j;
                            if (Double.TryParse(code[i], out j))
                            {
                                equation += j + " ";
                            }
                            else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                            code[i] == "csc" || code[i] == "sec" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                            {
                                equation += code[i] + " ";
                            }
                            else if (D.isnumvar(code[i]))
                            {
                                equation += D.referenceVar(code[i]) + " ";
                            }
                        }
                        CalculationEngine engine = new CalculationEngine();
                        D.setD(code[1], (engine.Calculate(equation, drict)));
                        this.commands.add_command(code[1], this.Math_equation);
                    }
                    
                }
                catch
                {
                    Console.WriteLine("Initialization error");
                }
            }
        }
        public class end : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                Base.STOP();
            }
        }
        /*public class with : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                base.Execute(code, D, Base);
            }
        }*/
        public class return_func : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                Base.changePosition(Base.positions[Base.positions.Count-1]);
                Base.positions.RemoveAt(Base.positions.Count - 1);
            }
        }
        public class jump : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    int a;
                    if (D.inint(code[1]))
                    {
                        a = (D.referenceI(code[1]));
                        Base.changePosition(a);
                    }
                    else if(int.TryParse(code[1],out a))
                    {
                        Base.changePosition(a);
                    }
                    else if (code[1] == ">>")
                    {
                        foreach (string i in Base.lines)
                        {
                            if(i == ">> " + code[2])
                            {
                                D.setI(code[2], Base.get_position());
                                Base.positions.Add(Base.get_position());
                                Base.changePosition(Base.lines.IndexOf(i));
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e + " Line: " + Base.get_position());
                }

            }
        }
        public class line_number : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                try
                {
                    if (D.inint(code[1]))
                    {
                        int x = Base.get_position();
                        D.setI(code[1], (x));
                    }
                    
                }
                catch(Exception e)
                {
                    Console.WriteLine(e + " Line: " + Base.get_position());
                }
            }
        }
        /*public class add : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    if (code[2] == "ReFd")
                    {
                        if (code[4] == "ReFd")
                        {
                            D.setD(code[1], (D.referenceD(code[3]) + D.referenceD(code[5])));
                        }
                        else
                        {
                            D.setD(code[1], (D.referenceD(code[3]) + Double.Parse(code[4])));
                        }
                    }
                    else if (code[3] == "ReFd")
                    {
                        D.setD(code[1], (Double.Parse(code[2]) + D.referenceD(code[4])));
                    }
                    else
                    {
                        D.setD(code[1], (Double.Parse(code[2]) + Double.Parse(code[3])));
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e + " Line: " + Base.get_position());
                }
                

            }
        }
        public class subtract : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    if (code[2] == "ReFd")
                    {
                        if (code[4] == "ReFd")
                        {
                            D.setD(code[1], (D.referenceD(code[3]) - D.referenceD(code[5])));
                        }
                        else
                        {
                            D.setD(code[1], (D.referenceD(code[3]) - Double.Parse(code[4])));
                        }
                    }
                    else if (code[3] == "ReFd")
                    {
                        D.setD(code[1], (Double.Parse(code[2]) - D.referenceD(code[4])));
                    }
                    else
                    {
                        D.setD(code[1], (Double.Parse(code[2]) - Double.Parse(code[3])));
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e + " Line: " + Base.get_position());
                }
                

            }
        }
        public class multiply : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    if (code[2] == "ReFd")
                    {
                        if (code[4] == "ReFd")
                        {
                            D.setD(code[1], (D.referenceD(code[3]) * D.referenceD(code[5])));
                        }
                        else
                        {
                            D.setD(code[1], (D.referenceD(code[3]) * Double.Parse(code[4])));
                        }
                    }
                    else if (code[3] == "ReFd")
                    {
                        D.setD(code[1], (Double.Parse(code[2]) * D.referenceD(code[4])));
                    }
                    else
                    {
                        D.setD(code[1], (Double.Parse(code[2]) * Double.Parse(code[3])));
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e + " Line: " + Base.get_position());
                }
                
            }
        }
        public class divide : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    if (code[2] == "ReFd")
                    {
                        if (code[4] == "ReFd")
                        {
                            D.setD(code[1], (D.referenceD(code[3]) / D.referenceD(code[5])));
                        }
                        else
                        {
                            D.setD(code[1], (D.referenceD(code[3]) / Double.Parse(code[4])));
                        }
                    }
                    else if (code[3] == "ReFd")
                    {
                        D.setD(code[1], (Double.Parse(code[2]) / D.referenceD(code[4])));
                    }
                    else
                    {
                        D.setD(code[1], (Double.Parse(code[2]) / Double.Parse(code[3])));
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e + " Line: " + Base.get_position());
                }
            }
        }*/
        /*public void activate_part(string name_of_type, CommandRegistry c)
        {
            if (name_of_type == "screen")
            {
                windows window = new windows();
                c.add_command("window", window);

            }
        }*/
        /*public class Data
        {
            Dictionary<string, string> strings = new Dictionary<string, string>();
            Dictionary<string, double> doubles = new Dictionary<string, double>();
            public string referenceS(string key)
            {
                if (strings.ContainsKey(key))
                {
                    return strings[key];
                }
                else
                {
                    return null;
                }
            }

            public double referenceD(string key)
            {
                if (doubles.ContainsKey(key))
                {
                    return doubles[key];
                }
                else
                {
                    return 0;
                }
            }
            public void setS(string key, string data)
            {
                if (strings.ContainsKey(key))
                {
                    strings.Remove(key);
                }
                strings.Add(key, data);
            }
            public void setD(string key, double data)
            {
                if (doubles.ContainsKey(key))
                {
                    doubles.Remove(key);
                }
                doubles.Add(key, data);
            }

        }*/
        
    }


}