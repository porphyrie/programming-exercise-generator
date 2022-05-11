using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

//fisier cu tipuri de date
//fisier cu simboluri terminale, keywords
//fisier cu gramatica

//as putea pune anumite cuvinte cheie in gramatici, care sa reprezinte acelasi lucru pentru toata lumea
//gen daca pun cuvantul number intr-o gramatica sa se genereze un numar random, indiferent de gramatica

//poti sa pui o limita de linii
//gen sa genereze un program intre 10-20 linii

namespace GrammarTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            string nodeTypeJSON =
            @"{
            ""TypeName"": ""nod"",
            ""Members"": {
                ""n"": ""int"",
                ""*urm"": ""nod""
            },
            ""NestedBlockLevel"": 0
            }";

            //The class you are deserializing, or one of the classes included as a property of that class, has a constructor that takes arguments, and doesn't also have a default constructor.
            //The deserializer isn't able to create an instance of that class, since it doesn't have the parameters to pass to that constructor.
            var nod = JsonSerializer.Deserialize<TypeCollection.Type>(nodeTypeJSON);

            //Console.WriteLine($"typename: {nod.TypeName}");
            //Console.WriteLine($"members: {nod.Members["n"]} {nod.Members["*urm"]}");
            //Console.WriteLine($"nestedBlockLevel: {nod.NestedBlockLevel}");

            var types = new TypeCollection();
            types.AddType(nod);

            //Console.WriteLine(types.GenDecl());

            //as putea sa pun anumite productii sa fie mai likely sa fie alese, gen un numar intre 0-10, suma max 10 pt fiecare regula
            //ies o gramada cu factor, clar trebuie preferinte
            //string grammarRulesJSON =
            //@"{
            //""program"": [
            //    [""block""]
            //],
            //""block"": [
            //    [""{"", ""\n"", ""decls"", ""stmts"", ""}""]
            //],
            //""decls"": [
            //    [""decls"", ""decl""],
            //    [""decl""],
            //    [""""]
            //],
            //""decl"": [
            //    [""type"", "" "", ""id"", "";"", ""\n""]
            //],
            //""stmts"": [
            //    [""stmts"", ""stmt""], 
            //    [""stmt""]
            //],
            //""stmt"": [
            //    [""flowstmt""],
            //    [""statement"", "";"", ""\n""]
            //],
            //""flowstmt"": [
            //    [""if"", "" "", ""("", ""expr"", "")"", ""\n"", ""stmt""],
            //    [""if"", "" "", ""("", ""expr"", "")"", ""\n"", ""stmt"", ""else"", ""\n"", ""stmt""],
            //    [""while"", "" "", ""("", ""expr"", "")"", "" "", ""block"", ""\n""], 
            //    [""do"", ""\n"", ""block"", "" "", ""while"", "" "", ""("", ""expr"", "")"", "";"", ""\n""]
            //]
            //}";

            //string boolJSON =
            //@"{
            //""bool"": [
            //    [""bool"", ""||"", ""join""], 
            //    [""join""]
            //],
            //""join"": [
            //    [""join"", ""&&"", ""equality""], 
            //    [""equality""]
            //],
            //""equality"": [
            //    [""equality"", ""=="", ""rel""], 
            //    [""equality"", ""!="", ""rel""], 
            //    [""rel""]
            //],
            //""rel"": [
            //    [""expr"", ""<"", ""expr""], 
            //    [""expr"", ""<="", ""expr""], 
            //    [""expr"", "">="", ""expr""], 
            //    [""expr"", "">"", ""expr""], 
            //    [""expr""]
            //],
            //""expr"": [
            //    [""expr"", ""+"", ""term""],
            //    [""expr"", ""-"", ""term""], 
            //    [""term""]
            //],
            //""term"": [
            //    [""term"",""*"",""factor""], 
            //    [""term"", ""/"", ""factor""], 
            //    [""factor""]
            //]
            //}";

            //mai merge sa segementez operand in operand 1 si operand 2, ca sa pot face operand1 != operand2
            //expresii cu not
            //cum rezolv cu stmt-ul ala cu p->null
            //sol: nu fac while-uri, cum prioritizez while-urile cu aux si s, cresc probabilitatea ca alea cu p sa fie alese

            //daca contine o inserare sau o stergere, pun intrebarea cu care sunt nodurile
            //daca are doar variabila p, pun intrebarea cu care e val lui p->n
            //daca are variabila s, pun intrebare cu care e val lui s

            //putine inserari, putine stergeri
            //de preferat 1, vezi cum controlezi expresia

            //if, else sa nu aiba aceleasi expresii
            //sa fac scaderi cu operatori diferiti

            string configurationPath= @"C:\Users\dpili\source\repos\GrammarTest\GrammarTest\Configuration.json";
            string customizedTemplatePath = @"C:\Users\dpili\source\repos\GrammarTest\GrammarTest\Example.cpp";
            string executablePath = @"C:\Users\dpili\source\repos\GrammarTest\GrammarTest\Result.exe";

            var configuration = new Configuration(configurationPath);
            configuration.SetCustomizedTemplatePath(customizedTemplatePath);
            configuration.SetExecutablePath(executablePath);

            var generator = new Generator(configuration);

            string question, programCode, answer;

            (question, programCode, answer) = generator.Generate();

            Console.WriteLine(question);
            Console.WriteLine(programCode);
            Console.WriteLine(answer);

        }
    }
}