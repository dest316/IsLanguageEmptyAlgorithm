using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;

namespace TYAPK
{
    public abstract class Symbol
    {
        public string value { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null) { return false; }
            if (obj is not Symbol) { return false; }
            Symbol other = obj as Symbol;
            return other.value.Equals(this.value);
        }
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
        public Symbol()
        {

        }
        public Symbol(string value)
        {
            this.value = value;
        }
    }
    public class NonTerminal: Symbol
    {
        public NonTerminal(string value) : base(value)
        {
        }
        public NonTerminal()
        {

        }

        public string value { get; set; }
    }
    public class Terminal: Symbol
    {
        public Terminal(string value): base(value)
        {
        }

        public string value { get; set; }
    }
    public class GrammaticRule
    {
        public NonTerminal leftExpression { get; set; }
        public List<Symbol> rightExpression { get; set; }
        public GrammaticRule(string expr)
        {
            try
            {
                var arr = expr.Split("->");
                if (arr.Length != 2) { throw new ArgumentException(); }
                leftExpression = new NonTerminal(arr[0]);
                List<Symbol> list = new List<Symbol>();
                foreach (char sym in arr[1]) { list.Add(new Terminal(sym.ToString())); }
                rightExpression = list;
            }
            catch (Exception) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: неверный формат правила грамматики");
                Console.BackgroundColor = ConsoleColor.White;
            }
        }
        
    }
    

    internal class Grammar
    {
        public List<NonTerminal> nonTerminals { get; set; }
        public List<Terminal> terminals { get; set; }
        public List<GrammaticRule> grammaticRules { get; set; }
        public NonTerminal axiom;
        public Grammar(List<NonTerminal> nonTerminals, List<Terminal> terminals, List<GrammaticRule> grammaticRules, NonTerminal axiom)
        {
            this.nonTerminals = nonTerminals ?? new List<NonTerminal>();
            this.terminals = terminals ?? new List<Terminal>();
            this.grammaticRules = grammaticRules ?? new List<GrammaticRule>();
            this.axiom = axiom ?? new NonTerminal();
        }
        public bool isTerminalRule(GrammaticRule rule)
        {
            foreach (Symbol symbol in rule.rightExpression)
            {
                if (terminals.Contains(symbol))
                {
                    foreach (Symbol symbol1 in nonTerminals)
                    {
                        if (symbol.Equals(symbol1)) { return false; }
                    }
                }
            }
            return true;
        }
        public bool isEmptyLanguage()
        {
            int i = 1;
            HashSet<GrammaticRule> set = new(); // создаем множество правил
            List<GrammaticRule> tempList = new();
            foreach (GrammaticRule grammaticRule in grammaticRules) 
            {
                if (isTerminalRule(grammaticRule)) { set.Add(grammaticRule); } //если правило "хорошее", то добавляем в сет
                else { tempList.Add(grammaticRule); }
            }
            bool doCycle = true;
            while (doCycle)
            {
                doCycle = false;
                foreach (var rule in tempList)
                {
                    foreach (var sym in rule.rightExpression)
                    {
                        foreach (var nonterm in set)
                        {
                            if (nonterm.leftExpression.Equals(sym))
                            {
                                doCycle = true;
                                set.Add(rule);
                                tempList.Remove(rule);
                                i++;
                            }
                        }
                    }
                }
            }
            foreach (var rule in set)
            {
                if (rule.leftExpression.Equals(axiom))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
