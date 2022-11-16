using System;
using System.Collections.Generic;
using TYAPK;

List<Terminal> terminals = new List<Terminal> { new Terminal("a"), new Terminal("b"), new Terminal("c")};
List<NonTerminal> nonTerminals = new List<NonTerminal> { new NonTerminal("XXX"),new NonTerminal("A"), new NonTerminal("B"), new NonTerminal("C"), new NonTerminal("D"), new NonTerminal("E")};
List<GrammaticRule> rules = new List<GrammaticRule> { new GrammaticRule("A->abc"), new GrammaticRule("B->bc"), new GrammaticRule("D->AB"), new GrammaticRule("C->D"), new GrammaticRule("E->C") };
NonTerminal axiom = new NonTerminal("E");
Grammar grammar = new Grammar(nonTerminals, terminals, rules, axiom);
Console.WriteLine(grammar.isEmptyLanguage());