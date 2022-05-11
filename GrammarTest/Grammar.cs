using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

/// <summary>
/// also, mare atentie si cand pui mai multe declaratii de un tip, dar nu ai variabile destule, iar intra in loop
//de pus notificari daca ai nr de decl mai mare ca nr de var de tipul ala
//also, var din expresii si cele din muststmt
//verifici existenta unor campuri obligatorii

/// </summary>

namespace GrammarTest
{
    public class Grammar
    {
        private Dictionary<string, List<List<string>>> rules;
        private List<string> terminals;
        private List<string> nonTerminals;

        private List<string> getTerminals(List<List<string>> terminalCategories)
        {
            List<string> terminals=new List<string>();

            for (int i = 0; i < terminalCategories.Count; i++)
                for (int j = 0; j < terminalCategories[i].Count; j++)
                    terminals.AddRange(rules[terminalCategories[i][j]].SelectMany(list => list).ToList());

            return terminals;
        }

        public string isMacro(string symbol)
        {
            if (Regex.IsMatch(symbol, "^rand\\(\\d*,\\d*\\)$"))
                return genNumCharacter(symbol);
            else
                return null;
        }

        public string isTerminal(string symbol)
        {
            if (terminals.Contains(symbol))
                return symbol;
            else
                return null;
        }

        public bool CheckIfTerminalOrMacro(string symbol)
        {
            if (isTerminal(symbol) != null || isMacro(symbol) != null)
                return true;
            else
                return false;
        }

        public bool CheckIfSymbolOrMacro(string symbol)
        {
            if (CheckIfTerminalOrMacro(symbol) || nonTerminals.Contains(symbol))
                return true;
            else
                return false;
        }

        private bool checkGrammarCorrectness()
        {
            foreach (var nonTerminal in nonTerminals)
            {
                List<string> symbols = rules[nonTerminal].SelectMany(list => list).Distinct().ToList();
                foreach (var symbol in symbols)
                {
                    if(!CheckIfSymbolOrMacro(symbol))
                        throw new GrammarException($"The symbol {symbol} does not have an associated rule.");
                }
            }
            return true;
        }

        public Grammar(string filePath)
        {
            string grammarRulesJSON = File.ReadAllText(filePath);
            var rules = JsonSerializer.Deserialize<Dictionary<string, List<List<string>>>>(grammarRulesJSON);

            this.rules = rules;
            terminals = getTerminals(this.rules["terminals"]);
            nonTerminals = getNonTerminals();
            checkGrammarCorrectness();
        }

        public List<List<string>> GetRules(string ruleName)
        {
            if (nonTerminals.Contains(ruleName))
                return rules[ruleName];
            else
                return null;
        }

        private List<string> getNonTerminals()
        {
            List<string> nonTerminals = new List<string>();

            nonTerminals.AddRange(rules.Keys);
            /*nonTerminals.Remove("terminals");
            var terminalCategories = rules["terminals"].SelectMany(list => list).Distinct().ToList();
            foreach (var terminalCategory in terminalCategories)
                nonTerminals.Remove(terminalCategory);*/

            return nonTerminals;
        }

        private string genNumCharacter(string symbol)
        {
            Regex digitRegex = new Regex("^rand\\((?<firstDigit>\\d*),(?<secondDigit>\\d*)\\)$");
            Match digitMatch = digitRegex.Match(symbol);

            Random rand = new Random();

            if (digitMatch.Success)
            {
                int firstDigit = Int32.Parse(digitMatch.Groups["firstDigit"].Value);
                int secondDigit = Int32.Parse(digitMatch.Groups["secondDigit"].Value);
                return rand.Next(firstDigit, secondDigit+1).ToString();
            }

            return "";
        }
    }
}

//rand cu litere ar incurca destul de mult situatia
//declaratiile vor fi scrise implicit in gramatica
//oricum codul nu trebuie sa fie mare
//constrangere ca putem lucra doar cu pointeri si int
//in expresii constrangeri cu int
//la verificarea stmt-urilor, nu se verifica daca sunt corecte, ci doar daca contin variabile
//niste iteratii obligatorii la while uri


//operatori compusi trebuie sa fie pusi in lista inaintea celor simpli

//dictionar ca sa nu se repete declaratiile
//la stmt nu e nevoie de asa ceva, se pot repeta, de asemenea problema cu infinte loop nu exista, deoarece se poate pune de mai multe ori un stmt
//totusi as putea pune ca la nivel de stmts sa nu se repete...

//3. trebuie sa ai stmts cu elementele declarate, daca nu, face loop infinit
//if symbol e stmt
//trim pe symboluri de stmt (asignari, incremntari)
//split pe whitespace
//delete elem care sunt numere
//ar trebui sa ramana doar pointeri sau variabile
//daca un element din lista ramasa contine ->
//split by ->
//si ia doar primul elem din split-uiala
//si fa ce faci si la celalalt, te uiti daca e in tabela de simboluri
//daca nu e in tabela, return false, si mergi o iteratie in spate

//4.
//daca symbolul de start contine expr si nu e e expr
//trim pe symboluri de expresii
//split pe whitespace
//delete elem care sunt numere
//ar trebui sa ramana doar pointeri sau variabile
//verifici daca sunt in tabela de simboluri
//daca sunt, le pui pe stiva cu expresii

//de citit template ca sa nu fac multe citiri de pe disk

//impartirea gramaticii in tipul liniei si linia, ca sa pot sterge mai usor liniile identice, sau ca sa vad scope-ul