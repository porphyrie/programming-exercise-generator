using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GrammarTest
{
    public class Generator
    {
        private Configuration configuration;
        private string template;

        public Generator(Configuration configuration)
        {
            this.configuration = configuration;
            int ok = 0;
            foreach (var line in File.ReadAllLines(configuration.GetTemplatePath()))
            {
                if (line.Contains("main"))
                    ok = 1;
                template += line + "\n";
            }
            if (ok == 0)
                throw new GeneratorException("Template file does not contain a main function.");
        }

        private bool checkLineCount(List<string> programCodeLines)
        {
            if ((programCodeLines.Count > configuration.GetMinLineCount()) && (programCodeLines.Count < configuration.GetMaxLineCount()))
                return true;
            else
                return false;
        }

        private bool checkUniqueness(List<string> programCodeLines)
        {
            for (int i = 0; i < programCodeLines.Count; i++)
                programCodeLines[i] = programCodeLines[i].TrimStart();
            //programCodeLines.RemoveAll(line => line.Equals("{"));
            //programCodeLines.RemoveAll(line => line.Equals("}"));
            //programCodeLines.RemoveAll(line => line.Contains("else"));
            //programCodeLines.Remove(programCodeLines[0]);

            int lineCount = programCodeLines.Count;
            int uniqueLineCount = programCodeLines.Distinct().ToList().Count();
            float uniqueness = (float)uniqueLineCount / lineCount;

            if (uniqueness > configuration.GetMinUniqueness())
                return true;
            else
                return false;
        }

        private bool checkFunction(string programCode)
        {
            List<string> programCodeLines = programCode.TrimEnd().Split('\n').ToList();

            if (checkLineCount(programCodeLines) && checkUniqueness(programCodeLines))
                return true;
            else
                return false;
        }

        public string GenFunction(string startSymbol, Grammar programGrammar)
        {
            string programCode = null;

            while (true)
            {
                try
                {
                    SyntaxTree syntaxTree = new SyntaxTree(startSymbol, programGrammar);

                    programCode = syntaxTree.GetTerminalDefinition();
                    programCode = formatTerminalDefinition(programCode);

                    if (!checkFunction(programCode))
                        throw new GeneratorException("The generated function didn't meet the configuration criteria.");

                    break;
                }
                catch (SyntaxTreeException e)
                {
                    continue;
                }
                catch (GeneratorException e)
                {
                    continue;
                }

            };

            return programCode;
        }

        private string formatTerminalDefinition(string programCode)
        {
            string output;

            var processStartInfo = new ProcessStartInfo()
            {
                FileName = configuration.GetClangFormatPath(),
                Arguments = configuration.GetClangFormatArgs(),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var process = Process.Start(processStartInfo);
            process.StandardInput.WriteLine(programCode);
            process.StandardInput.Close();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }

        public void CustomizeTemplate(string programCode)
        {
            var sourceCode = new StringBuilder();
            var stringReader = new StringReader(template);
            string line;
            while ((line = stringReader.ReadLine()) != null)
            {
                if (line.Contains("main"))
                    sourceCode.AppendLine(programCode);
                sourceCode.AppendLine(line);
            }
            File.WriteAllText(configuration.GetCustomizedTemplatePath(), sourceCode.ToString());
        }

        public void CompileCustomizedTemplate()
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = @"g++.exe",
                Arguments = @$"{configuration.GetCustomizedTemplatePath()} -o {configuration.GetExecutablePath()}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            var process = Process.Start(processStartInfo);
            var output = process.StandardOutput.ReadToEnd();
            var err = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (err.Length > 0)
                throw new GeneratorException("The customized template can't be compiled.\n" + err);
        }

        public string RunCustomizedTemplate()
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = @$"{configuration.GetExecutablePath()}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            var process = Process.Start(processStartInfo);
            var output = process.StandardOutput.ReadToEnd();
            var err = process.StandardError.ReadToEnd();
            int exitCode = process.ExitCode;
            process.WaitForExit();

            if (exitCode != 1)
                throw new GeneratorException("The customized template had a runtime error.");

            return output;
        }

        private (string question, string answer) processOutput(string output)
        {
            string question = configuration.GetQuestion();
            string patternToReplaceInQuestion = configuration.GetPatternToReplaceInQuestion();
            string patternToDetectHypothesisContent = configuration.GetPatternToDetectHypothesisContent();
            string patternToDetectCorrectAnswer = configuration.GetPatternToDetectCorrectAnswer();

            Regex hypothesisRegex = new Regex($"{patternToDetectHypothesisContent}(?<hypothesisContent>(.|\n|\r)*){patternToDetectHypothesisContent}");
            Match hypothesisMatch = hypothesisRegex.Match(output);

            Regex answerRegex = new Regex($"{patternToDetectCorrectAnswer}(?<correctAnswer>(.|\n|\r)*){patternToDetectCorrectAnswer}");
            Match answerMatch = answerRegex.Match(output);

            if (!hypothesisMatch.Success || !answerMatch.Success)
                throw new GeneratorException("The output of the program does not match the patterns provided.");

            string hypothesisContent = hypothesisMatch.Groups["hypothesisContent"].Value.Trim();
            string correctAnswer = answerMatch.Groups["correctAnswer"].Value.Trim();

            question = Regex.Replace(question, patternToReplaceInQuestion, hypothesisContent);

            return (question, correctAnswer);
        }

        public (string question, string programCode, string answer) Generate()
        {
            string programCode, output;

            Grammar programGrammar = new Grammar(configuration.GetGrammarPath());
            string startSymbol = configuration.GetStartSymbol();

            while (true)
            {
                try
                {
                    programCode = GenFunction(startSymbol, programGrammar).Trim();

                    CustomizeTemplate(programCode);
                    CompileCustomizedTemplate();
                    output = RunCustomizedTemplate();

                    break;
                }
                catch (GeneratorException e)
                {
                    continue;
                }
            };


            string question, answer;
            (question, answer) = processOutput(output);
            
            return (question, programCode, answer);
        }
    }
}
