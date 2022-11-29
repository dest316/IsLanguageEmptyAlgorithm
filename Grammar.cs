using System;

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
    }
    public class Terminal: Symbol
    {
        public Terminal(string value): base(value)
        {
        }
    }
    public class GrammaticRule
    {
        public NonTerminal leftExpression { get; set; }
        public List<Symbol> rightExpression { get; set; }
        public bool correctRule = true;
        public GrammaticRule(string expr)
        {
            try
            {              
                var arr = expr.Split("->");
                if (arr.Length != 2) { throw new ArgumentException("Ошибка: неверный формат правила грамматики"); }
                leftExpression = (arr[0][0] == '<' && arr[0][arr[0].Length - 1] == '>') ? new NonTerminal(arr[0][1..(arr[0].Length - 1)]) : new NonTerminal(arr[0]);
                //вместо рэнджа удалять ВСЕ угольные скобочки в примере
                List<Symbol> list = new List<Symbol>();
                for (int i = 0; i < arr[1].Length; i++)
                {
                    if (!arr[1][i].Equals('<'))
                        list.Add(new Terminal(arr[1][i].ToString()));
                    else
                    {
                        string newNonTerminalValue = "";
                        char cur = arr[1][i];
                        while (i < arr[1].Length)
                        {
                            i++;
                            cur = arr[1][i];
                            if (cur == '>')
                                break;
                            newNonTerminalValue += cur;                           
                        }
                        if (cur == '>')
                        {
                            list.Add(new NonTerminal(newNonTerminalValue));
                        }
                        else
                            throw new ArgumentException();
                    }
                }
                rightExpression = list;
            }
            catch (ArgumentException e) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
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
            try
            {
                this.nonTerminals = nonTerminals ?? new List<NonTerminal>();
                this.terminals = terminals ?? new List<Terminal>();
                this.grammaticRules = grammaticRules ?? new List<GrammaticRule>();
                bool correctAxiomFlag = false;
                foreach (var nontr in nonTerminals)
                {
                    if (axiom.Equals(nontr)) { correctAxiomFlag = true; }
                }
                if (!correctAxiomFlag) { throw new ArgumentException("Ошибка: Аксиома не содержится в списке нетерминальных символов"); }
                this.axiom = axiom ?? new NonTerminal();
                foreach (var gr in grammaticRules)
                {
                    if (!nonTerminals.Contains(gr.leftExpression))
                    {
                        gr.correctRule = false;
                    }
                    if (gr.rightExpression == null)
                    {
                        return;
                    }
                    foreach (var nt in gr.rightExpression)
                    {
                        if (!nonTerminals.Contains(nt) && !terminals.Contains(nt)) //unsafe cast mb
                        {
                            throw new ArgumentException("Встречен неопознанный символ");
                        }
                    }
                }
            }
            catch (ArgumentException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (NullReferenceException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Правило имело неверный формат");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        private bool isTerminalRule(GrammaticRule rule)
        {
            if (rule.leftExpression == null || rule.rightExpression == null)
            { return false; }
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
        public HashSet<GrammaticRule> GetGoodGrammaticRules()
        {
            int i = 1;
            HashSet<GrammaticRule> set = new(); // создаем множество правил
            List<GrammaticRule> tempList = new();
            foreach (GrammaticRule grammaticRule in grammaticRules)
            {
                if (grammaticRule.correctRule)
                {
                    if (isTerminalRule(grammaticRule)) { set.Add(grammaticRule); } //если правило "хорошее", то добавляем в сет
                    else { tempList.Add(grammaticRule); }
                }
            }
            bool doCycle = true;
            while (doCycle)
            {
                doCycle = false;
                foreach (var rule in tempList)
                {
                    if (rule.rightExpression == null) { continue; }
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
            return set;
        }
        public bool isEmptyLanguage()
        {
            
            HashSet<GrammaticRule> set = GetGoodGrammaticRules(); // создаем множество правил
            
            
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
